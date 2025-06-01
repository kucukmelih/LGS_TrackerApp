using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MySql.Data.MySqlClient;

namespace LGS_Tracker
{
    public partial class ReportViewer : Form
    {
        private int? userId = null;

        // Constructor optionally receives a student user ID (used when opened by student)
        public ReportViewer(int? studentUserId = null)
        {
            InitializeComponent();
            this.userId = studentUserId;

            btnGenerate.Click += btnGenerate_Click;
            btnBack.Click += btnBack_Click;

            ApplyTheme();
            ApplyHoverEffect(btnGenerate);
            ApplyHoverEffect(btnBack);
        }

        // On form load, initialize UI based on whether admin or student
        private void ReportViewerForm_Load(object sender, EventArgs e)
        {
            if (userId == null)
            {
                // Admin view: show student combo box
                string query = "SELECT user_id, full_name FROM users WHERE role = 'student'";
                DataTable students = DB.ExecuteQuery(query);

                this.Size = new Size(1200, 900);
                DataRow newRow = students.NewRow();
                newRow["user_id"] = DBNull.Value;
                newRow["full_name"] = "Select Student";
                students.Rows.InsertAt(newRow, 0);

                cmbStudent.DataSource = students;
                cmbStudent.DisplayMember = "full_name";
                cmbStudent.ValueMember = "user_id";
                cmbStudent.DropDownStyle = ComboBoxStyle.DropDownList;
                cmbStudent.Visible = true;
                cmbStudent.SelectedIndex = 0;

                chkExams.Items.Clear();
                cmbStudent.SelectedIndexChanged += cmbStudent_SelectedIndexChanged;
            }
            else
            {
                // Student view: load their exams directly
                cmbStudent.Visible = false;
                int studentId = GetStudentIdByUserId(userId.Value);
                if (studentId != -1)
                    LoadExams(studentId);
            }
        }

        // When student selection changes, load their exams
        private void cmbStudent_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbStudent.SelectedIndex == 0 || cmbStudent.SelectedValue == null || cmbStudent.SelectedValue is DBNull)
            {
                chkExams.Items.Clear();
                return;
            }

