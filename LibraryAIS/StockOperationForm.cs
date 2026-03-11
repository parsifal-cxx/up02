using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace LibraryAIS
{
    public partial class StockOperationForm : Form
    {
        private int _bookId;
        private bool _isPurchase; // true = закупка, false = списание

        public StockOperationForm(int bookId, bool isPurchase)
        {
            InitializeComponent();
            _bookId = bookId;
            _isPurchase = isPurchase;
            SetupForm();
        }

        private void SetupForm()
        {
            if (_isPurchase)
            {
                this.Text = "Докупка экземпляров";
                lblTitle.Text = "Поступление (сущ. книга)";
                lblTitle.BackColor = System.Drawing.Color.FromArgb(46, 204, 113); // Зеленый
                lblReason.Visible = false;
                txtReason.Visible = false;
            }
            else
            {
                this.Text = "Списание экземпляров";
                lblTitle.Text = "Акт списания";
                lblTitle.BackColor = System.Drawing.Color.FromArgb(231, 76, 60); // Красный
                lblReason.Visible = true;
                txtReason.Visible = true;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int count = (int)numCount.Value;
            string date = dtpDate.Value.ToString("yyyy-MM-dd");
            string reason = txtReason.Text;

            if (!_isPurchase && string.IsNullOrWhiteSpace(reason))
            {
                MessageBox.Show("Укажите причину списания!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string query;
                string updateBookQuery;

                if (_isPurchase)
                {
                    query = $"INSERT INTO purchases (BookID, Date, Count) VALUES ({_bookId}, '{date}', {count})";
                    updateBookQuery = $"UPDATE books SET Count = Count + {count}, AvailableNow = AvailableNow + {count} WHERE BookID = {_bookId}";
                }
                else
                {
                    // Проверка остатков
                    int currentAvailable = Convert.ToInt32(DatabaseHelper.ExecuteScalar($"SELECT AvailableNow FROM books WHERE BookID = {_bookId}", null));
                    if (count > currentAvailable)
                    {
                        MessageBox.Show($"Нельзя списать {count} шт., доступно только {currentAvailable} шт.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    query = $"INSERT INTO writeoffs (BookID, Date, Count, Reason) VALUES ({_bookId}, '{date}', {count}, '{reason}')";
                    updateBookQuery = $"UPDATE books SET Count = Count - {count}, AvailableNow = AvailableNow - {count} WHERE BookID = {_bookId}";
                }

                DatabaseHelper.ExecuteNonQuery(query);
                DatabaseHelper.ExecuteNonQuery(updateBookQuery);

                MessageBox.Show("Операция выполнена успешно!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            this.Close();
        }
    }
}