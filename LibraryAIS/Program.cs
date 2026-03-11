using System;
using System.Windows.Forms;

namespace LibraryAIS
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Проверка подключения к базе данных
            if (!DatabaseHelper.TestConnection())
            {
                MessageBox.Show(
                    "Не удалось подключиться к базе данных!\n\n" +
                    "Проверьте:\n" +
                    "1. MySQL сервер запущен\n" +
                    "2. База данных db82 существует\n" +
                    "3. Параметры подключения в DatabaseHelper.cs корректны\n\n" +
                    "Приложение будет закрыто.",
                    "Ошибка запуска",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            Application.Run(new LoginForm());
        }
    }
}