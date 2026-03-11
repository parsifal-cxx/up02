using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace LibraryAIS
{
    public partial class BorrowingsForm : Form
    {
        private DataTable borrowingsTable;
        private DataView borrowingsView;

        public BorrowingsForm()
        {
            InitializeComponent();
        }

        private void BorrowingsForm_Load(object sender, EventArgs e)
        {
            LoadSortOptions();
            LoadBorrowings();
        }

        private void LoadSortOptions()
        {
            cmbSort.Items.Clear();
            cmbSort.Items.Add("Без сортировки");
            cmbSort.Items.Add("По дате заимствования (сначала новые)");
            cmbSort.Items.Add("По дате заимствования (сначала старые)");
            cmbSort.Items.Add("По дате возврата (сначала новые)");
            cmbSort.Items.Add("По дате возврата (сначала старые)");
            cmbSort.SelectedIndex = 0;
        }

        private void LoadBorrowings()
        {
            // ИЗМЕНЕНИЕ: Используем STR_TO_DATE для правильной конвертации строк в даты
            string query = @"SELECT br.BorrowingID, 
                    CONCAT(s.Surname, ' ', s.Name, ' ', IFNULL(s.Patronymic, '')) as 'ФИО студента',
                    b.Title as 'Книга',
                    STR_TO_DATE(br.BorrowDate, '%d.%m.%Y') as 'Дата заимствования',
                    STR_TO_DATE(br.ReturnDate, '%d.%m.%Y') as 'Дата возврата',
                    br.Count as 'Количество',
                    g.Name as 'Группа'
                    FROM borrowings br
                    LEFT JOIN students s ON br.StudentID = s.StudentID
                    LEFT JOIN books b ON br.BookID = b.BookID
                    LEFT JOIN `groups` g ON s.`Group` = g.GroupID
                    ORDER BY STR_TO_DATE(br.BorrowDate, '%d.%m.%Y') DESC";

            borrowingsTable = DatabaseHelper.ExecuteQuery(query);
            borrowingsView = new DataView(borrowingsTable);
            dgvBorrowings.DataSource = borrowingsView;

            if (dgvBorrowings.Columns.Contains("BorrowingID"))
                dgvBorrowings.Columns["BorrowingID"].Visible = false;

            // ИЗМЕНЕНИЕ: Устанавливаем формат отображения даты, чтобы не показывалось время "00:00:00"
            if (dgvBorrowings.Columns.Contains("Дата заимствования"))
                dgvBorrowings.Columns["Дата заимствования"].DefaultCellStyle.Format = "dd.MM.yyyy";

            if (dgvBorrowings.Columns.Contains("Дата возврата"))
                dgvBorrowings.Columns["Дата возврата"].DefaultCellStyle.Format = "dd.MM.yyyy";
        }

        private void ApplyFilters()
        {
            if (borrowingsView == null) return;

            string filter = "";

            // Живой поиск
            if (!string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                string searchText = txtSearch.Text.Replace("'", "''");

                // ИЗМЕНЕНИЕ: Добавлено CONVERT(..., 'System.String') для полей с датами
                filter = "([ФИО студента] LIKE '%" + searchText + "%' OR " +
                        "[Книга] LIKE '%" + searchText + "%' OR " +
                        "CONVERT([Дата заимствования], 'System.String') LIKE '%" + searchText + "%' OR " +
                        "CONVERT([Дата возврата], 'System.String') LIKE '%" + searchText + "%')";
            }

            borrowingsView.RowFilter = filter;
            ApplySort();
        }

        private void ApplySort()
        {
            if (borrowingsView == null) return;

            switch (cmbSort.SelectedIndex)
            {
                case 1: // По дате заимствования (новые)
                    borrowingsView.Sort = "[Дата заимствования] DESC";
                    break;
                case 2: // По дате заимствования (старые)
                    borrowingsView.Sort = "[Дата заимствования] ASC";
                    break;
                case 3: // По дате возврата (новые)
                    borrowingsView.Sort = "[Дата возврата] DESC";
                    break;
                case 4: // По дате возврата (старые)
                    borrowingsView.Sort = "[Дата возврата] ASC";
                    break;
                default:
                    borrowingsView.Sort = "";
                    break;
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void cmbSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplySort();
        }

        private void btnResetFilters_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            cmbSort.SelectedIndex = 0;
            LoadBorrowings();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            BorrowingEditForm editForm = new BorrowingEditForm();
            if (editForm.ShowDialog() == DialogResult.OK)
            {
                LoadBorrowings();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvBorrowings.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите запись для редактирования!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int borrowingId = Convert.ToInt32(dgvBorrowings.SelectedRows[0].Cells["BorrowingID"].Value);
            BorrowingEditForm editForm = new BorrowingEditForm(borrowingId);
            if (editForm.ShowDialog() == DialogResult.OK)
            {
                LoadBorrowings();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvBorrowings.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите запись для удаления!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (InputHelper.ConfirmDelete())
            {
                int borrowingId = Convert.ToInt32(dgvBorrowings.SelectedRows[0].Cells["BorrowingID"].Value);

                string query = "DELETE FROM borrowings WHERE BorrowingID = " + borrowingId;
                int result = DatabaseHelper.ExecuteNonQuery(query);

                if (result > 0)
                {
                    MessageBox.Show("Запись успешно удалена!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadBorrowings();
                }
            }
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            if (dgvBorrowings.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите запись для отметки возврата!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Проверяем, не возвращена ли уже книга
            string returnDate = dgvBorrowings.SelectedRows[0].Cells["Дата возврата"].Value?.ToString();
            if (!string.IsNullOrEmpty(returnDate))
            {
                MessageBox.Show("Книга уже возвращена!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (MessageBox.Show("Отметить возврат книги на сегодняшнюю дату?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                int borrowingId = Convert.ToInt32(dgvBorrowings.SelectedRows[0].Cells["BorrowingID"].Value);
                string today = DateTime.Now.ToString("dd.MM.yyyy");

                // Получаем BookID и количество из записи о выдаче
                string selectQuery = "SELECT BookID, Count FROM borrowings WHERE BorrowingID = " + borrowingId;
                DataTable dt = DatabaseHelper.ExecuteQuery(selectQuery);

                if (dt != null && dt.Rows.Count > 0)
                {
                    int bookId = Convert.ToInt32(dt.Rows[0]["BookID"]);
                    int count = Convert.ToInt32(dt.Rows[0]["Count"]);

                    // Обновляем дату возврата
                    string updateBorrowingQuery = "UPDATE borrowings SET ReturnDate = '" + today +
                                                  "' WHERE BorrowingID = " + borrowingId;
                    int result = DatabaseHelper.ExecuteNonQuery(updateBorrowingQuery);

                    if (result > 0)
                    {
                        // Возвращаем количество книг в AvailableNow
                        string updateBookQuery = "UPDATE books SET AvailableNow = AvailableNow + " + count +
                                                 " WHERE BookID = " + bookId;
                        DatabaseHelper.ExecuteNonQuery(updateBookQuery);

                        MessageBox.Show("Возврат отмечен!", "Успех",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadBorrowings();
                    }
                }
                else
                {
                    MessageBox.Show("Запись о выдаче не найдена!", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadBorrowings();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}