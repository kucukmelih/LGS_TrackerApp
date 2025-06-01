using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;

namespace LGS_Tracker
{
    public partial class AdminExamViewer : Form
    {
        private int? selectedUserId = null;
        private DataTable allExamData;

        // Dictionary to translate Turkish subject names to English
        private readonly Dictionary<string, string> subjectTranslations = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "Matematik", "Math" },
            { "Fen Bilimleri", "Science" },
            { "Türkçe", "Turkish" },
            { "İnkılap Tarihi", "History" },
            { "Din Kültürü", "Religion" },
            { "İngilizce", "English" }
        };

        public AdminExamViewer()
        {
            InitializeComponent();
            this.Load += AdminExamViewer_Load;
            cmbStudent.SelectedIndexChanged += cmbStudent_SelectedIndexChanged;
            dgvExams.CellClick += dgvExams_CellClick;
            btnExport.Click += btnExport_Click;
            btnBack.Click += (s, e) => this.Parent.Controls.Remove(this);
            btnDelete.Click += btnDelete_Click;
            ApplyHoverEffect(btnDelete);
            ApplyHoverEffect(btnExport);
            ApplyHoverEffect(btnBack);
        }

        // Form load event: set theme and load student list
        private void AdminExamViewer_Load(object sender, EventArgs e)
        {
            ApplyTheme();
            LoadStudentList();
        }

        // Apply overall theme colors to the form and controls
        private void ApplyTheme()
        {
            this.BackColor = Color.FromArgb(240, 248, 255);

            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is Label)
                    ctrl.ForeColor = Color.Black;
                else if (ctrl is ComboBox || ctrl is TextBox)
                {
                    ctrl.BackColor = Color.White;
                    ctrl.ForeColor = Color.Black;
                }
                else if (ctrl is Button btn)
                {
                    btn.BackColor = Color.FromArgb(100, 149, 237); // Cornflower Blue
                    btn.ForeColor = Color.Black;
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderSize = 1;
                    btn.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
                }
            }

            ApplyGridStyle(dgvExams);
            ApplyGridStyle(dgvDetails);
        }

        // Apply hover effect to buttons
        private void ApplyHoverEffect(Button btn)
        {
            btn.MouseEnter += (s, e) => btn.BackColor = Color.FromArgb(70, 130, 180); // Steel Blue
            btn.MouseLeave += (s, e) => btn.BackColor = Color.FromArgb(100, 149, 237);
        }

        // Apply visual styling to DataGridViews
        private void ApplyGridStyle(DataGridView dgv)
        {
            dgv.BackgroundColor = Color.White;
            dgv.DefaultCellStyle.ForeColor = Color.Black;
            dgv.DefaultCellStyle.BackColor = Color.White;
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(100, 149, 237);
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;
            dgv.GridColor = Color.FromArgb(200, 200, 200);
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(100, 149, 237);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.RowHeadersVisible = false;
        }

        // Load all students into ComboBox
        private void LoadStudentList()
        {
            string query = @"
                SELECT u.user_id, u.full_name
                FROM users u
                INNER JOIN students s ON u.user_id = s.user_id
                ORDER BY u.full_name";

            DataTable students = DB.ExecuteQuery(query);

            cmbStudent.DataSource = students;
            cmbStudent.DisplayMember = "full_name";
            cmbStudent.ValueMember = "user_id";
            cmbStudent.SelectedIndex = -1;
        }

        // When a student is selected, load their exams
        private void cmbStudent_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbStudent.SelectedIndex >= 0)
            {
                var selectedRow = cmbStudent.SelectedItem as DataRowView;
                if (selectedRow != null)
                {
                    selectedUserId = Convert.ToInt32(selectedRow["user_id"]);
                    LoadExamData();
                }
            }
        }

        // Delete exam and its results
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvExams.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an exam to delete.");
                return;
            }

            int selectedExamId = Convert.ToInt32(dgvExams.SelectedRows[0].Cells["exam_id"].Value);

            DialogResult result = MessageBox.Show(
                "Are you sure you want to delete this exam? This will also delete related results.",
                "Delete Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    // Delete child results first
                    string deleteResultsQuery = "DELETE FROM results WHERE exam_id = @examId";
                    DB.ExecuteNonQuery(deleteResultsQuery, new MySqlParameter("@examId", selectedExamId));

                    // Then delete the exam itself
                    string deleteExamQuery = "DELETE FROM exams WHERE exam_id = @examId";
                    DB.ExecuteNonQuery(deleteExamQuery, new MySqlParameter("@examId", selectedExamId));

                    MessageBox.Show("Exam deleted successfully.");

                    // Refresh view
                    LoadExamData();
                    dgvDetails.DataSource = null;
                    chartPerformance.Series.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting exam:\n" + ex.Message);
                }
            }
        }

        // Load exam and result data for selected student
        private void LoadExamData()
        {
            string query = @"
        SELECT e.exam_id, e.exam_date, e.exam_description, e.score,
               r.subject, r.correct, r.incorrect, r.blank,
               ROUND((r.correct / (r.correct + r.incorrect + r.blank)) * 100, 2) AS success_rate
        FROM exams e
        INNER JOIN results r ON e.exam_id = r.exam_id
        INNER JOIN students s ON e.student_id = s.student_id
        INNER JOIN users u ON s.user_id = u.user_id
        WHERE u.user_id = @uid
        ORDER BY e.exam_date DESC, r.subject";

            allExamData = DB.ExecuteQuery(query, new MySqlParameter("@uid", selectedUserId));

            var examSummaryTable = allExamData.DefaultView.ToTable(true, "exam_id", "exam_date", "exam_description", "score");
            dgvExams.DataSource = examSummaryTable;

            dgvExams.Columns["exam_id"].Visible = false;
            dgvExams.Columns["exam_date"].HeaderText = "Date";
            dgvExams.Columns["exam_description"].HeaderText = "Description";
            dgvExams.Columns["score"].HeaderText = "Score";

            dgvDetails.DataSource = null;
            chartPerformance.Series.Clear();
        }

        // When an exam is clicked, show its result breakdown and chart
        private void dgvExams_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            int selectedExamId = Convert.ToInt32(dgvExams.Rows[e.RowIndex].Cells["exam_id"].Value);
            DataRow[] rows = allExamData.Select($"exam_id = {selectedExamId}");

            DataTable detailsTable = new DataTable();
            detailsTable.Columns.Add("Subject", typeof(string));
            detailsTable.Columns.Add("Success Rate", typeof(double));

            foreach (var row in rows)
            {
                string subjectTr = row["subject"].ToString();
                string subjectEn = TranslateSubject(subjectTr);

                var newRow = detailsTable.NewRow();
                newRow["Subject"] = subjectEn;
                newRow["Success Rate"] = row["success_rate"];
                detailsTable.Rows.Add(newRow);
            }

            dgvDetails.DataSource = detailsTable;

            // Color rows based on success rate
            foreach (DataGridViewRow row in dgvDetails.Rows)
            {
                if (row.Cells["Success Rate"].Value != DBNull.Value)
                {
                    double rate = Convert.ToDouble(row.Cells["Success Rate"].Value);
                    if (rate >= 80)
                        row.DefaultCellStyle.BackColor = Color.LightGreen;
                    else if (rate >= 50)
                        row.DefaultCellStyle.BackColor = Color.LightYellow;
                    else
                        row.DefaultCellStyle.BackColor = Color.LightCoral;
                }
            }

            DrawChart(detailsTable);
        }

        // Translate subject names from Turkish to English
        private string TranslateSubject(string original)
        {
            return subjectTranslations.TryGetValue(original.Trim(), out string translated) ? translated : original;
        }

        // Draw column chart based on success rate data
        private void DrawChart(DataTable table)
        {
            chartPerformance.Series.Clear();
            var series = new Series("Success Rate")
            {
                ChartType = SeriesChartType.Column,
                Color = Color.SteelBlue
            };

            foreach (DataRow row in table.Rows)
            {
                string subject = row["Subject"].ToString();
                double rate = Convert.ToDouble(row["Success Rate"]);
                series.Points.AddXY(subject, rate);
            }

            chartPerformance.Series.Add(series);
            chartPerformance.ChartAreas[0].RecalculateAxesScale();
        }

        // Export selected exam details to CSV
        private void btnExport_Click(object sender, EventArgs e)
        {
            if (dgvExams.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an exam first.");
                return;
            }

            int selectedExamId = Convert.ToInt32(dgvExams.SelectedRows[0].Cells["exam_id"].Value);
            DataRow[] rows = allExamData.Select($"exam_id = {selectedExamId}");

            if (rows.Length == 0)
            {
                MessageBox.Show("No data available for the selected exam.");
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv",
                FileName = $"exam_{selectedExamId}_details.csv"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                StringBuilder sb = new StringBuilder();

                sb.AppendLine("Subject,Correct,Incorrect,Blank,Success Rate");

                foreach (var row in rows)
                {
                    string subjectTr = row["subject"].ToString();
                    string subjectEn = TranslateSubject(subjectTr);

                    int correct = Convert.ToInt32(row["correct"]);
                    int incorrect = Convert.ToInt32(row["incorrect"]);
                    int blank = Convert.ToInt32(row["blank"]);
                    double successRate = Convert.ToDouble(row["success_rate"]);

                    sb.AppendLine($"{subjectEn},{correct},{incorrect},{blank},{successRate}%");
                }

                string scoreStr = rows[0]["score"] != DBNull.Value ? rows[0]["score"].ToString() : "N/A";
                sb.AppendLine();
                sb.AppendLine($"Total Score (/500):,{scoreStr}");

                File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
                MessageBox.Show("Exported to CSV successfully.");
            }
        }
    }
}
