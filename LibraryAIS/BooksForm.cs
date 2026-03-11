using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace LibraryAIS
{
    public partial class BooksForm : Form
    {
        private DataTable booksTable;
        private DataView booksView;

        public BooksForm()
        {
            InitializeComponent();
        }

        private void BooksForm_Load(object sender, EventArgs e)
        {
            LoadCategories();
            LoadPublishers();
            LoadSortOptions();
            LoadBooks();
            ConfigureButtonsByRole();
        }

        private void ConfigureButtonsByRole()
        {
            // Библиотекарь может только просматривать
            if (CurrentUser.IsLibrarian)
            {
                btnPurchase.Visible = false;
                btnWriteOff.Visible = false;
                btnEdit.Visible = false;
            }
        }

        private void LoadCategories()
        {
            string query = "SELECT CategoryID, Name FROM categories ORDER BY Name";
            DataTable dt = DatabaseHelper.ExecuteQuery(query);

            DataRow emptyRow = dt.NewRow();
            emptyRow["CategoryID"] = 0;
            emptyRow["Name"] = "Все категории";
            dt.Rows.InsertAt(emptyRow, 0);

            cmbFilterCategory.DataSource = dt;
            cmbFilterCategory.DisplayMember = "Name";
            cmbFilterCategory.ValueMember = "CategoryID";
            cmbFilterCategory.SelectedIndex = 0;
        }

        private void LoadPublishers()
        {
            string query = "SELECT PublisherID, Name FROM publishers ORDER BY Name";
            DataTable dt = DatabaseHelper.ExecuteQuery(query);

            DataRow emptyRow = dt.NewRow();
            emptyRow["PublisherID"] = 0;
            emptyRow["Name"] = "Все издательства";
            dt.Rows.InsertAt(emptyRow, 0);

            cmbFilterPublisher.DataSource = dt;
            cmbFilterPublisher.DisplayMember = "Name";
            cmbFilterPublisher.ValueMember = "PublisherID";
            cmbFilterPublisher.SelectedIndex = 0;
        }

        private void LoadSortOptions()
        {
            cmbSort.Items.Clear();
            cmbSort.Items.Add("Без сортировки");
            cmbSort.Items.Add("По названию (А-Я)");
            cmbSort.Items.Add("По названию (Я-А)");
            cmbSort.Items.Add("По году (сначала новые)");
            cmbSort.Items.Add("По году (сначала старые)");
            cmbSort.SelectedIndex = 0;
        }

        private void LoadBooks()
        {
            string query = @"SELECT b.BookID, b.Title as 'Название', 
                    CONCAT(a1.Surname, ' ', a1.Name, ' ', IFNULL(a1.Patronymic, '')) as 'Автор',
                    IFNULL(CONCAT(a2.Surname, ' ', a2.Name, ' ', IFNULL(a2.Patronymic, '')), '') as 'Соавтор',
                    c.Name as 'Категория',
                    p.Name as 'Издательство',
                    b.Year as 'Год',
                    CAST(b.Count AS SIGNED) as 'Всего',
                    CAST(b.AvailableNow AS SIGNED) as 'Доступно',
                    b.Publisher as PublisherID
                    FROM books b
                    LEFT JOIN authors a1 ON b.AuthorOne = a1.AuthorID
                    LEFT JOIN authors a2 ON b.AuthorTwo = a2.AuthorID
                    LEFT JOIN publishers p ON b.Publisher = p.PublisherID
                    LEFT JOIN bookcategories bc ON b.BookID = bc.BookID
                    LEFT JOIN categories c ON bc.CategoryID = c.CategoryID
                    ORDER BY b.Title";

            booksTable = DatabaseHelper.ExecuteQuery(query);
            booksView = new DataView(booksTable);
            dgvBooks.DataSource = booksView;

            // Скрываем служебные колонки
            if (dgvBooks.Columns.Contains("BookID"))
                dgvBooks.Columns["BookID"].Visible = false;
            if (dgvBooks.Columns.Contains("PublisherID"))
                dgvBooks.Columns["PublisherID"].Visible = false;
        }

        private void ApplyFilters()
        {
            if (booksView == null) return;

            string filter = "";

            if (!string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                string searchText = txtSearch.Text.Replace("'", "''");
                filter = "(Название LIKE '%" + searchText + "%' OR Автор LIKE '%" + searchText + "%')";
            }

            if (cmbFilterPublisher.SelectedValue != null)
            {
                int publisherId = 0;
                if (int.TryParse(cmbFilterPublisher.SelectedValue.ToString(), out publisherId))
                {
                    if (publisherId > 0)
                    {
                        if (!string.IsNullOrEmpty(filter)) filter += " AND ";
                        filter += "PublisherID = " + publisherId;
                    }
                }
            }

            booksView.RowFilter = filter;
            ApplySort();
        }

        private void ApplySort()
        {
            if (booksView == null) return;

            switch (cmbSort.SelectedIndex)
            {
                case 1: booksView.Sort = "Название ASC"; break;
                case 2: booksView.Sort = "Название DESC"; break;
                case 3: booksView.Sort = "Год DESC"; break;
                case 4: booksView.Sort = "Год ASC"; break;
                default: booksView.Sort = ""; break;
            }
        }

        private void FilterByCategory()
        {
            if (cmbFilterCategory.SelectedValue == null) return;

            int categoryId = 0;
            if (!int.TryParse(cmbFilterCategory.SelectedValue.ToString(), out categoryId)) return;

            string query;
            if (categoryId > 0)
            {
                query = @"SELECT b.BookID, b.Title as 'Название', 
                CONCAT(a1.Surname, ' ', a1.Name, ' ', IFNULL(a1.Patronymic, '')) as 'Автор',
                IFNULL(CONCAT(a2.Surname, ' ', a2.Name, ' ', IFNULL(a2.Patronymic, '')), '') as 'Соавтор',
                c.Name as 'Категория',
                p.Name as 'Издательство',
                b.Year as 'Год',
                CAST(b.Count AS SIGNED) as 'Всего',
                CAST(b.AvailableNow AS SIGNED) as 'Доступно',
                b.Publisher as PublisherID
                FROM books b
                LEFT JOIN authors a1 ON b.AuthorOne = a1.AuthorID
                LEFT JOIN authors a2 ON b.AuthorTwo = a2.AuthorID
                LEFT JOIN publishers p ON b.Publisher = p.PublisherID
                INNER JOIN bookcategories bc ON b.BookID = bc.BookID
                LEFT JOIN categories c ON bc.CategoryID = c.CategoryID
                WHERE bc.CategoryID = " + categoryId + @"
                ORDER BY b.Title";
            }
            else
            {
                query = @"SELECT b.BookID, b.Title as 'Название', 
                CONCAT(a1.Surname, ' ', a1.Name, ' ', IFNULL(a1.Patronymic, '')) as 'Автор',
                IFNULL(CONCAT(a2.Surname, ' ', a2.Name, ' ', IFNULL(a2.Patronymic, '')), '') as 'Соавтор',
                c.Name as 'Категория',
                p.Name as 'Издательство',
                b.Year as 'Год',
                CAST(b.Count AS SIGNED) as 'Всего',
                CAST(b.AvailableNow AS SIGNED) as 'Доступно',
                b.Publisher as PublisherID
                FROM books b
                LEFT JOIN authors a1 ON b.AuthorOne = a1.AuthorID
                LEFT JOIN authors a2 ON b.AuthorTwo = a2.AuthorID
                LEFT JOIN publishers p ON b.Publisher = p.PublisherID
                LEFT JOIN bookcategories bc ON b.BookID = bc.BookID
                LEFT JOIN categories c ON bc.CategoryID = c.CategoryID
                ORDER BY b.Title";
            }

            booksTable = DatabaseHelper.ExecuteQuery(query);
            booksView = new DataView(booksTable);
            dgvBooks.DataSource = booksView;

            if (dgvBooks.Columns.Contains("BookID")) dgvBooks.Columns["BookID"].Visible = false;
            if (dgvBooks.Columns.Contains("PublisherID")) dgvBooks.Columns["PublisherID"].Visible = false;

            ApplyFilters();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e) { ApplyFilters(); }
        private void cmbFilterCategory_SelectedIndexChanged(object sender, EventArgs e) { FilterByCategory(); }
        private void cmbFilterPublisher_SelectedIndexChanged(object sender, EventArgs e) { ApplyFilters(); }
        private void cmbSort_SelectedIndexChanged(object sender, EventArgs e) { ApplySort(); }

        private void btnResetFilters_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            cmbFilterCategory.SelectedIndex = 0;
            cmbFilterPublisher.SelectedIndex = 0;
            cmbSort.SelectedIndex = 0;
            LoadBooks();
        }

        // ============================================
        // ЛОГИКА ЗАКУПКИ И СПИСАНИЯ
        // ============================================

        private void btnPurchase_Click(object sender, EventArgs e)
        {
            // 1. Диалог: Новая или Существующая?
            DialogResult result = MessageBox.Show(
                "Вы оформляете поступление совершенно новой книги, которой еще нет в каталоге?\n\n" +
                "ДА - Создать новую карточку книги и оформить приход.\n" +
                "НЕТ - Добавить экземпляры к уже существующей книге.",
                "Тип поступления",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question);

            if (result == DialogResult.Cancel) return;

            if (result == DialogResult.Yes)
            {
                // Новая книга -> BookEditForm
                BookEditForm editForm = new BookEditForm();
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    LoadBooks();
                }
            }
            else
            {
                // Существующая книга -> StockOperationForm
                if (dgvBooks.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Сначала выберите книгу в таблице, к которой нужно добавить экземпляры!",
                        "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int bookId = Convert.ToInt32(dgvBooks.SelectedRows[0].Cells["BookID"].Value);
                StockOperationForm stockForm = new StockOperationForm(bookId, true);
                if (stockForm.ShowDialog() == DialogResult.OK)
                {
                    LoadBooks();
                }
            }
        }

        private void btnWriteOff_Click(object sender, EventArgs e)
        {
            if (dgvBooks.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите книгу для списания!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int bookId = Convert.ToInt32(dgvBooks.SelectedRows[0].Cells["BookID"].Value);

            // Открываем форму списания
            StockOperationForm stockForm = new StockOperationForm(bookId, false);

            if (stockForm.ShowDialog() == DialogResult.OK)
            {
                LoadBooks(); // Обновляем данные

                // Проверка на нулевой остаток
                int currentCount = Convert.ToInt32(DatabaseHelper.ExecuteScalar(
                    "SELECT Count FROM books WHERE BookID = " + bookId, null));

                if (currentCount <= 0)
                {
                    if (MessageBox.Show(
                        "Все экземпляры этой книги списаны (количество = 0).\n" +
                        "Хотите полностью удалить карточку книги из каталога?",
                        "Удаление пустой карточки",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        DatabaseHelper.ExecuteNonQuery("DELETE FROM bookcategories WHERE BookID = " + bookId);
                        DatabaseHelper.ExecuteNonQuery("DELETE FROM books WHERE BookID = " + bookId);
                        MessageBox.Show("Карточка книги удалена.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadBooks();
                    }
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvBooks.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите книгу для редактирования!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int bookId = Convert.ToInt32(dgvBooks.SelectedRows[0].Cells["BookID"].Value);
            BookEditForm editForm = new BookEditForm(bookId);
            if (editForm.ShowDialog() == DialogResult.OK)
            {
                LoadBooks();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e) { LoadBooks(); }
        private void btnClose_Click(object sender, EventArgs e) { this.Close(); }
        private void BooksForm_FormClosing(object sender, FormClosingEventArgs e) { }

        private void dgvBooks_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Проверяем, что кликнули по строке, а не по заголовку
            if (e.RowIndex >= 0)
            {
                int bookId = Convert.ToInt32(dgvBooks.Rows[e.RowIndex].Cells["BookID"].Value);

                // Открываем форму просмотра
                BookViewForm viewForm = new BookViewForm(bookId);
                viewForm.ShowDialog();
            }
        }
    }
}