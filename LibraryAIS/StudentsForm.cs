using System;
using System.Data;
using System.Windows.Forms;

namespace LibraryAIS
{
    public partial class StudentsForm : Form
    {
        private DataTable studentsTable;
        private DataView studentsView;

        public StudentsForm()
        {
            InitializeComponent();
        }

        private void StudentsForm_Load(object sender, EventArgs e)
        {
            LoadGroups();
            LoadStudents();
            ConfigureButtonsByRole();
        }

        private void ConfigureButtonsByRole()
        {
            // Библиотекарь может только просматривать
            if (CurrentUser.IsLibrarian)
            {
                btnAdd.Visible = false;
                btnEdit.Visible = false;
            }
        }

        private void LoadGroups()
        {
            string query = "SELECT GroupID, Name FROM `groups` ORDER BY Name";
            DataTable dt = DatabaseHelper.ExecuteQuery(query);

            DataRow emptyRow = dt.NewRow();
            emptyRow["GroupID"] = 0;
            emptyRow["Name"] = "Все группы";
            dt.Rows.InsertAt(emptyRow, 0);

            cmbFilterGroup.DataSource = dt;
            cmbFilterGroup.DisplayMember = "Name";
            cmbFilterGroup.ValueMember = "GroupID";
            cmbFilterGroup.SelectedIndex = 0;
        }

        private void LoadStudents()
        {
            string query = @"SELECT s.StudentID, s.Surname as 'Фамилия', s.Name as 'Имя', 
                            s.Patronymic as 'Отчество', g.Name as 'Группа', s.Phone as 'Телефон',
                            s.`Group` as GroupID
                            FROM students s
                            LEFT JOIN `groups` g ON s.`Group` = g.GroupID
                            ORDER BY s.Surname";

            studentsTable = DatabaseHelper.ExecuteQuery(query);
            studentsView = new DataView(studentsTable);
            dgvStudents.DataSource = studentsView;

            if (dgvStudents.Columns.Contains("StudentID"))
                dgvStudents.Columns["StudentID"].Visible = false;
            if (dgvStudents.Columns.Contains("GroupID"))
                dgvStudents.Columns["GroupID"].Visible = false;
        }

        private void ApplyFilters()
        {
            if (studentsView == null) return;

            string filter = "";

            // Поиск по ФИО
            if (!string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                string searchText = txtSearch.Text.Replace("'", "''");
                filter = "(Фамилия LIKE '%" + searchText + "%' OR Имя LIKE '%" + searchText + "%' OR Отчество LIKE '%" + searchText + "%')";
            }

            // Фильтр по группе
            if (cmbFilterGroup.SelectedValue != null && Convert.ToInt32(cmbFilterGroup.SelectedValue) > 0)
            {
                if (!string.IsNullOrEmpty(filter)) filter += " AND ";
                filter += "GroupID = " + cmbFilterGroup.SelectedValue;
            }

            studentsView.RowFilter = filter;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void cmbFilterGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void btnResetFilters_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            cmbFilterGroup.SelectedIndex = 0;
            LoadStudents();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            StudentEditForm editForm = new StudentEditForm();
            if (editForm.ShowDialog() == DialogResult.OK)
            {
                LoadStudents();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvStudents.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите студента для редактирования!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int studentId = Convert.ToInt32(dgvStudents.SelectedRows[0].Cells["StudentID"].Value);
            StudentEditForm editForm = new StudentEditForm(studentId);
            if (editForm.ShowDialog() == DialogResult.OK)
            {
                LoadStudents();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvStudents.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите студента для удаления!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (InputHelper.ConfirmDelete())
            {
                int studentId = Convert.ToInt32(dgvStudents.SelectedRows[0].Cells["StudentID"].Value);

                string query = "DELETE FROM students WHERE StudentID = " + studentId;
                int result = DatabaseHelper.ExecuteNonQuery(query);

                if (result > 0)
                {
                    MessageBox.Show("Студент успешно удален!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadStudents();
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadStudents();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}