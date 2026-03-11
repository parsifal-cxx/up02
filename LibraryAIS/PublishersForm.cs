using System;
using System.Data;
using System.Windows.Forms;

namespace LibraryAIS
{
    public partial class PublishersForm : Form
    {
        public PublishersForm()
        {
            InitializeComponent();
        }

        private void PublishersForm_Load(object sender, EventArgs e)
        {
            LoadPublishers();
        }

        private void LoadPublishers()
        {
            string query = "SELECT PublisherID, Name as 'Название' FROM publishers ORDER BY Name";
            DataTable dt = DatabaseHelper.ExecuteQuery(query);
            dgvPublishers.DataSource = dt;

            if (dgvPublishers.Columns.Contains("PublisherID"))
                dgvPublishers.Columns["PublisherID"].Visible = false;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            SimpleDictionaryEditForm form = new SimpleDictionaryEditForm(DictionaryType.Publisher);
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadPublishers();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvPublishers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите издательство для редактирования!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = Convert.ToInt32(dgvPublishers.SelectedRows[0].Cells["PublisherID"].Value);
            SimpleDictionaryEditForm form = new SimpleDictionaryEditForm(DictionaryType.Publisher, id);

            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadPublishers();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvPublishers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите издательство для удаления!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (InputHelper.ConfirmDelete())
            {
                int id = Convert.ToInt32(dgvPublishers.SelectedRows[0].Cells["PublisherID"].Value);

                int count = Convert.ToInt32(DatabaseHelper.ExecuteScalar(
                    $"SELECT COUNT(*) FROM books WHERE Publisher = {id}", null));

                if (count > 0)
                {
                    MessageBox.Show($"Нельзя удалить издательство, так как к нему привязаны книги ({count} шт.)!",
                        "Ошибка удаления", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                DatabaseHelper.ExecuteNonQuery($"DELETE FROM publishers WHERE PublisherID = {id}");
                LoadPublishers();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadPublishers();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}