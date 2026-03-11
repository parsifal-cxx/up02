using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace LibraryAIS
{
    public partial class AuthorsForm : Form
    {
        public AuthorsForm()
        {
            InitializeComponent();
        }

        private void AuthorsForm_Load(object sender, EventArgs e)
        {
            LoadAuthors();
        }

        private void LoadAuthors()
        {
            string query = @"SELECT AuthorID, Surname as 'Фамилия', Name as 'Имя', 
                            Patronymic as 'Отчество' FROM authors ORDER BY Surname";
            DataTable dt = DatabaseHelper.ExecuteQuery(query);
            dgvAuthors.DataSource = dt;

            if (dgvAuthors.Columns.Contains("AuthorID"))
                dgvAuthors.Columns["AuthorID"].Visible = false;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Открываем новую красивую форму добавления
            AuthorEditForm editForm = new AuthorEditForm();
            if (editForm.ShowDialog() == DialogResult.OK)
            {
                LoadAuthors();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvAuthors.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите автора для редактирования!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int authorId = Convert.ToInt32(dgvAuthors.SelectedRows[0].Cells["AuthorID"].Value);

            // Открываем форму редактирования
            AuthorEditForm editForm = new AuthorEditForm(authorId);
            if (editForm.ShowDialog() == DialogResult.OK)
            {
                LoadAuthors();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvAuthors.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите автора для удаления!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (InputHelper.ConfirmDelete())
            {
                int authorId = Convert.ToInt32(dgvAuthors.SelectedRows[0].Cells["AuthorID"].Value);

                // Проверка: есть ли книги у этого автора?
                string checkQuery = $"SELECT COUNT(*) FROM books WHERE AuthorOne = {authorId} OR AuthorTwo = {authorId}";
                int count = Convert.ToInt32(DatabaseHelper.ExecuteScalar(checkQuery, null));

                if (count > 0)
                {
                    MessageBox.Show($"Нельзя удалить автора, так как в базе есть его книги ({count} шт.).\nСначала удалите книги.",
                        "Ошибка удаления", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string query = "DELETE FROM authors WHERE AuthorID = " + authorId;
                int result = DatabaseHelper.ExecuteNonQuery(query);

                if (result > 0)
                {
                    MessageBox.Show("Автор успешно удален!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadAuthors();
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadAuthors();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}