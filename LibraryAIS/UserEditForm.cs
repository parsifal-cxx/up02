using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;

namespace LibraryAIS
{
    public partial class UserEditForm : Form
    {
        private int userId = 0;
        private bool isEditMode = false;

        public UserEditForm(int id = 0)
        {
            InitializeComponent();
            userId = id;
            isEditMode = (id > 0);
        }

        private void UserEditForm_Load(object sender, EventArgs e)
        {
            // Загрузка ролей
            cmbRole.DataSource = DatabaseHelper.ExecuteQuery("SELECT RoleID, Name FROM roles ORDER BY RoleID");
            cmbRole.DisplayMember = "Name"; cmbRole.ValueMember = "RoleID";

            if (isEditMode)
            {
                lblTitle.Text = "Редактирование пользователя";
                DataRow r = DatabaseHelper.ExecuteQuery($"SELECT * FROM users WHERE UserID={userId}").Rows[0];
                txtLogin.Text = r["Login"].ToString();
                txtPassword.Text = r["Password"].ToString();
                txtSurname.Text = r["Surname"].ToString();
                txtName.Text = r["Name"].ToString();
                txtPatronymic.Text = r["Patronymic"].ToString();
                cmbRole.SelectedValue = r["Role"];
            }
            else
            {
                lblTitle.Text = "Новый пользователь";
            }
        }

        // Ограничение ввода логина: [a-zA-Z0-9_]
        private void txtLogin_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar)) return;
            if (!Regex.IsMatch(e.KeyChar.ToString(), "[a-zA-Z0-9_]")) e.Handled = true;
        }

        // Ограничение ФИО (только русские и дефис)
        private void txtFIO_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar)) return;
            bool isCyrillic = (e.KeyChar >= 'А' && e.KeyChar <= 'я') || e.KeyChar == 'ё' || e.KeyChar == 'Ё';
            if (!isCyrillic && e.KeyChar != '-') e.Handled = true;
        }

        private void txtFIO_Leave(object sender, EventArgs e) => InputHelper.CapitalizeFirstLetter(sender, e);

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!InputHelper.ValidateNotEmpty(txtLogin, "Логин")) return;
            if (!InputHelper.ValidateNotEmpty(txtPassword, "Пароль")) return;
            if (!InputHelper.ValidateNotEmpty(txtSurname, "Фамилия")) return;
            if (!InputHelper.ValidateNotEmpty(txtName, "Имя")) return;

            // ПРОВЕРКА ДУБЛИКАТОВ ЛОГИНА
            if (DatabaseHelper.IsDuplicate("users", "Login", txtLogin.Text, "UserID", userId))
            {
                MessageBox.Show("Пользователь с таким логином уже существует!", "Дубликат", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string query = isEditMode
                ? "UPDATE users SET Login=@l, Password=@p, Surname=@s, Name=@n, Patronymic=@pat, Role=@r WHERE UserID=@id"
                : "INSERT INTO users (Login, Password, Surname, Name, Patronymic, Role) VALUES (@l, @p, @s, @n, @pat, @r)";

            MySqlParameter[] p = {
                new MySqlParameter("@l", txtLogin.Text.Trim()),
                new MySqlParameter("@p", txtPassword.Text),
                new MySqlParameter("@s", txtSurname.Text.Trim()),
                new MySqlParameter("@n", txtName.Text.Trim()),
                new MySqlParameter("@pat", txtPatronymic.Text.Trim()),
                new MySqlParameter("@r", cmbRole.SelectedValue),
                new MySqlParameter("@id", userId)
            };

            DatabaseHelper.ExecuteNonQuery(query, p);
            MessageBox.Show("Сохранено!", "Успех");
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e) => Close();
        private void UserEditForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult != DialogResult.OK && DialogResult != DialogResult.Cancel && !InputHelper.ConfirmClose()) e.Cancel = true;
        }
    }
}