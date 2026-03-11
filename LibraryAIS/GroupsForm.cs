using System;
using System.Data;
using System.Windows.Forms;

namespace LibraryAIS
{
    public partial class GroupsForm : Form
    {
        public GroupsForm()
        {
            InitializeComponent();
        }

        private void GroupsForm_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            string query = "SELECT GroupID, Name as 'Название' FROM `groups` ORDER BY Name";
            DataTable dt = DatabaseHelper.ExecuteQuery(query);
            dgvGroups.DataSource = dt;

            if (dgvGroups.Columns.Contains("GroupID"))
                dgvGroups.Columns["GroupID"].Visible = false;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Используем универсальную форму с типом Group
            SimpleDictionaryEditForm form = new SimpleDictionaryEditForm(DictionaryType.Group);

            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadData();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvGroups.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите группу для редактирования!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = Convert.ToInt32(dgvGroups.SelectedRows[0].Cells["GroupID"].Value);
            SimpleDictionaryEditForm form = new SimpleDictionaryEditForm(DictionaryType.Group, id);

            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadData();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvGroups.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите группу для удаления!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (InputHelper.ConfirmDelete())
            {
                int id = Convert.ToInt32(dgvGroups.SelectedRows[0].Cells["GroupID"].Value);

                // Проверка: есть ли студенты в группе?
                int count = Convert.ToInt32(DatabaseHelper.ExecuteScalar(
                    $"SELECT COUNT(*) FROM students WHERE `Group` = {id}", null));

                if (count > 0)
                {
                    MessageBox.Show($"Нельзя удалить группу, так как в ней числятся студенты ({count} чел.)!",
                        "Ошибка удаления", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                DatabaseHelper.ExecuteNonQuery($"DELETE FROM `groups` WHERE GroupID = {id}");
                LoadData();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}