using System;
using System.Windows.Forms;

namespace LibraryAIS
{
    public static class InputHelper
    {
        // Блокировка английских букв (разрешены только русские, цифры и спецсимволы)
        public static void BlockEnglishLetters(object sender, KeyPressEventArgs e)
        {
            char c = e.KeyChar;

            // Разрешаем управляющие символы (Backspace и т.д.)
            if (char.IsControl(c))
            {
                return;
            }

            // Разрешаем цифры
            if (char.IsDigit(c))
            {
                return;
            }

            // Разрешаем пробел и знаки препинания
            if (c == ' ' || c == '-' || c == '.' || c == ',')
            {
                return;
            }

            // Проверяем, является ли символ русской буквой
            if ((c >= 'А' && c <= 'я') || c == 'ё' || c == 'Ё')
            {
                return;
            }

            // Все остальное блокируем
            e.Handled = true;
        }

        public static void BlockEnglishLettersAndDigits(object sender, KeyPressEventArgs e)
        {
            char c = e.KeyChar;

            // Разрешаем управляющие символы (Backspace и т.д.)
            if (char.IsControl(c))
            {
                return;
            }

            // Разрешаем пробел и знаки препинания
            if (c == ' ' || c == '-' || c == '.' || c == ',')
            {
                return;
            }

            // Проверяем, является ли символ русской буквой
            if ((c >= 'А' && c <= 'я') || c == 'ё' || c == 'Ё')
            {
                return;
            }

            // Все остальное блокируем
            e.Handled = true;
        }

        // Автоматическое преобразование первой буквы в заглавную для ФИО
        public static void CapitalizeFirstLetter(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb != null && tb.Text.Length > 0)
            {
                string text = tb.Text;
                // Делаем первую букву заглавной
                tb.Text = char.ToUpper(text[0]) + text.Substring(1).ToLower();
                tb.SelectionStart = tb.Text.Length;
            }
        }

        // Проверка на пустое поле
        public static bool ValidateNotEmpty(TextBox textBox, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                MessageBox.Show("Поле \"" + fieldName + "\" обязательно для заполнения!",
                    "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox.Focus();
                return false;
            }
            return true;
        }

        // Проверка на пустой ComboBox
        public static bool ValidateComboBoxSelected(ComboBox comboBox, string fieldName)
        {
            if (comboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Необходимо выбрать значение в поле \"" + fieldName + "\"!",
                    "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBox.Focus();
                return false;
            }
            return true;
        }

        // Подтверждение закрытия окна
        public static bool ConfirmClose()
        {
            DialogResult result = MessageBox.Show("Вы действительно хотите закрыть это окно?",
                "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return result == DialogResult.Yes;
        }

        // Подтверждение удаления
        public static bool ConfirmDelete()
        {
            DialogResult result = MessageBox.Show("Вы действительно хотите удалить эту запись?",
                "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return result == DialogResult.Yes;
        }
    }
}