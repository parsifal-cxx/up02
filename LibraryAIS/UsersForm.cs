using System;
using System.Data;
using System.Windows.Forms;

namespace LibraryAIS
{
    public partial class UsersForm : Form
    {
        public UsersForm()
        {
            InitializeComponent();
        }

        private void UsersForm_Load(object sender, EventArgs e)
        {
            LoadUsers();
        }

        private void LoadUsers()
        {
            string query = @"SELECT u.UserID, u.Login as 'Логин', 
                            u.Surname as 'Фамилия', u.Name as 'Имя', u.Patronymic as 'Отчество',
                            r.Name as 'Роль'
                            FROM users u
                            LEFT JOIN roles r ON u.Role = r.RoleID
                            ORDER BY u.Surname";

            DataTable dt = DatabaseHelper.ExecuteQuery(query);
            dgvUsers.DataSource = dt;

            if (dgvUsers.Columns.Contains("UserID"))
                dgvUsers.Columns["UserID"].Visible = false;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            UserEditForm editForm = new UserEditForm();
            if (editForm.ShowDialog() == DialogResult.OK)
            {
                LoadUsers();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите пользователя для редактирования!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int userId = Convert.ToInt32(dgvUsers.SelectedRows[0].Cells["UserID"].Value);
            UserEditForm editForm = new UserEditForm(userId);
            if (editForm.ShowDialog() == DialogResult.OK)
            {
                LoadUsers();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите пользователя для удаления!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int userId = Convert.ToInt32(dgvUsers.SelectedRows[0].Cells["UserID"].Value);

            // Нельзя удалить текущего пользователя
            if (userId == CurrentUser.UserID)
            {
                MessageBox.Show("Нельзя удалить текущего пользователя!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (InputHelper.ConfirmDelete())
            {
                string query = "DELETE FROM users WHERE UserID = " + userId;
                int result = DatabaseHelper.ExecuteNonQuery(query);

                if (result > 0)
                {
                    MessageBox.Show("Пользователь успешно удален!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadUsers();
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadUsers();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}