using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace LibraryAIS
{
    public partial class AuthorEditForm : Form
    {
        private int _authorId = 0;
        private bool _isEditMode = false;

        public AuthorEditForm(int id = 0)
        {
            InitializeComponent();
            _authorId = id;
            _isEditMode = (id > 0);
        }

        private void AuthorEditForm_Load(object sender, EventArgs e)
        {
            if (_isEditMode)
            {
                lblTitle.Text = "Редактирование автора";
                // Загрузка данных
                string query = "SELECT * FROM authors WHERE AuthorID = " + _authorId;
                DataTable dt = DatabaseHelper.ExecuteQuery(query);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    txtSurname.Text = row["Surname"].ToString();
                    txtName.Text = row["Name"].ToString();
                    txtPatronymic.Text = row["Patronymic"].ToString();
                }
            }
            else
            {
                lblTitle.Text = "Новый автор";
            }
        }

        // Разрешаем только буквы, дефис и Backspace
        private void txtInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            // 1. Разрешаем Backspace
            if (char.IsControl(e.KeyChar)) return;

            // 2. Разрешаем Дефис
            if (e.KeyChar == '-') return;

            // 3. Разрешаем ТОЛЬКО Русские буквы
            if (!((e.KeyChar >= 'А' && e.KeyChar <= 'я') || e.KeyChar == 'ё' || e.KeyChar == 'Ё'))
            {
                e.Handled = true; // Блокируем всё остальное (цифры, пробелы, символы)
            }
        }

        private void txtInput_Leave(object sender, EventArgs e)
        {
            // Автоматически делаем первую букву заглавной
            InputHelper.CapitalizeFirstLetter(sender, e);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Валидация
            if (!InputHelper.ValidateNotEmpty(txtSurname, "Фамилия")) return;
            if (!InputHelper.ValidateNotEmpty(txtName, "Имя")) return;

            string surname = txtSurname.Text.Trim();
            string name = txtName.Text.Trim();
            string patronymic = txtPatronymic.Text.Trim();

            // ПРОВЕРКА НА ДУБЛИКАТЫ
            // Ищем автора с таким же ФИО, исключая текущего (при редактировании)
            string checkQuery = @"SELECT COUNT(*) FROM authors 
                                 WHERE Surname = @sur AND Name = @name 
                                 AND (Patronymic = @pat OR (Patronymic IS NULL AND @pat = ''))
                                 AND AuthorID != @id";

            MySqlParameter[] checkParams = {
                new MySqlParameter("@sur", surname),
                new MySqlParameter("@name", name),
                new MySqlParameter("@pat", patronymic),
                new MySqlParameter("@id", _authorId)
            };

            if (DatabaseHelper.IsDuplicateComposite(checkQuery, checkParams))
            {
                MessageBox.Show("Автор с таким ФИО уже существует в базе!", "Дубликат",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Сохранение
            try
            {
                string query;
                MySqlParameter[] parameters;

                if (_isEditMode)
                {
                    query = "UPDATE authors SET Surname = @surname, Name = @name, Patronymic = @pat WHERE AuthorID = @id";
                    parameters = new MySqlParameter[]
                    {
                        new MySqlParameter("@surname", surname),
                        new MySqlParameter("@name", name),
                        new MySqlParameter("@patronymic", string.IsNullOrWhiteSpace(patronymic) ? (object)DBNull.Value : patronymic),
                        new MySqlParameter("@id", _authorId)
                    };
                }
                else
                {
                    query = "INSERT INTO authors (Surname, Name, Patronymic) VALUES (@surname, @name, @patronymic)";
                    parameters = new MySqlParameter[]
                    {
                        new MySqlParameter("@surname", surname),
                        new MySqlParameter("@name", name),
                        new MySqlParameter("@patronymic", string.IsNullOrWhiteSpace(patronymic) ? (object)DBNull.Value : patronymic)
                    };
                }

                DatabaseHelper.ExecuteNonQuery(query, parameters);

                MessageBox.Show("Автор успешно сохранен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void AuthorEditForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult != DialogResult.OK && this.DialogResult != DialogResult.Cancel)
            {
                if (!InputHelper.ConfirmClose())
                {
                    e.Cancel = true;
                }
            }
        }
    }
}