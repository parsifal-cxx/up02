using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace LibraryAIS
{
    public partial class LoginForm : Form
    {
        private int failedAttempts = 0; // Счетчик неудачных попыток

        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            // Проверка заполнения полей
            if (string.IsNullOrWhiteSpace(txtLogin.Text))
            {
                MessageBox.Show("Введите логин!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLogin.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Введите пароль!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            // Попытка авторизации
            string query = @"SELECT u.UserID, u.Login, u.Name, u.Surname, u.Patronymic, 
                            u.Role, r.Name as RoleName 
                            FROM users u 
                            LEFT JOIN roles r ON u.Role = r.RoleID 
                            WHERE u.Login = @login AND u.Password = @password";

            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@login", txtLogin.Text.Trim()),
                new MySqlParameter("@password", txtPassword.Text)
            };

            DataTable result = DatabaseHelper.ExecuteQuery(query, parameters);

            if (result.Rows.Count > 0)
            {
                // Успешная авторизация
                DataRow user = result.Rows[0];

                CurrentUser.UserID = Convert.ToInt32(user["UserID"]);
                CurrentUser.Login = user["Login"].ToString();
                CurrentUser.Name = user["Name"].ToString();
                CurrentUser.Surname = user["Surname"].ToString();
                CurrentUser.Patronymic = user["Patronymic"].ToString();
                CurrentUser.RoleID = Convert.ToInt32(user["Role"]);
                CurrentUser.RoleName = user["RoleName"].ToString();

                // Сброс счетчика
                failedAttempts = 0;

                // Открываем главную форму
                MainForm mainForm = new MainForm();
                this.Hide();
                mainForm.ShowDialog();

                // После закрытия главной формы - показываем форму авторизации снова
                CurrentUser.Clear();
                txtLogin.Clear();
                txtPassword.Clear();
                this.Show();
            }
            else
            {
                // Неудачная попытка
                failedAttempts++;
                MessageBox.Show("Неверный логин или пароль!", "Ошибка авторизации",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                // При повторной неудачной попытке очищаем поля
                if (failedAttempts > 1)
                {
                    txtLogin.Clear();
                    txtPassword.Clear();
                    txtLogin.Focus();
                }
                else
                {
                    txtPassword.Clear();
                    txtPassword.Focus();
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите выйти из программы?",
                "Подтверждение выхода", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}