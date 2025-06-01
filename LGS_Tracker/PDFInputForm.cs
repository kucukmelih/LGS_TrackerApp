using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using MySql.Data.MySqlClient;

namespace LGS_Tracker
{
    public partial class OCRandPDFInputForm : Form
    {
        private string selectedFilePath = "";

        // Dictionary to map normalized Turkish subject names to English
        private readonly Dictionary<string, string> TranslateSubject = new Dictionary<string, string>
        {
            { "TURKCE", "Turkish" },
            { "INKILAP TARIHI", "History" },
            { "DIN KULTURU VE AHLAK BILGISI", "Religious Culture" },
            { "INGILIZCE", "English" },
            { "MATEMATIK", "Mathematics" },
            { "FEN BILIMLERI", "Science" }
        };

        // Default question counts for each subject
        private readonly Dictionary<string, int> TotalQuestions = new Dictionary<string, int>
        {
            { "Turkish", 20 },
            { "History", 10 },
            { "Religious Culture", 10 },
            { "English", 10 },
            { "Mathematics", 20 },
            { "Science", 20 }
        };

        public OCRandPDFInputForm()
        {
            InitializeComponent();
            Load += OCRandPDFInputForm_Load;
            btnSelectPDF.Click += BtnSelectPDF_Click;
            btnSaveToDB.Click += BtnSaveToDB_Click;
            btnBack.Click += BtnBack_Click;
        }

        // On form load, apply theme and populate students
        private void OCRandPDFInputForm_Load(object sender, EventArgs e)
        {
            txtOutput.Text = string.Empty;
            LoadStudents();
            ApplyTheme();
        }

        // Apply light theme to the form
        private void ApplyTheme()
        {
            BackColor = Color.FromArgb(240, 248, 255);

            foreach (Control ctrl in Controls)
            {
                if (ctrl is Label)
                    ctrl.ForeColor = Color.Black;
                else if (ctrl is TextBox || ctrl is ComboBox)
                {
                    ctrl.BackColor = Color.White;
                    ctrl.ForeColor = Color.Black;
                }
                else if (ctrl is Button)
                {
                    Button btn = (Button)ctrl;
                    btn.BackColor = Color.FromArgb(100, 149, 237);
                    btn.ForeColor = Color.Black;
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderSize = 1;
                    btn.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
                }
            }
        }

        // Load students from database into ComboBox
        private void LoadStudents()
        {
            try
            {
                const string query = "SELECT user_id, full_name FROM users WHERE role = 'student'";
                DataTable students = DB.ExecuteQuery(query);

                DataRow newRow = students.NewRow();
                newRow["user_id"] = DBNull.Value;
                newRow["full_name"] = "Select Student";
                students.Rows.InsertAt(newRow, 0);

                cmbStudent.DataSource = students;
                cmbStudent.DisplayMember = "full_name";
                cmbStudent.ValueMember = "user_id";
                cmbStudent.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load students: " + ex.Message);
            }
        }

