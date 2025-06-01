using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace LGS_Tracker
{
    public partial class StudentListForm : Form
    {
        public StudentListForm()
        {
            InitializeComponent();

            // Event bindings for button clicks
            this.Load += StudentListForm_Load;
            btnDelete.Click += btnDelete_Click;
            btnEdit.Click += btnEdit_Click;
            btnAdd.Click += btnAddStudent_Click;
            btnBack.Click += btnBack_Click;
            btnSearch.Click += btnSearch_Click;

            // Apply hover effects to buttons
            ApplyHoverEffect(btnAdd);
            ApplyHoverEffect(btnEdit);
            ApplyHoverEffect(btnDelete);
            ApplyHoverEffect(btnBack);
            ApplyHoverEffect(btnSearch);
        }

        // On form load, apply theme and load student list
        private void StudentListForm_Load(object sender, EventArgs e)
        {
            ApplyTheme();
            LoadStudents();
            txtSearch.ReadOnly = false;
            txtSearch.Enabled = true;
        }

        // Load all students or apply name filter
        public void LoadStudents(string nameFilter = "")
        {
            try
            {
                string query = @"
                    SELECT u.user_id AS 'ID', u.full_name AS 'Full Name', u.username AS 'Username'
                    FROM users u
                    INNER JOIN students s ON u.user_id = s.user_id";

                if (!string.IsNullOrEmpty(nameFilter))
                {
                    query += " WHERE u.full_name LIKE @name";
                }

                query += " ORDER BY u.user_id";

                DataTable table;
                if (!string.IsNullOrEmpty(nameFilter))
                {
                    table = DB.ExecuteQuery(query, new MySqlParameter("@name", $"%{nameFilter}%"));
                }
                else
                {
                    table = DB.ExecuteQuery(query);
                }

                // Bind the result to DataGridView
                dgvStudents.DataSource = table;

                // Style settings for grid
                dgvStudents.BackgroundColor = Color.White;
                dgvStudents.DefaultCellStyle.ForeColor = Color.Black;
                dgvStudents.DefaultCellStyle.BackColor = Color.White;
                dgvStudents.DefaultCellStyle.SelectionBackColor = Color.FromArgb(100, 149, 237);
                dgvStudents.DefaultCellStyle.SelectionForeColor = Color.White;
                dgvStudents.GridColor = Color.FromArgb(200, 200, 200);
                dgvStudents.EnableHeadersVisualStyles = false;
                dgvStudents.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(100, 149, 237);
                dgvStudents.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgvStudents.RowHeadersVisible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load student list:\n" + ex.Message);
            }
        }

        // Perform search based on textbox input
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchText = txtSearch.Text.Trim();
            LoadStudents(searchText);
        }

        // Open edit form for selected student
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvStudents.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a student to edit.");
                return;
            }

            int userId = Convert.ToInt32(dgvStudents.SelectedRows[0].Cells["ID"].Value);
            EditStudentForm editForm = new EditStudentForm(userId);
            editForm.ShowDialog();
            LoadStudents();
        }

        // Delete selected student from database
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvStudents.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a student to delete.");
                return;
            }

            int userId = Convert.ToInt32(dgvStudents.SelectedRows[0].Cells["ID"].Value);
            string fullName = dgvStudents.SelectedRows[0].Cells["Full Name"].Value.ToString();

            DialogResult result = MessageBox.Show($"Are you sure you want to delete {fullName}?", "Delete Confirmation", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                try
                {
                    string deleteQuery = "DELETE FROM users WHERE user_id = @id";
                    DB.ExecuteNonQuery(deleteQuery, new MySqlParameter("@id", userId));

                    MessageBox.Show("Student deleted successfully.");
                    LoadStudents();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while deleting the student:\n" + ex.Message);
                }
            }
        }

        // Open registration form to add a new student
        private void btnAddStudent_Click(object sender, EventArgs e)
        {
            RegisterForm registerForm = new RegisterForm(this);
            registerForm.ShowDialog();
            LoadStudents();
        }

        // Close the student list form and go back
        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Parent.Controls.Remove(this);
            this.Dispose();
        }

        // Apply consistent color theme
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
                    if (btn == btnSearch)
                    {
                        btn.BackColor = Color.FromArgb(173, 216, 230); // Light Blue for search
                    }
                    else
                    {
                        btn.BackColor = Color.FromArgb(100, 149, 237); // Cornflower Blue
                    }

                    btn.ForeColor = Color.Black;
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderSize = 1;
                    btn.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
                }
            }
        }

        // Add hover effects for buttons
        private void ApplyHoverEffect(Button btn)
        {
            btn.MouseEnter += (s, e) => btn.BackColor = Color.FromArgb(70, 130, 180);
            btn.MouseLeave += (s, e) => btn.BackColor = Color.FromArgb(100, 149, 237);
        }
    }
}
