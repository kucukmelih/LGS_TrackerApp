using System;
using System.Data;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace LGS_Tracker
{
    public partial class RegisterForm : Form
    {
        private Form _containerForm;

        public RegisterForm(Form containerForm)
        {
            InitializeComponent();
            _containerForm = containerForm;

            // Event bindings
            this.Load += RegisterForm_Load;
            btnRegister.Click += btnRegister_Click;
            btnBackToLogin.Click += btnBackToLogin_Click;
            cmbRole.SelectedIndexChanged += cmbRole_SelectedIndexChanged;

            // Hover effects
            btnRegister.MouseEnter += (s, e) => btnRegister.BackColor = Color.FromArgb(70, 130, 180);
            btnRegister.MouseLeave += (s, e) => btnRegister.BackColor = Color.FromArgb(100, 149, 237);
            btnBackToLogin.MouseEnter += (s, e) => btnBackToLogin.BackColor = Color.FromArgb(70, 130, 180);
            btnBackToLogin.MouseLeave += (s, e) => btnBackToLogin.BackColor = Color.FromArgb(100, 149, 237);
        }

        // Apply theme and default role on form load
        private void RegisterForm_Load(object sender, EventArgs e)
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
                    btn.BackColor = Color.FromArgb(100, 149, 237);
                    btn.ForeColor = Color.Black;
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
                    btn.FlatAppearance.BorderSize = 1;
                }
            }

            // Set default role to "Student"
            cmbRole.SelectedIndexChanged -= cmbRole_SelectedIndexChanged;
            cmbRole.SelectedItem = "Student";
            cmbRole.SelectedIndexChanged += cmbRole_SelectedIndexChanged;
            SetStudentFieldsVisible(true);
            txtEmail.Visible = true;
            lblEmail.Visible = true;
        }

        // Go back to login or student list form
        private void btnBackToLogin_Click(object sender, EventArgs e)
        {
            this.Hide();
            if (_containerForm is LoginForm lf) lf.Show();
            else if (_containerForm is StudentListForm slf) slf.Show();
            this.Close();
        }

        // Show/hide student-specific fields based on role
        private void cmbRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool isStudent = cmbRole.SelectedItem?.ToString().ToLower() == "student";
            SetStudentFieldsVisible(isStudent);
        }

        // Toggle visibility of school/gender fields
        private void SetStudentFieldsVisible(bool visible)
        {
            txtSchoolName.Visible = visible;
            txtClassLevel.Visible = visible;

            lblSchoolName.Visible = visible;
            lblClassLevel.Visible = visible;

            rbMale.Visible = visible;
            rbFemale.Visible = visible;
            rbOther.Visible = visible;
            lblGender.Visible = visible;
        }

        // Main registration logic
        private void btnRegister_Click(object sender, EventArgs e)
        {
            // Read all form inputs
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();
            string fullName = txtFullName.Text.Trim();
            string role = cmbRole.SelectedItem?.ToString().ToLower();
            string email = txtEmail.Text.Trim();

            // Validate required fields
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(confirmPassword) || string.IsNullOrWhiteSpace(fullName) ||
                string.IsNullOrEmpty(role) || string.IsNullOrWhiteSpace(email))
            {
                lblMessage.Text = "Please fill in all fields including email.";
                return;
            }

            // Email validation
            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                lblMessage.Text = "Please enter a valid email address.";
                return;
            }

            // Password format validation
            if (!Regex.IsMatch(password, @"^(?=.*[A-Za-z])(?=.*\d).{6,}$"))
            {
                lblMessage.Text = "Password must be at least 6 characters and include both letters and numbers.";
                return;
            }

            // Password confirmation check
            if (password != confirmPassword)
            {
                lblMessage.Text = "Passwords do not match.";
                return;
            }

            // Ensure only one admin account
            if (role == "admin")
            {
                string checkAdminQuery = "SELECT COUNT(*) FROM users WHERE role = 'admin'";
                int adminCount = Convert.ToInt32(DB.ExecuteScalar(checkAdminQuery));
                if (adminCount >= 1)
                {
                    lblMessage.Text = "Only one admin is allowed.";
                    return;
                }
            }

            try
            {
                // Check if username already exists
                string checkQuery = "SELECT COUNT(*) FROM users WHERE username = @username";
                object existing = DB.ExecuteScalar(checkQuery, new MySqlParameter("@username", username));

                if (Convert.ToInt32(existing) > 0)
                {
                    lblMessage.Text = "This username is already taken.";
                    return;
                }

                // Insert into users table
                string insertUser = "INSERT INTO users (username, password, full_name, role) VALUES (@username, @password, @full_name, @role)";
                int affected = DB.ExecuteNonQuery(insertUser,
                    new MySqlParameter("@username", username),
                    new MySqlParameter("@password", password),
                    new MySqlParameter("@full_name", fullName),
                    new MySqlParameter("@role", role)
                );

                if (affected == 1)
                {
                    int newUserId = Convert.ToInt32(DB.ExecuteScalar("SELECT LAST_INSERT_ID()"));

                    if (role == "student")
                    {
                        // Prepare student-specific data
                        string schoolName = txtSchoolName.Text.Trim();
                        string classLevel = txtClassLevel.Text.Trim();

                        string gender = null;
                        if (rbMale.Checked)
                            gender = "Male";
                        else if (rbFemale.Checked)
                            gender = "Female";
                        else if (rbOther.Checked)
                            gender = "Other";

                        if (string.IsNullOrWhiteSpace(gender))
                        {
                            lblMessage.Text = "Please select gender.";
                            return;
                        }

                        // Insert into students table
                        string insertStudent = @"
                            INSERT INTO students (user_id, email, gender, school_name, class_level)
                            VALUES (@user_id, @email, @gender, @school_name, @class_level)";

                        DB.ExecuteNonQuery(insertStudent,
                            new MySqlParameter("@user_id", newUserId),
                            new MySqlParameter("@email", email),
                            new MySqlParameter("@gender", gender),
                            new MySqlParameter("@school_name", schoolName),
                            new MySqlParameter("@class_level", classLevel)
                        );
                    }
                    else if (role == "admin")
                    {
                        // Admin email goes into users table
                        string insertAdminEmail = "UPDATE users SET email = @adminEmail WHERE user_id = @uid";
                        DB.ExecuteNonQuery(insertAdminEmail,
                            new MySqlParameter("@adminEmail", email),
                            new MySqlParameter("@uid", newUserId));
                    }

                    MessageBox.Show("Registration successful.");

                    this.Hide();

                    // Navigate back to original form
                    if (_containerForm is StudentListForm slf)
                    {
                        slf.LoadStudents();
                        slf.Show();
                    }
                    else if (_containerForm is LoginForm lf)
                    {
                        lf.Show();
                    }

                    this.Close();
                }
                else
                {
                    lblMessage.Text = "An error occurred while registering.";
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error: " + ex.Message;
            }
        }
    }
}
