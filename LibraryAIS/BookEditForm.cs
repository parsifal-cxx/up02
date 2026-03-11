using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace LibraryAIS
{
    public partial class BookEditForm : Form
    {
        private int bookId = 0;
        private bool isEditMode = false;

        //хранение картинки как byte[] (для записи в LONGBLOB)
        private byte[] selectedImageBytes = null;     // новая выбранная картинка
        private byte[] currentDbImageBytes = null;    // картинка из БД (при редактировании)

        public BookEditForm()
        {
            InitializeComponent();
            isEditMode = false;
            numCount.ValueChanged += NumCount_ValueChanged;
        }

        public BookEditForm(int id)
        {
            InitializeComponent();
            bookId = id;
            isEditMode = true;
        }

        private void BookEditForm_Load(object sender, EventArgs e)
        {
            LoadAuthors();
            LoadPublishers();
            LoadCategories();

            if (isEditMode)
            {
                lblTitle.Text = "Редактирование данных книги";
                numCount.Enabled = false;
                numAvailable.Enabled = false;
                LoadBookData();
            }
            else
            {
                lblTitle.Text = "Добавление новой книги";
                lblAvailable.Visible = false;
                numAvailable.Visible = false;
                numAvailable.Value = numCount.Value;
            }
        }

        private void NumCount_ValueChanged(object sender, EventArgs e)
        {
            if (!isEditMode) numAvailable.Value = numCount.Value;
        }

        private void LoadAuthors()
        {
            string query = @"SELECT AuthorID, CONCAT(Surname, ' ', Name, ' ', IFNULL(Patronymic, '')) as FullName 
                             FROM authors ORDER BY Surname";
            DataTable dt = DatabaseHelper.ExecuteQuery(query);

            DataTable dtWithEmpty = dt.Copy();
            DataRow emptyRow = dtWithEmpty.NewRow();
            emptyRow["AuthorID"] = DBNull.Value;
            emptyRow["FullName"] = "(нет соавтора)";
            dtWithEmpty.Rows.InsertAt(emptyRow, 0);

            cmbAuthorOne.DataSource = dt;
            cmbAuthorOne.DisplayMember = "FullName";
            cmbAuthorOne.ValueMember = "AuthorID";

            cmbAuthorTwo.DataSource = dtWithEmpty;
            cmbAuthorTwo.DisplayMember = "FullName";
            cmbAuthorTwo.ValueMember = "AuthorID";
            cmbAuthorTwo.SelectedIndex = 0;
        }

        private void LoadPublishers()
        {
            string query = "SELECT PublisherID, Name FROM publishers ORDER BY Name";
            DataTable dt = DatabaseHelper.ExecuteQuery(query);
            cmbPublisher.DataSource = dt;
            cmbPublisher.DisplayMember = "Name";
            cmbPublisher.ValueMember = "PublisherID";
        }

        private void LoadCategories()
        {
            string query = "SELECT CategoryID, Name FROM categories ORDER BY Name";
            DataTable dt = DatabaseHelper.ExecuteQuery(query);
            cmbCategory.DataSource = dt;
            cmbCategory.DisplayMember = "Name";
            cmbCategory.ValueMember = "CategoryID";
        }

        private void LoadBookData()
        {
            // Параметризуем
            string query = @"SELECT Title, AuthorOne, AuthorTwo, Publisher, Year, 
                                    CAST(Count AS SIGNED) as Count, 
                                    CAST(AvailableNow AS SIGNED) as AvailableNow,
                                    ImagePath
                             FROM books WHERE BookID = @id";

            var dt = DatabaseHelper.ExecuteQuery(query, new MySqlParameter[]
            {
                new MySqlParameter("@id", bookId)
            });

            if (dt.Rows.Count == 0) return;

            DataRow row = dt.Rows[0];
            txtTitle.Text = row["Title"].ToString();
            cmbAuthorOne.SelectedValue = row["AuthorOne"];

            if (row["AuthorTwo"] != DBNull.Value) cmbAuthorTwo.SelectedValue = row["AuthorTwo"];
            if (row["Publisher"] != DBNull.Value) cmbPublisher.SelectedValue = row["Publisher"];
            if (row["Year"] != DBNull.Value) numYear.Value = Convert.ToDecimal(row["Year"]);

            numCount.Value = Convert.ToDecimal(row["Count"]);
            numAvailable.Value = Convert.ToDecimal(row["AvailableNow"]);

            // загрузка картинки из LONGBLOB
            picBook.Image = null;
            currentDbImageBytes = null;

            ClearPictureBoxImage();
            currentDbImageBytes = null;

            if (row["ImagePath"] != DBNull.Value)
            {
                object imgObj = row["ImagePath"];

                if (imgObj is byte[] bytes && bytes.Length > 0)
                {
                    currentDbImageBytes = bytes;
                    SetPictureBoxImageFromBytes(bytes);
                }
                else if (imgObj is string s)
                {
                    MessageBox.Show("В ImagePath хранится строка (старый формат): " + s);
                }
            }
        } 

        // ВЫБОР КАРТИНКИ (читаем в byte[])
        private void btnSelectImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Изображения|*.jpg;*.jpeg;*.png;*.bmp";
                if (ofd.ShowDialog() != DialogResult.OK)
                    return;

                FileInfo fileInfo = new FileInfo(ofd.FileName);

                // Проверка размера (2 МБ)
                if (fileInfo.Length > 2 * 1024 * 1024)
                {
                    MessageBox.Show("Размер изображения не должен превышать 2 МБ!",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // сразу читаем байты
                selectedImageBytes = File.ReadAllBytes(ofd.FileName);

                // Предпросмотр
                SetPictureBoxImageFromBytes(selectedImageBytes);
            }
        }

        //делаем копию изображения, чтобы не зависеть от потока
        private void SetPictureBoxImageFromBytes(byte[] bytes)
        {
            // очистка если пусто
            if (bytes == null || bytes.Length < 10)
            {
                ClearPictureBoxImage();
                return;
            }

            try
            {
                using (var ms = new MemoryStream(bytes))
                using (var bmp = new Bitmap(ms))     // Bitmap сразу из потока
                {
                    // Клонируем, чтобы не зависеть от потока
                    var copy = new Bitmap(bmp);

                    ClearPictureBoxImage();
                    picBook.Image = copy;
                }
            }
            catch (ArgumentException)
            {
                ClearPictureBoxImage();
                MessageBox.Show("Данные изображения повреждены или имеют старый формат.");
            }
        }

        private void ClearPictureBoxImage()
        {
            if (picBook.Image != null)
            {
                var old = picBook.Image;
                picBook.Image = null;
                old.Dispose();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Валидация полей
            if (!InputHelper.ValidateNotEmpty(txtTitle, "Название")) return;
            if (!InputHelper.ValidateComboBoxSelected(cmbAuthorOne, "Автор")) return;
            if (!InputHelper.ValidateComboBoxSelected(cmbPublisher, "Издательство")) return;

            string title = txtTitle.Text.Trim();
            int authorOne = Convert.ToInt32(cmbAuthorOne.SelectedValue);

            object authorTwoVal = cmbAuthorTwo.SelectedValue;
            object authorTwo = (authorTwoVal == null || authorTwoVal == DBNull.Value) ? DBNull.Value : authorTwoVal;

            int publisher = Convert.ToInt32(cmbPublisher.SelectedValue);
            int year = Convert.ToInt32(numYear.Value);
            int count = Convert.ToInt32(numCount.Value);

            int available = Convert.ToInt32(numAvailable.Value);
            int categoryId = Convert.ToInt32(cmbCategory.SelectedValue);

            // Новое: финальные байты картинки (если не выбрали новую — сохраняем старую)
            byte[] finalImageBytes = selectedImageBytes ?? currentDbImageBytes;

            // Проверка на дубликат
            string checkQuery = @"SELECT COUNT(*) FROM books 
                                  WHERE Title = @title 
                                  AND AuthorOne = @a1 
                                  AND Publisher = @pub 
                                  AND Year = @year
                                  AND BookID != @id";

            MySqlParameter[] checkParams = {
                new MySqlParameter("@title", title),
                new MySqlParameter("@a1", authorOne),
                new MySqlParameter("@pub", publisher),
                new MySqlParameter("@year", year),
                new MySqlParameter("@id", bookId)
            };

            if (DatabaseHelper.IsDuplicateComposite(checkQuery, checkParams))
            {
                MessageBox.Show(
                    "Такая книга уже существует в каталоге!\n\nЕсли вы хотите добавить экземпляры, используйте кнопку 'Закупка / Поступление' на главной форме.",
                    "Дубликат найден", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                string query;
                MySqlParameter[] parameters;

                // Новое: параметр LONGBLOB
                MySqlParameter imageParam = new MySqlParameter("@image", MySqlDbType.LongBlob);
                imageParam.Value = (object)finalImageBytes ?? DBNull.Value;

                if (isEditMode)
                {
                    query = @"UPDATE books SET Title = @title, AuthorOne = @author1, 
                                               AuthorTwo = @author2, Publisher = @publisher, Year = @year, 
                                               ImagePath = @image
                              WHERE BookID = @id";

                    parameters = new MySqlParameter[]
                    {
                        new MySqlParameter("@title", title),
                        new MySqlParameter("@author1", authorOne),
                        new MySqlParameter("@author2", authorTwo),
                        new MySqlParameter("@publisher", publisher),
                        new MySqlParameter("@year", year),
                        imageParam,
                        new MySqlParameter("@id", bookId)
                    };

                    DatabaseHelper.ExecuteNonQuery(query, parameters);

                    DatabaseHelper.ExecuteNonQuery("DELETE FROM bookcategories WHERE BookID = " + bookId);
                    DatabaseHelper.ExecuteNonQuery("INSERT INTO bookcategories (BookID, CategoryID) VALUES (" + bookId + ", " + categoryId + ")");
                }
                else
                {
                    query = @"INSERT INTO books (Title, AuthorOne, AuthorTwo, Publisher, Year, Count, AvailableNow, ImagePath) 
                              VALUES (@title, @author1, @author2, @publisher, @year, @count, @available, @image)";

                    parameters = new MySqlParameter[]
                    {
                        new MySqlParameter("@title", title),
                        new MySqlParameter("@author1", authorOne),
                        new MySqlParameter("@author2", authorTwo),
                        new MySqlParameter("@publisher", publisher),
                        new MySqlParameter("@year", year),
                        new MySqlParameter("@count", count),
                        new MySqlParameter("@available", available),
                        imageParam
                    };

                    DatabaseHelper.ExecuteNonQuery(query, parameters);

                    object lastIdObj = DatabaseHelper.ExecuteScalar("SELECT LAST_INSERT_ID()", null);
                    bookId = Convert.ToInt32(lastIdObj);

                    DatabaseHelper.ExecuteNonQuery("DELETE FROM bookcategories WHERE BookID = " + bookId);
                    DatabaseHelper.ExecuteNonQuery("INSERT INTO bookcategories (BookID, CategoryID) VALUES (" + bookId + ", " + categoryId + ")");

                    string date = DateTime.Now.ToString("yyyy-MM-dd");
                    string purchaseQuery = $"INSERT INTO purchases (BookID, Date, Count) VALUES ({bookId}, '{date}', {count})";
                    DatabaseHelper.ExecuteNonQuery(purchaseQuery);
                }

                MessageBox.Show("Данные успешно сохранены!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void BookEditForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult != DialogResult.OK && this.DialogResult != DialogResult.Cancel)
            {
                if (!InputHelper.ConfirmClose()) e.Cancel = true;
            }
        }
    }
}