            if (int.TryParse(cmbStudent.SelectedValue.ToString(), out int selectedUserId))
            {
                int studentId = GetStudentIdByUserId(selectedUserId);
                if (studentId != -1)
                    LoadExams(studentId);
            }
        }

        // Gets student ID by user ID from DB
        private int GetStudentIdByUserId(int userId)
        {
            string query = "SELECT student_id FROM students WHERE user_id = @uid";
            var result = DB.ExecuteScalar(query, new MySqlParameter("@uid", userId));
            return result != null ? Convert.ToInt32(result) : -1;
        }

        // Loads exams for a given student ID and fills checkbox list
        private void LoadExams(int studentId)
        {
            string query = @"SELECT exam_id, exam_date, exam_description
                             FROM exams
                             WHERE student_id = @sid
                             ORDER BY exam_date";

            DataTable exams = DB.ExecuteQuery(query, new MySqlParameter("@sid", studentId));

            chkExams.Items.Clear();
            foreach (DataRow row in exams.Rows)
            {
                chkExams.Items.Add($"{Convert.ToDateTime(row["exam_date"]).ToShortDateString()} - {row["exam_description"]}");
            }
        }

        // Handle PDF generation button click
        private void btnGenerate_Click(object sender, EventArgs e)
        {
            // Validate selection
            if (userId == null)
            {
                if (cmbStudent.SelectedIndex == 0 || cmbStudent.SelectedValue == null || cmbStudent.SelectedValue is DBNull)
                {
                    MessageBox.Show("Please select a student first.");
                    return;
                }
            }

            int selectedUserId = userId ?? Convert.ToInt32(cmbStudent.SelectedValue);
            int studentId = GetStudentIdByUserId(selectedUserId);

            if (chkExams.CheckedItems.Count == 0)
            {
                MessageBox.Show("Please select at least one exam to generate the PDF.");
                return;
            }

            // Get all exams for this student
            string allExamsQuery = @"SELECT exam_id, exam_date, exam_description, score
                                     FROM exams
                                     WHERE student_id = @sid
                                     ORDER BY exam_date";

            DataTable allExams = DB.ExecuteQuery(allExamsQuery, new MySqlParameter("@sid", studentId));
            var selectedDescriptions = chkExams.CheckedItems.Cast<string>().ToList();
            var selectedExams = allExams.AsEnumerable()
                .Where(row => selectedDescriptions.Contains($"{Convert.ToDateTime(row["exam_date"]).ToShortDateString()} - {row["exam_description"]}"))
                .ToList();

            if (selectedExams.Count == 0)
            {
                MessageBox.Show("Selected exam data not found.");
                return;
            }

            // Ask where to save PDF
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "PDF files (*.pdf)|*.pdf",
                FileName = "Selected_Exam_Report.pdf"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                var doc = new Document();
                PdfWriter.GetInstance(doc, new FileStream(sfd.FileName, FileMode.Create));
                doc.Open();

                // Fonts for PDF content
                var titleFont = FontFactory.GetFont("Helvetica", "Cp1254", 18f, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                var headerFont = FontFactory.GetFont("Helvetica", "Cp1254", 12f, iTextSharp.text.Font.BOLD, BaseColor.WHITE);
                var normalFont = FontFactory.GetFont("Helvetica", "Cp1254", 11f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

                // Add document title
                var title = new Paragraph("LGS Tracker - Exam Report", titleFont) { Alignment = Element.ALIGN_CENTER };
                doc.Add(title);
                doc.Add(new Paragraph("\n"));

                // Subject translations for PDF headers
                Dictionary<string, string> subjectTranslations = new Dictionary<string, string>
                {
                    {"Matematik", "Mathematics"},
                    {"Fen Bilimleri", "Science"},
                    {"Türkçe", "Turkish"},
                    {"İnkılap Tarihi", "History of Revolution"},
                    {"Din Kültürü", "Religious Culture"},
                    {"İngilizce", "English"}
                };

                // Add each selected exam into the PDF
                foreach (var exam in selectedExams)
                {
                    int examId = Convert.ToInt32(exam["exam_id"]);
                    string examTitle = $"{Convert.ToDateTime(exam["exam_date"]).ToShortDateString()} - {exam["exam_description"]}";
                    string examScore = exam["score"]?.ToString() ?? "N/A";

                    var examHeader = new Paragraph($"{examTitle} | Score: {examScore}",
                        FontFactory.GetFont("Helvetica", "Cp1254", 13f, iTextSharp.text.Font.BOLD));
                    examHeader.SpacingAfter = 5f;
                    doc.Add(examHeader);

                    // Query result data for the exam
                    string resultQuery = @"SELECT subject, correct, incorrect, blank FROM results WHERE exam_id = @eid";
                    DataTable results = DB.ExecuteQuery(resultQuery, new MySqlParameter("@eid", examId));

                    PdfPTable table = new PdfPTable(4) { WidthPercentage = 100 };

                    // Table headers
                    string[] headers = { "Subject", "Correct", "Incorrect", "Blank" };
                    foreach (string header in headers)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(header, headerFont))
                        {
                            BackgroundColor = new BaseColor(0, 122, 204),
                            HorizontalAlignment = Element.ALIGN_CENTER
                        };
                        table.AddCell(cell);
                    }

                    // Table rows: subject results
                    foreach (DataRow row in results.Rows)
                    {
                        string subjectTR = row["subject"].ToString();
                        string subjectEN = subjectTranslations.ContainsKey(subjectTR) ? subjectTranslations[subjectTR] : subjectTR;

                        table.AddCell(new Phrase(subjectEN, normalFont));
                        table.AddCell(new Phrase(row["correct"].ToString(), normalFont));
                        table.AddCell(new Phrase(row["incorrect"].ToString(), normalFont));
                        table.AddCell(new Phrase(row["blank"].ToString(), normalFont));
                    }

                    doc.Add(table);
                    doc.Add(new Paragraph("\n"));
                }

                doc.Close();
                MessageBox.Show("PDF generated successfully.");
            }
        }

        // Closes the form and returns to previous screen
        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Applies UI theme to form and controls
        private void ApplyTheme()
        {
            this.BackColor = Color.FromArgb(240, 248, 255);

            foreach (Control ctrl in this.Controls)
            {
                ctrl.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);

                if (ctrl is Label)
                    ctrl.ForeColor = Color.Black;
                else if (ctrl is TextBox || ctrl is ComboBox)
                {
                    ctrl.BackColor = Color.White;
                    ctrl.ForeColor = Color.Black;
                }
                else if (ctrl is Button btn)
                {
                    btn.BackColor = Color.FromArgb(100, 149, 237);
                    btn.ForeColor = Color.Black;
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderSize = 1;
                    btn.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
                }
            }
        }

        // Adds hover effect to buttons
        private void ApplyHoverEffect(Button btn)
        {
            btn.MouseEnter += (s, e) => btn.BackColor = Color.FromArgb(70, 130, 180);
            btn.MouseLeave += (s, e) => btn.BackColor = Color.FromArgb(100, 149, 237);
        }
    }
}
