using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace LibraryAIS
{
    public partial class StudentEditForm : Form
    {
        private int studentId = 0;
        private bool isEditMode = false;

        public StudentEditForm()
        {
            InitializeComponent();
            isEditMode = false;
        }

        public StudentEditForm(int id)
        {
            InitializeComponent();
            studentId = id;
            isEditMode = true;
        }

        private void StudentEditForm_Load(object sender, EventArgs e)
        {
            LoadGroups();

            if (isEditMode)
            {
                lblTitle.Text = "Редактирование студента";
                LoadStudentData();
            }
            else
            {
                lblTitle.Text = "Добавление студента";
            }
        }

        private void LoadGroups()
        {
            string query = "SELECT GroupID, Name FROM `groups` ORDER BY Name";
            DataTable dt = DatabaseHelper.ExecuteQuery(query);

            cmbGroup.DataSource = dt;
            cmbGroup.DisplayMember = "Name";
            cmbGroup.ValueMember = "GroupID";
        }

        private void LoadStudentData()
        {
            string query = "SELECT * FROM students WHERE StudentID = " + studentId;
            DataTable dt = DatabaseHelper.ExecuteQuery(query);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];

                txtSurname.Text = row["Surname"].ToString();
                txtName.Text = row["Name"].ToString();
                txtPatronymic.Text = row["Patronymic"].ToString();

                // Заполняем MaskedTextBox (маска применится автоматически)
                mskPhone.Text = row["Phone"].ToString();

                if (row["Group"] != DBNull.Value)
                {
                    cmbGroup.SelectedValue = row["Group"];
                }
            }
        }

        private void txtFIO_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Блокировка английских букв и цифр, разрешаем только русские буквы и дефис
            if (char.IsControl(e.KeyChar)) return; // Backspace

            // Если нужна строгая проверка:
            bool isCyrillic = (e.KeyChar >= 'А' && e.KeyChar <= 'я') || e.KeyChar == 'ё' || e.KeyChar == 'Ё';
            bool isHyphen = e.KeyChar == '-';

            if (!isCyrillic && !isHyphen)
            {
                e.Handled = true; // Блокируем ввод
            }
        }

        private void txtFIO_Leave(object sender, EventArgs e)
        {
            // Автоматическая заглавная буква
            InputHelper.CapitalizeFirstLetter(sender, e);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // 1. Валидация обязательных полей
            if (!InputHelper.ValidateNotEmpty(txtSurname, "Фамилия")) return;
            if (!InputHelper.ValidateNotEmpty(txtName, "Имя")) return;
            if (!InputHelper.ValidateComboBoxSelected(cmbGroup, "Группа")) return;

            // 2. Валидация телефона (должен быть заполнен по маске)
            if (!mskPhone.MaskCompleted)
            {
                MessageBox.Show("Введите номер телефона полностью!", "Ошибка валидации",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                mskPhone.Focus();
                return;
            }

            string surname = txtSurname.Text.Trim();
            string name = txtName.Text.Trim();
            string patronymic = txtPatronymic.Text.Trim();
            int groupId = Convert.ToInt32(cmbGroup.SelectedValue);
            string phone = mskPhone.Text; // Получаем текст вместе с маской

            // 3. ПРОВЕРКА НА ДУБЛИКАТЫ (Студент с таким ФИО уже есть?)
            // Используем универсальный метод из DatabaseHelper
            string checkQuery = @"SELECT COUNT(*) FROM students 
                                 WHERE Surname = @sur AND Name = @name 
                                 AND Patronymic = @pat AND StudentID != @id";

            MySqlParameter[] checkParams = {
                new MySqlParameter("@sur", surname),
                new MySqlParameter("@name", name),
                new MySqlParameter("@pat", patronymic),
                new MySqlParameter("@id", studentId)
            };

            if (DatabaseHelper.IsDuplicateComposite(checkQuery, checkParams))
            {
                MessageBox.Show("Студент с такими ФИО уже существует в базе!", "Дубликат",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 4. Сохранение данных
            string query;
            MySqlParameter[] parameters;

            if (isEditMode)
            {
                query = @"UPDATE students SET Surname = @surname, Name = @name, 
                         Patronymic = @patronymic, `Group` = @group, Phone = @phone 
                         WHERE StudentID = @id";
                parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@surname", surname),
                    new MySqlParameter("@name", name),
                    new MySqlParameter("@patronymic", patronymic),
                    new MySqlParameter("@group", groupId),
                    new MySqlParameter("@phone", phone),
                    new MySqlParameter("@id", studentId)
                };
            }
            else
            {
                query = @"INSERT INTO students (Surname, Name, Patronymic, `Group`, Phone) 
                         VALUES (@surname, @name, @patronymic, @group, @phone)";
                parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@surname", surname),
                    new MySqlParameter("@name", name),
                    new MySqlParameter("@patronymic", patronymic),
                    new MySqlParameter("@group", groupId),
                    new MySqlParameter("@phone", phone)
                };
            }

            int result = DatabaseHelper.ExecuteNonQuery(query, parameters);

            if (result > 0)
            {
                MessageBox.Show("Данные студента успешно сохранены!", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void StudentEditForm_FormClosing(object sender, FormClosingEventArgs e)
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