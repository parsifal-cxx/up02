using System;
using System.Data;
using System.Globalization;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace LibraryAIS
{
    public partial class BorrowingEditForm : Form
    {
        private int borrowingId = 0;
        private bool isEditMode = false;

        public BorrowingEditForm()
        {
            InitializeComponent();
            isEditMode = false;

            chkReturned.Visible = false;
            chkReturned.Checked = false;
            dtpReturnDate.Enabled = false;
        }

        public BorrowingEditForm(int id)
        {
            InitializeComponent();
            borrowingId = id;
            isEditMode = true;

            chkReturned.Visible = true;
        }

        private void BorrowingEditForm_Load(object sender, EventArgs e)
        {
            LoadStudents();
            LoadBooks();

            dtpBorrowDate.Value = DateTime.Now;
            dtpReturnDate.Value = DateTime.Now;

            dtpReturnDate.Enabled = chkReturned.Checked;

            if (isEditMode)
            {
                lblTitle.Text = "Редактирование заимствования";
                LoadBorrowingData();
            }
        }

        private void LoadStudents()
        {
            string query = @"SELECT s.StudentID, 
                            CONCAT(s.Surname, ' ', s.Name, ' ', IFNULL(s.Patronymic, ''), ' (', g.Name, ')') as FullName 
                            FROM students s
                            LEFT JOIN `groups` g ON s.`Group` = g.GroupID
                            ORDER BY s.Surname";

            DataTable dt = DatabaseHelper.ExecuteQuery(query);

            cmbStudent.DataSource = dt;
            cmbStudent.DisplayMember = "FullName";
            cmbStudent.ValueMember = "StudentID";

            cmbStudent.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbStudent.AutoCompleteSource = AutoCompleteSource.ListItems;
        }

        private void LoadBooks()
        {
            string query;

            if (isEditMode)
            {
                // В режиме редактирования показываем все книги,
                // чтобы текущая книга точно была в списке, даже если AvailableNow = 0
                query = @"SELECT BookID, CONCAT(Title, ' (Доступно: ', CAST(AvailableNow AS SIGNED), ')') as BookInfo 
                          FROM books 
                          ORDER BY Title";
            }
            else
            {
                query = @"SELECT BookID, CONCAT(Title, ' (Доступно: ', CAST(AvailableNow AS SIGNED), ')') as BookInfo 
                          FROM books 
                          WHERE AvailableNow > 0
                          ORDER BY Title";
            }

            DataTable dt = DatabaseHelper.ExecuteQuery(query);

            cmbBook.DataSource = dt;
            cmbBook.DisplayMember = "BookInfo";
            cmbBook.ValueMember = "BookID";

            cmbBook.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbBook.AutoCompleteSource = AutoCompleteSource.ListItems;
        }

        private void LoadBorrowingData()
        {
            string query = "SELECT * FROM borrowings WHERE BorrowingID = " + borrowingId;
            DataTable dt = DatabaseHelper.ExecuteQuery(query);

            if (dt.Rows.Count == 0) return;

            DataRow row = dt.Rows[0];

            cmbStudent.SelectedValue = row["StudentID"];
            cmbBook.SelectedValue = row["BookID"];

            cmbStudent.Enabled = false;
            cmbBook.Enabled = false;
            numCount.Enabled = false;

            // BorrowDate
            if (row["BorrowDate"] != DBNull.Value)
            {
                string borrowStr = row["BorrowDate"].ToString().Trim();
                DateTime bd;

                if (DateTime.TryParseExact(borrowStr, "dd.MM.yyyy",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out bd))
                {
                    dtpBorrowDate.Value = bd;
                }
            }

            // ReturnDate (VARCHAR "dd.MM.yyyy" или NULL)
            if (row["ReturnDate"] != DBNull.Value)
            {
                string returnStr = row["ReturnDate"].ToString().Trim();

                DateTime rd;
                if (!string.IsNullOrWhiteSpace(returnStr) &&
                    DateTime.TryParseExact(returnStr, "dd.MM.yyyy",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out rd))
                {
                    chkReturned.Checked = true;
                    dtpReturnDate.Value = rd;
                }
                else
                {
                    chkReturned.Checked = false;
                }
            }
            else
            {
                chkReturned.Checked = false;
            }

            dtpReturnDate.Enabled = chkReturned.Checked;

            if (row["Count"] != DBNull.Value)
                numCount.Value = Convert.ToDecimal(row["Count"]);
        }

        private void chkReturned_CheckedChanged(object sender, EventArgs e)
        {
            dtpReturnDate.Enabled = chkReturned.Checked;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (cmbStudent.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите студента из списка!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbBook.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите книгу из списка!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int studentId = Convert.ToInt32(cmbStudent.SelectedValue);
            int bookId = Convert.ToInt32(cmbBook.SelectedValue);
            int count = Convert.ToInt32(numCount.Value);

            string borrowDateStr = dtpBorrowDate.Value.ToString("dd.MM.yyyy");

            MySqlParameter pReturnDate = new MySqlParameter("@returnDate", MySqlDbType.VarChar);
            pReturnDate.Size = 10;

            if (chkReturned.Checked)
                pReturnDate.Value = dtpReturnDate.Value.ToString("dd.MM.yyyy");
            else
                pReturnDate.Value = DBNull.Value;

            string query;
            MySqlParameter[] parameters;

            if (isEditMode)
            {
                query = @"UPDATE borrowings 
                          SET BorrowDate = @borrowDate, ReturnDate = @returnDate 
                          WHERE BorrowingID = @id";

                parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@borrowDate", MySqlDbType.VarChar) { Value = borrowDateStr, Size = 10 },
                    pReturnDate,
                    new MySqlParameter("@id", MySqlDbType.Int32) { Value = borrowingId }
                };

                int resultEdit = DatabaseHelper.ExecuteNonQuery(query, parameters);
                if (resultEdit > 0)
                {
                    MessageBox.Show("Запись успешно сохранена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                }

                return;
            }

            // 1) Проверяем остаток
            object scalar = DatabaseHelper.ExecuteScalar(
                "SELECT CAST(AvailableNow AS SIGNED) FROM books WHERE BookID = @bookId",
                new MySqlParameter[]
                {
                    new MySqlParameter("@bookId", MySqlDbType.Int32) { Value = bookId }
                });

            int available = (scalar == null || scalar == DBNull.Value) ? 0 : Convert.ToInt32(scalar);

            if (count > available)
            {
                MessageBox.Show($"Недостаточно книг! Доступно: {available}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 2) Добавляем запись
            query = @"INSERT INTO borrowings (StudentID, BookID, BorrowDate, ReturnDate, Count) 
                      VALUES (@studentId, @bookId, @borrowDate, @returnDate, @count)";

            parameters = new MySqlParameter[]
            {
                new MySqlParameter("@studentId", MySqlDbType.Int32) { Value = studentId },
                new MySqlParameter("@bookId", MySqlDbType.Int32) { Value = bookId },
                new MySqlParameter("@borrowDate", MySqlDbType.VarChar) { Value = borrowDateStr, Size = 10 },
                pReturnDate,
                new MySqlParameter("@count", MySqlDbType.Int32) { Value = count }
            };

            int result = DatabaseHelper.ExecuteNonQuery(query, parameters);

            if (result > 0)
            {
                // 3) Списываем с остатка
                DatabaseHelper.ExecuteNonQuery(
                    "UPDATE books SET AvailableNow = AvailableNow - @cnt WHERE BookID = @bookId",
                    new MySqlParameter[]
                    {
                        new MySqlParameter("@cnt", MySqlDbType.Int32) { Value = count },
                        new MySqlParameter("@bookId", MySqlDbType.Int32) { Value = bookId }
                    });

                MessageBox.Show("Запись успешно сохранена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void BorrowingEditForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult != DialogResult.OK && DialogResult != DialogResult.Cancel)
            {
                if (!InputHelper.ConfirmClose())
                    e.Cancel = true;
            }
        }

        private void cmbStudent_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar)) return;

            bool isCyrillic = (e.KeyChar >= 'А' && e.KeyChar <= 'я') ||
                              e.KeyChar == 'ё' || e.KeyChar == 'Ё' ||
                              e.KeyChar == ' ';

            if (!isCyrillic && e.KeyChar != '-')
                e.Handled = true;
        }
    }
}