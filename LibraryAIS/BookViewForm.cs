using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace LibraryAIS
{
    public partial class BookViewForm : Form
    {
        private int _bookId;

        public BookViewForm(int bookId)
        {
            InitializeComponent();
            _bookId = bookId;
        }

        private void BookViewForm_Load(object sender, EventArgs e)
        {
            LoadBookInfo();
        }

        private void LoadBookInfo()
        {
            string query = @"SELECT 
                            b.Title,
                            CONCAT(a1.Surname, ' ', a1.Name, ' ', IFNULL(a1.Patronymic, '')) as Author1,
                            IFNULL(CONCAT(a2.Surname, ' ', a2.Name, ' ', IFNULL(a2.Patronymic, '')), '') as Author2,
                            p.Name as Publisher,
                            c.Name as Category,
                            b.Year,
                            CAST(b.Count AS SIGNED) as Count,
                            CAST(b.AvailableNow AS SIGNED) as AvailableNow,
                            b.ImagePath
                            FROM books b
                            LEFT JOIN authors a1 ON b.AuthorOne = a1.AuthorID
                            LEFT JOIN authors a2 ON b.AuthorTwo = a2.AuthorID
                            LEFT JOIN publishers p ON b.Publisher = p.PublisherID
                            LEFT JOIN bookcategories bc ON b.BookID = bc.BookID
                            LEFT JOIN categories c ON bc.CategoryID = c.CategoryID
                            WHERE b.BookID = " + _bookId;

            DataTable dt = DatabaseHelper.ExecuteQuery(query);

            if (dt.Rows.Count == 0)
                return;

            DataRow row = dt.Rows[0];

            lblTitle.Text = row["Title"].ToString();

            string authors = row["Author1"].ToString();
            if (!string.IsNullOrWhiteSpace(row["Author2"].ToString()))
                authors += ", " + row["Author2"].ToString();
            lblAuthor.Text = authors;

            lblCategory.Text = row["Category"].ToString();
            lblPublisher.Text = row["Publisher"].ToString();
            lblYear.Text = row["Year"].ToString();
            lblCount.Text = row["Count"].ToString();
            lblAvailable.Text = row["AvailableNow"].ToString();

            // Картинка
            ClearPictureBoxImage();

            if (row["ImagePath"] != DBNull.Value)
            {
                object imgObj = row["ImagePath"];

                // В новых записях должно быть byte[]
                if (imgObj is byte[] bytes && bytes.Length > 0)
                {
                    SetPictureBoxImageFromBytes(bytes);
                }
                else
                {
                    // Если там вдруг строка (старые записи) — просто игнорируем
                    ClearPictureBoxImage();
                }
            }

            // Цвет доступности
            if (int.TryParse(lblAvailable.Text, out int availableCount))
            {
                if (availableCount == 0)
                {
                    lblAvailable.ForeColor = Color.Red;
                    lblAvailableLabel.ForeColor = Color.Red;
                }
                else
                {
                    var green = Color.FromArgb(46, 204, 113);
                    lblAvailable.ForeColor = green;
                    lblAvailableLabel.ForeColor = green;
                }
            }
        }

        private void SetPictureBoxImageFromBytes(byte[] bytes)
        {
            try
            {
                using (var ms = new MemoryStream(bytes))
                using (var bmp = new Bitmap(ms))
                {
                    // клон, чтобы изображение не зависело от потока
                    var copy = new Bitmap(bmp);

                    ClearPictureBoxImage();
                    picBook.Image = copy;
                }
            }
            catch
            {
                ClearPictureBoxImage();
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}