using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using LGS_Tracker;

namespace LGS_Tracker
{
    public partial class StudentForm : Form
    {
        private int userId;

        // Constructor: receives the logged-in user's ID
        public StudentForm(int userId)
        {
            InitializeComponent();
            this.userId = userId;

            // Event handlers for UI actions
            this.Load += StudentForm_Load;
            btnAddExam.Click += btnAddExam_Click;
            btnViewResults.Click += btnViewResults_Click;
            btnExportPDF.Click += btnExportPDF_Click;
            btnLogout.Click += btnLogout_Click;

            // Add hover UI effects to buttons
            ApplyHoverEffect(btnAddExam);
            ApplyHoverEffect(btnViewResults);
            ApplyHoverEffect(btnExportPDF);
            ApplyHoverEffect(btnLogout);
        }

        // On form load, display welcome text and apply theme
        private void StudentForm_Load(object sender, EventArgs e)
        {
            lblWelcome.Text = $"Welcome, {GetUsernameById(userId)}!";
            ApplyTheme();
        }

        // Apply light UI theme across all controls
        private void ApplyTheme()
        {
            this.BackColor = Color.FromArgb(240, 248, 255);

            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is Label)
                    ctrl.ForeColor = Color.Black;
                else if (ctrl is TextBox || ctrl is ComboBox)
                {
                    ctrl.BackColor = Color.White;
                    ctrl.ForeColor = Color.Black;
                }
                else if (ctrl is Button btn)
                {
                    if (btn == btnLogout)
                    {
                        // Logout button has a red tone
                        btn.BackColor = Color.FromArgb(255, 99, 99);
                        btn.ForeColor = Color.White;
                        btn.FlatStyle = FlatStyle.Flat;
                        btn.FlatAppearance.BorderSize = 1;
                        btn.FlatAppearance.BorderColor = Color.DarkRed;
                    }
                    else
                    {
                        btn.BackColor = Color.FromArgb(100, 149, 237); // Cornflower Blue
                        btn.ForeColor = Color.Black;
                        btn.FlatStyle = FlatStyle.Flat;
                        btn.FlatAppearance.BorderSize = 1;
                        btn.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
                    }
                }
            }
        }

        // Button hover style logic
        private void ApplyHoverEffect(Button btn)
        {
            btn.MouseEnter += (s, e) =>
            {
                if (btn == btnLogout)
                    btn.BackColor = Color.FromArgb(220, 20, 60); // Crimson
                else
                    btn.BackColor = Color.FromArgb(70, 130, 180); // Steel Blue
            };

            btn.MouseLeave += (s, e) =>
            {
                if (btn == btnLogout)
                    btn.BackColor = Color.FromArgb(255, 99, 99);
                else
                    btn.BackColor = Color.FromArgb(100, 149, 237); // Cornflower Blue
            };
        }

        // Fetch username from database by ID
        private string GetUsernameById(int id)
        {
            try
            {
                string query = "SELECT username FROM users WHERE user_id = @userId";
                var result = DB.ExecuteScalar(query, new MySqlParameter("@userId", id));
                return result != null ? result.ToString() : "Unknown";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching username: " + ex.Message);
                return "Error";
            }
        }

        // Removes any form embedded inside this form
        private void ClearEmbeddedForms()
        {
            foreach (var ctrl in this.Controls.OfType<Form>().ToList())
            {
                ctrl.Close();
                this.Controls.Remove(ctrl);
            }
        }

        // Embeds a form into the current form panel
        private void ShowEmbeddedForm(Form form)
        {
            ClearEmbeddedForms();
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            this.Controls.Add(form);
            form.BringToFront();
            form.Show();
        }

        // Navigate to Add Exam form (with current user ID)
        private void btnAddExam_Click(object sender, EventArgs e)
        {
            ShowEmbeddedForm(new AddExamForm(userId));
        }

        // Navigate to View Results form
        private void btnViewResults_Click(object sender, EventArgs e)
        {
            ShowEmbeddedForm(new StudentExamViewerForm(userId));
        }

        // Navigate to Report PDF generation form
        private void btnExportPDF_Click(object sender, EventArgs e)
        {
            ShowEmbeddedForm(new ReportViewer(userId));
        }

        // Log out and return to login form
        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
        }
    }
}
