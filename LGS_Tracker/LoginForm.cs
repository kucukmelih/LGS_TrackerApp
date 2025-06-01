using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace LGS_Tracker
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();

            // Event subscriptions
            this.Load += LoginForm_Load;
            btnLogin.Click += btnLogin_Click;
            btnRegister.Click += btnRegister_Click;

            // Hover effects for buttons
            btnLogin.MouseEnter += (s, e) => btnLogin.BackColor = Color.FromArgb(70, 130, 180);
            btnLogin.MouseLeave += (s, e) => btnLogin.BackColor = Color.FromArgb(100, 149, 237);

            btnRegister.MouseEnter += (s, e) => btnRegister.BackColor = Color.FromArgb(70, 130, 180);
            btnRegister.MouseLeave += (s, e) => btnRegister.BackColor = Color.FromArgb(100, 149, 237);
        }

        // When form loads, apply UI theme
        private void LoginForm_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.FromArgb(240, 248, 255);

            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is Label)
                    ctrl.ForeColor = Color.Black;
                else if (ctrl is TextBox)
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

            lblMessage.Text = "";
        }

        // Handle login button click
        private void btnLogin_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "";

            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            // Validate inputs
            if (username == "" || password == "")
            {
                lblMessage.Text = "Please enter both username and password.";
                return;
            }

            // SQL query to authenticate user
            string query = "SELECT user_id, role FROM users WHERE username = @username AND password = @password";

            try
            {
                DataTable result = DB.ExecuteQuery(query,
                    new MySqlParameter("@username", username),
                    new MySqlParameter("@password", password)
                );

                // If match found, determine role and open appropriate form
                if (result.Rows.Count == 1)
                {
                    string role = result.Rows[0]["role"].ToString();
                    int userId = Convert.ToInt32(result.Rows[0]["user_id"]);

                    Form nextForm;
                    if (role == "admin")
                        nextForm = new AdminForm();
                    else
                        nextForm = new StudentForm(userId);

                    this.Hide();
                    nextForm.FormClosed += (s, args) => this.Show();
                    nextForm.Show();
                }
                else
                {
                    lblMessage.Text = "Incorrect username or password.";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        // Handle register button click
        private void btnRegister_Click(object sender, EventArgs e)
        {
            this.Hide();
            RegisterForm registerForm = new RegisterForm(this);
            registerForm.Show();
        }
    }
}
