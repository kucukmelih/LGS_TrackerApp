using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LGS_Tracker;

namespace LGS_Tracker
{
    public partial class AdminForm : Form
    {
        public AdminForm()
        {
            InitializeComponent();

            // Event subscriptions
            this.Load += AdminForm_Load;
            btnAddExam.Click += btnAddExam_Click;
            btnStudentList.Click += btnStudentList_Click;
            btnViewAllResults.Click += btnViewAllResults_Click;
            btnReport.Click += btnReport_Click;
            btnOCRPDF.Click += btnOCRPDF_Click;
            btnLogout.Click += btnLogout_Click;

            // Apply hover effects to all buttons
            ApplyHoverEffect(btnAddExam);
            ApplyHoverEffect(btnStudentList);
            ApplyHoverEffect(btnViewAllResults);
            ApplyHoverEffect(btnReport);
            ApplyHoverEffect(btnOCRPDF);
            ApplyHoverEffect(btnLogout);
        }

        // When form loads, apply theme and display welcome message
        private void AdminForm_Load(object sender, EventArgs e)
        {
            lblMessage.Text = "Welcome, Admin!";
            ApplyTheme();
        }

        // Apply colors and styles to form controls
        private void ApplyTheme()
        {
            this.BackColor = Color.FromArgb(240, 248, 255);

            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is Label)
                {
                    ctrl.ForeColor = Color.Black;
                }
                else if (ctrl is TextBox || ctrl is ComboBox)
                {
                    ctrl.BackColor = Color.White;
                    ctrl.ForeColor = Color.Black;
                }
                else if (ctrl is Button btn)
                {
                    // Special theme for logout button
                    if (btn == btnLogout)
                    {
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

        // Add hover behavior for each button
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

        // Remove any open child forms before opening a new one
        private void ClearEmbeddedForms()
        {
            foreach (var ctrl in this.Controls.OfType<Form>().ToList())
            {
                ctrl.Close();
                this.Controls.Remove(ctrl);
            }
        }

        // Show a given form embedded within the main AdminForm panel
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

        // Open AddExamForm
        private void btnAddExam_Click(object sender, EventArgs e)
        {
            ShowEmbeddedForm(new AddExamForm());
            lblMessage.Text = "Add Exam Page Loaded";
        }

        // Open StudentListForm
        private void btnStudentList_Click(object sender, EventArgs e)
        {
            ShowEmbeddedForm(new StudentListForm());
            lblMessage.Text = "Student List Page Loaded";
        }

        // Open AdminExamViewer form
        private void btnViewAllResults_Click(object sender, EventArgs e)
        {
            ShowEmbeddedForm(new AdminExamViewer());
            lblMessage.Text = "All Results Page Loaded";
        }

        // Open ReportViewer form
        private void btnReport_Click(object sender, EventArgs e)
        {
            ShowEmbeddedForm(new ReportViewer());
            lblMessage.Text = "Report Page Loaded";
        }

        // Open OCR/PDF input form
        private void btnOCRPDF_Click(object sender, EventArgs e)
        {
            ShowEmbeddedForm(new OCRandPDFInputForm());
            lblMessage.Text = "OCR/PDF Page Loaded";
        }

        // Logout and return to LoginForm
        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
        }
    }
}
