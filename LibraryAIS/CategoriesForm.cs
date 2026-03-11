using System;
using System.Data;
using System.Windows.Forms;

namespace LibraryAIS
{
    public partial class CategoriesForm : Form
    {
        public CategoriesForm()
        {
            InitializeComponent();
        }

        private void CategoriesForm_Load(object sender, EventArgs e)
        {
            LoadCategories();
        }

        private void LoadCategories()
        {
            string query = "SELECT CategoryID, Name as 'Название' FROM categories ORDER BY Name";
            DataTable dt = DatabaseHelper.ExecuteQuery(query);
            dgvCategories.DataSource = dt;

            if (dgvCategories.Columns.Contains("CategoryID"))
                dgvCategories.Columns["CategoryID"].Visible = false;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Открываем универсальную форму с типом Category
            SimpleDictionaryEditForm form = new SimpleDictionaryEditForm(DictionaryType.Category);

            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadCategories();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvCategories.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите категорию для редактирования!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = Convert.ToInt32(dgvCategories.SelectedRows[0].Cells["CategoryID"].Value);

            // Открываем форму с типом Category и ID
            SimpleDictionaryEditForm form = new SimpleDictionaryEditForm(DictionaryType.Category, id);

            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadCategories();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvCategories.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите категорию для удаления!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (InputHelper.ConfirmDelete())
            {
                int id = Convert.ToInt32(dgvCategories.SelectedRows[0].Cells["CategoryID"].Value);

                // Проверка: есть ли книги в этой категории?
                int count = Convert.ToInt32(DatabaseHelper.ExecuteScalar(
                    $"SELECT COUNT(*) FROM bookcategories WHERE CategoryID = {id}", null));

                if (count > 0)
                {
                    MessageBox.Show($"Нельзя удалить категорию, так как к ней привязаны книги ({count} шт.)!",
                        "Ошибка удаления", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                DatabaseHelper.ExecuteNonQuery($"DELETE FROM categories WHERE CategoryID = {id}");
                LoadCategories();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadCategories();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}