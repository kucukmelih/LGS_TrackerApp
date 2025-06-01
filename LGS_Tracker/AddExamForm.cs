using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace LGS_Tracker
{
    public partial class AddExamForm : Form
    {
        private int? studentUserId;
        private Form _containerForm;

        // Constructor: Accepts optional userId and container form to return back to
        public AddExamForm(int? userId = null, Form containerForm = null)
        {
            InitializeComponent();

            studentUserId = userId;
            _containerForm = containerForm;

            this.Load += AddExamForm_Load;
            btnSave.Click += btnSave_Click;
            btnCancel.Click += btnCancel_Click;

            ApplyHoverEffect(btnSave);
            ApplyHoverEffect(btnCancel);
        }

        // Form Load event: apply theme and load student list if no specific user ID is provided
        private void AddExamForm_Load(object sender, EventArgs e)
        {
            ApplyTheme();

            if (studentUserId != null)
            {
                cmbStudent.Visible = false;
                lblStudent.Visible = false;
            }
            else
            {
                try
                {
                    string query = "SELECT u.user_id, u.full_name FROM users u INNER JOIN students s ON u.user_id = s.user_id ORDER BY u.full_name";
                    DataTable students = DB.ExecuteQuery(query);
                    cmbStudent.DataSource = students;
                    cmbStudent.DisplayMember = "full_name";
                    cmbStudent.ValueMember = "user_id";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error while loading student list:\n" + ex.Message);
                }
            }
        }

        // Applies light theme colors and styles to all controls
        private void ApplyTheme()
        {
            this.BackColor = Color.FromArgb(240, 248, 255);

            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is Label)
                    ctrl.ForeColor = Color.Black;
                else if (ctrl is TextBox || ctrl is ComboBox || ctrl is NumericUpDown || ctrl is DateTimePicker)
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

        // Adds hover effects to buttons
        private void ApplyHoverEffect(Button btn)
        {
            btn.MouseEnter += (s, e) => btn.BackColor = Color.FromArgb(70, 130, 180);
            btn.MouseLeave += (s, e) => btn.BackColor = Color.FromArgb(100, 149, 237);
        }

        // Save button click handler: validates and saves exam and results
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            int userId = studentUserId ?? Convert.ToInt32(cmbStudent.SelectedValue);
            DateTime examDate = dateTimePicker.Value.Date;
            string description = txtDescription.Text.Trim();

            try
            {
                object sidObj = DB.ExecuteScalar("SELECT student_id FROM students WHERE user_id = @uid", new MySqlParameter("@uid", userId));
                if (sidObj == null)
                {
                    MessageBox.Show("Student not found.");
                    return;
                }

                int studentId = Convert.ToInt32(sidObj);

                // Insert exam info first (without score)
                DB.ExecuteNonQuery("INSERT INTO exams (student_id, exam_date, exam_description) VALUES (@sid, @date, @desc)",
                    new MySqlParameter("@sid", studentId),
                    new MySqlParameter("@date", examDate),
                    new MySqlParameter("@desc", description));

                int examId = Convert.ToInt32(DB.ExecuteScalar("SELECT LAST_INSERT_ID()"));

                var results = new List<(string subject, int correct, int incorrect)>();

                // Local method to add each subject's result
                void Add(string subject, NumericUpDown d, NumericUpDown y, NumericUpDown b)
                {
                    int correct = (int)d.Value;
                    int incorrect = (int)y.Value;
                    int blank = (int)b.Value;
                    AddResult(examId, subject, correct, incorrect, blank);
                    results.Add((subject, correct, incorrect));
                }

                // Add results for each subject
                Add("Matematik", nudMatD, nudMatY, nudMatB);
                Add("Fen Bilimleri", nudFenD, nudFenY, nudFenB);
                Add("Türkçe", nudTurkceD, nudTurkceY, nudTurkceB);
                Add("İnkılap Tarihi", nudTarihD, nudTarihY, nudTarihB);
                Add("Din Kültürü", nudDinD, nudDinY, nudDinB);
                Add("İngilizce", nudIngD, nudIngY, nudIngB);

                // Calculate total score
                double score = CalculateLGSScore(results);

                // Update the exam row with score
                DB.ExecuteNonQuery("UPDATE exams SET score = @score WHERE exam_id = @eid",
                    new MySqlParameter("@score", score),
                    new MySqlParameter("@eid", examId));

                MessageBox.Show("Exam has been saved successfully. Score: " + score);
                ReturnToContainer();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while saving:\n" + ex.Message);
            }
        }

        // Cancel button click: return to previous form or close
        private void btnCancel_Click(object sender, EventArgs e)
        {
            ReturnToContainer();
        }

        // Navigate back to container form or close if standalone
        private void ReturnToContainer()
        {
            if (_containerForm != null)
            {
                this.Parent.Controls.Clear();
                _containerForm.TopLevel = false;
                _containerForm.FormBorderStyle = FormBorderStyle.None;
                _containerForm.Dock = DockStyle.Fill;
                this.Parent.Controls.Add(_containerForm);
                _containerForm.Show();
            }
            else
            {
                this.Close();
            }
        }

        // Inserts a result row into database
        private void AddResult(int examId, string subject, int d, int y, int b)
        {
            DB.ExecuteNonQuery(@"
                INSERT INTO results (exam_id, subject, correct, incorrect, blank)
                VALUES (@eid, @subject, @c, @i, @b)",
                new MySqlParameter("@eid", examId),
                new MySqlParameter("@subject", subject),
                new MySqlParameter("@c", d),
                new MySqlParameter("@i", y),
                new MySqlParameter("@b", b)
            );
        }

        // Validates each subject's input totals
        private bool ValidateInputs()
        {
            return ValidateSection("Matematik", nudMatD, nudMatY, nudMatB, 20)
                && ValidateSection("Fen Bilimleri", nudFenD, nudFenY, nudFenB, 20)
                && ValidateSection("Türkçe", nudTurkceD, nudTurkceY, nudTurkceB, 20)
                && ValidateSection("İnkılap Tarihi", nudTarihD, nudTarihY, nudTarihB, 10)
                && ValidateSection("Din Kültürü", nudDinD, nudDinY, nudDinB, 10)
                && ValidateSection("İngilizce", nudIngD, nudIngY, nudIngB, 10);
        }

        // Checks if correct + incorrect + blank = total for a section
        private bool ValidateSection(string subject, NumericUpDown d, NumericUpDown y, NumericUpDown b, int total)
        {
            int sum = (int)(d.Value + y.Value + b.Value);
            if (sum != total)
            {
                MessageBox.Show($"{subject}: Total must be {total}. Current sum is {sum}.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        // Calculates the weighted LGS score based on net values
        private double CalculateLGSScore(List<(string subject, int correct, int incorrect)> answers)
        {
            var weights = new Dictionary<string, double>
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

            foreach (var (subject, correct, incorrect) in answers)
            {
                if (!weights.ContainsKey(subject)) continue;

                double net = correct - (incorrect / 3.0);
                double weighted = net * weights[subject];

                totalWeightedNet += weighted;
                maxWeightedNet += weights[subject] * (subject == "Matematik" || subject == "Fen Bilimleri" || subject == "Türkçe" ? 20 : 10);
            }

            return Math.Round((totalWeightedNet / maxWeightedNet) * 500, 2);
        }
    }
}