        // PDF selection button
        private void BtnSelectPDF_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog { Filter = "PDF files (*.pdf)|*.pdf" })
            {
                if (ofd.ShowDialog() != DialogResult.OK) return;

                selectedFilePath = ofd.FileName;
                lblSelectedFile.Text = selectedFilePath;
                ExtractFromPDF(selectedFilePath);
            }
        }

        // Normalize Turkish characters for comparison
        private static string Normalize(string input)
        {
            string upper = input.ToUpper(new CultureInfo("tr-TR", false));
            return Regex.Replace(upper.Replace("İ", "I").Replace("Ğ", "G").Replace("Ü", "U")
                .Replace("Ö", "O").Replace("Ş", "S").Replace("Ç", "C"), "\\p{Mn}", "").Trim();
        }

        // Extract exam results from PDF file
        private void ExtractFromPDF(string filePath)
        {
            try
            {
                txtOutput.Clear();
                List<string> subjectsOrdered = new List<string> { "Turkish", "History", "Religious Culture", "English", "Mathematics", "Science" };
                Dictionary<string, Tuple<int?, int?, int?>> subjectsResult = new Dictionary<string, Tuple<int?, int?, int?>>();

                string allText = string.Empty;
                using (PdfReader reader = new PdfReader(filePath))
                {
                    for (int i = 1; i <= reader.NumberOfPages; i++)
                        allText += PdfTextExtractor.GetTextFromPage(reader, i);
                }

                // Locate "DERS ANALİZİ" section
                string[] lines = allText.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                int startIndex = Array.FindIndex(lines, line => line.ToUpper().Contains("DERS ANALİZİ")) + 1;
                if (startIndex <= 0)
                {
                    MessageBox.Show("DERS ANALİZİ section not found.");
                    return;
                }

                // Collect 6 lines corresponding to 6 subjects
                List<string> dataLines = new List<string>();
                for (int i = startIndex; i < lines.Length && dataLines.Count < 6; i++)
                {
                    string line = lines[i].Trim();
                    if (line.StartsWith("LGS-")) dataLines.Add(line);
                }

                if (dataLines.Count != 6)
                {
                    MessageBox.Show("Expected 6 subject lines but found a different number.");
                    return;
                }

                // Parse each subject's result
                for (int i = 0; i < dataLines.Count; i++)
                {
                    string subject = subjectsOrdered[i];
                    string line = dataLines[i];

                    List<Match> rawMatches = Regex.Matches(line, @"\d+([.,]\d+)?").Cast<Match>().ToList();
                    List<int> numbers = new List<int>();

                    foreach (var match in rawMatches)
                    {
                        string value = match.Value;
                        if (!value.Contains(",") && !value.Contains("."))
                            numbers.Add(int.Parse(value));
                    }

                    if (numbers.Count < 2)
                    {
                        MessageBox.Show($"Not enough data found in line for {subject}");
                        continue;
                    }

                    int total = numbers[0];
                    int correct = numbers[1];
                    int incorrect = numbers.Count > 2 ? numbers[2] : 0;
                    int blank = numbers.Count > 3 ? numbers[3] : Math.Max(0, total - correct - incorrect);

                    if (TotalQuestions.ContainsKey(subject))
                        TotalQuestions[subject] = total;

                    subjectsResult[subject] = new Tuple<int?, int?, int?>(correct, incorrect, blank);
                }

                // Output parsed results
                txtOutput.Text = string.Join(Environment.NewLine,
                    subjectsResult.Select(s =>
                        string.Format("{0} - Correct: {1}, Incorrect: {2}, Blank: {3}",
                                      s.Key,
                                      s.Value.Item1 ?? 0,
                                      s.Value.Item2 ?? 0,
                                      s.Value.Item3 ?? 0)));
            }
            catch (Exception ex)
            {
                MessageBox.Show("PDF parse error: " + ex.Message);
            }
        }

        // Save parsed exam results to database
        private void BtnSaveToDB_Click(object sender, EventArgs e)
        {
            if (cmbStudent.SelectedIndex == 0 || cmbStudent.SelectedValue is DBNull || string.IsNullOrWhiteSpace(txtOutput.Text))
            {
                MessageBox.Show("Please select a student and extract data first.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDescription.Text))
            {
                MessageBox.Show("Please enter a description for the exam.");
                return;
            }

            int userId = Convert.ToInt32(cmbStudent.SelectedValue);
            int studentId = GetStudentIdByUserId(userId);
            string examDescription = txtDescription.Text.Trim();

            // Insert exam row
            object examIdObj = DB.ExecuteScalar(
                "INSERT INTO exams(student_id, exam_date, exam_description) VALUES(@sid, NOW(), @desc); SELECT LAST_INSERT_ID();",
                new MySqlParameter("@sid", studentId),
                new MySqlParameter("@desc", examDescription));

            if (examIdObj == null)
            {
                MessageBox.Show("Exam insert failed.");
                return;
            }

            int examId = Convert.ToInt32(examIdObj);

            // Map English subject names back to Turkish for DB
            Dictionary<string, string> reverseSubjectMap = new Dictionary<string, string>
            {
                { "Mathematics", "Matematik" },
                { "Science", "Fen Bilimleri" },
                { "Turkish", "Türkçe" },
                { "History", "İnkılap Tarihi" },
                { "Religious Culture", "Din Kültürü" },
                { "English", "İngilizce" }
            };

            List<Tuple<string, int, int>> results = new List<Tuple<string, int, int>>();

            // Parse output box line-by-line and insert results
            foreach (string line in txtOutput.Text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {
                Match match = Regex.Match(line, @"^(.*?) - Correct: (\d+), Incorrect: (\d+), Blank: (\d+)");
                if (!match.Success) continue;

                string englishSubject = match.Groups[1].Value.Trim();
                string turkishSubject = reverseSubjectMap.ContainsKey(englishSubject) ? reverseSubjectMap[englishSubject] : null;

                if (turkishSubject == null)
                {
                    MessageBox.Show($"'{englishSubject}' is not a valid subject.");
                    continue;
                }

                int correct = int.Parse(match.Groups[2].Value);
                int incorrect = int.Parse(match.Groups[3].Value);
                int blank = int.Parse(match.Groups[4].Value);

                DB.ExecuteNonQuery(
                    "INSERT INTO results(exam_id, subject, correct, incorrect, blank) VALUES(@eid, @subject, @correct, @incorrect, @blank);",
                    new MySqlParameter("@eid", examId),
                    new MySqlParameter("@subject", turkishSubject),
                    new MySqlParameter("@correct", correct),
                    new MySqlParameter("@incorrect", incorrect),
                    new MySqlParameter("@blank", blank));

                results.Add(new Tuple<string, int, int>(turkishSubject, correct, incorrect));
            }

            // Calculate and update final score
            double score = CalculateLGSScore(results);

            DB.ExecuteNonQuery("UPDATE exams SET score = @score WHERE exam_id = @eid",
                new MySqlParameter("@score", score),
                new MySqlParameter("@eid", examId));

            MessageBox.Show("Data saved successfully.\nScore: " + score);
        }

        // Score calculation using LGS weighted formula
        private double CalculateLGSScore(List<Tuple<string, int, int>> answers)
        {
            Dictionary<string, double> weights = new Dictionary<string, double>
            {
                { "Türkçe", 4.0 },
                { "Matematik", 4.0 },
                { "Fen Bilimleri", 4.0 },
                { "İnkılap Tarihi", 1.0 },
                { "Din Kültürü", 1.0 },
                { "İngilizce", 1.0 }
            };

            double totalWeightedNet = 0;
            double maxWeightedNet = 0;

            foreach (Tuple<string, int, int> item in answers)
            {
                string subject = item.Item1;
                int correct = item.Item2;
                int incorrect = item.Item3;

                if (!weights.ContainsKey(subject)) continue;

                double net = correct - (incorrect / 3.0);
                double weighted = net * weights[subject];

                totalWeightedNet += weighted;
                maxWeightedNet += weights[subject] * (subject == "Matematik" || subject == "Fen Bilimleri" || subject == "Türkçe" ? 20 : 10);
            }

            return Math.Round((totalWeightedNet / maxWeightedNet) * 500, 2);
        }

        // Gets the student ID from user ID
        private int GetStudentIdByUserId(int userId)
        {
            object result = DB.ExecuteScalar("SELECT student_id FROM students WHERE user_id = @uid", new MySqlParameter("@uid", userId));
            return result != null ? Convert.ToInt32(result) : -1;
        }

        // Go back / close form
        private void BtnBack_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
