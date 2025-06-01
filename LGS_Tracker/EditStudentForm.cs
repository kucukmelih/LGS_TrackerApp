using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace LGS_Tracker
{
    public partial class EditStudentForm : Form
    {
        private int userId;

        // Constructor: receives the user ID to load and update
        public EditStudentForm(int userId)
        {
            InitializeComponent();
            this.userId = userId;

            // Event bindings
            this.Load += EditStudentForm_Load;
            btnSave.Click += btnSave_Click;

            // Button hover effect
            btnSave.MouseEnter += (s, e) => btnSave.BackColor = Color.FromArgb(70, 130, 180);
            btnSave.MouseLeave += (s, e) => btnSave.BackColor = Color.FromArgb(100, 149, 237);
        }

        // Load event: fetch user and student info and fill textboxes
        private void EditStudentForm_Load(object sender, EventArgs e)
        {
            ApplyTheme();

            // Load user info (username, email)
            string userQuery = "SELECT username, email FROM users WHERE user_id = @uid";
            DataTable userDt = DB.ExecuteQuery(userQuery, new MySqlParameter("@uid", userId));
            if (userDt.Rows.Count > 0)
            {
                txtUsername.Text = userDt.Rows[0]["username"].ToString();
                txtEmail.Text = userDt.Rows[0]["email"].ToString();
            }

            // Load student info (school name, class level)
            string studentQuery = "SELECT school_name, class_level FROM students WHERE user_id = @uid";
            DataTable studentDt = DB.ExecuteQuery(studentQuery, new MySqlParameter("@uid", userId));
            if (studentDt.Rows.Count > 0)
            {
                txtSchoolName.Text = studentDt.Rows[0]["school_name"].ToString();
                txtClassLevel.Text = studentDt.Rows[0]["class_level"].ToString();
            }
        }

        // Apply visual styling to the form controls
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
                    btn.BackColor = Color.FromArgb(100, 149, 237);
                    btn.ForeColor = Color.Black;
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderSize = 1;
                    btn.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
                }
            }
        }

        // Save button click event: validate and update user & student data
        private void btnSave_Click(object sender, EventArgs e)
        {
            // Get trimmed input values
            string newUsername = txtUsername.Text.Trim();
            string newPassword = txtPassword.Text.Trim();
            string newEmail = txtEmail.Text.Trim();
            string newSchool = txtSchoolName.Text.Trim();
            string newClass = txtClassLevel.Text.Trim();

            // Basic validation
            if (string.IsNullOrEmpty(newUsername) || string.IsNullOrEmpty(newEmail))
            {
                MessageBox.Show("Username and email cannot be empty.");
                return;
            }

            // Build user update query
            string userUpdate = "UPDATE users SET username = @username, email = @email";
            var userParams = new List<MySqlParameter>
            {
                new MySqlParameter("@username", newUsername),
                new MySqlParameter("@email", newEmail),
                new MySqlParameter("@uid", userId)
            };

            // If password is entered, include in update
            if (!string.IsNullOrEmpty(newPassword))
            {
                userUpdate += ", password = @password";
                userParams.Add(new MySqlParameter("@password", newPassword));
            }

            userUpdate += " WHERE user_id = @uid";
            DB.ExecuteNonQuery(userUpdate, userParams.ToArray());

            // Check if the user is a student
            string checkStudent = "SELECT COUNT(*) FROM students WHERE user_id = @uid";
            int isStudent = Convert.ToInt32(DB.ExecuteScalar(checkStudent, new MySqlParameter("@uid", userId)));

            // If student record exists, update school and class info
            if (isStudent > 0)
            {
                string studentUpdate = "UPDATE students SET school_name = @school, class_level = @class WHERE user_id = @uid";
                DB.ExecuteNonQuery(studentUpdate,
                    new MySqlParameter("@school", newSchool),
                    new MySqlParameter("@class", newClass),
                    new MySqlParameter("@uid", userId)
                );
            }

            MessageBox.Show("Student information has been updated.");
            this.Close();
        }
    }
}
