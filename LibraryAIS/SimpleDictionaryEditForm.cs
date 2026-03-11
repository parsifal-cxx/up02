using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace LibraryAIS
{
    // Перечисление типов справочников для универсальной формы
    public enum DictionaryType
    {
        Category,
        Publisher,
        Group
    }

    public partial class SimpleDictionaryEditForm : Form
    {
        private int _id = 0;
        private bool _isEdit = false;
        private DictionaryType _type;

        // Поля для динамической настройки под конкретный справочник
        private string _tableName;
        private string _idColumn;
        private string _entityName; // Для заголовков ("категорию", "группу" и т.д.)

        public SimpleDictionaryEditForm(DictionaryType type, int id = 0)
        {
            InitializeComponent();
            _type = type;
            _id = id;
            _isEdit = (id > 0);

            ConfigureForm();
        }

        private void ConfigureForm()
        {
            // Настраиваем форму в зависимости от переданного типа
            switch (_type)
            {
                case DictionaryType.Category:
                    _tableName = "categories";
                    _idColumn = "CategoryID";
                    _entityName = "категорию";
                    this.Text = "Справочник категорий";
                    break;
                case DictionaryType.Publisher:
                    _tableName = "publishers";
                    _idColumn = "PublisherID";
                    _entityName = "издательство";
                    this.Text = "Справочник издательств";
                    break;
                case DictionaryType.Group:
                    _tableName = "groups";
                    _idColumn = "GroupID";
                    _entityName = "группу";
                    this.Text = "Справочник групп";
                    break;
            }

            if (_isEdit)
            {
                lblTitle.Text = $"Редактировать {_entityName}";

                // Загружаем текущее значение из базы
                string query = $"SELECT Name FROM `{_tableName}` WHERE `{_idColumn}` = {_id}";
                object result = DatabaseHelper.ExecuteScalar(query, null);

                if (result != null)
                {
                    txtName.Text = result.ToString();
                }
            }
            else
            {
                lblTitle.Text = $"Добавить {_entityName}";
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Валидация на пустое поле
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Поле названия не может быть пустым!", "Ошибка валидации",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ПРОВЕРКА НА ДУБЛИКАТЫ
            // Используем универсальный метод из DatabaseHelper
            if (DatabaseHelper.IsDuplicate(_tableName, "Name", txtName.Text, _idColumn, _id))
            {
                MessageBox.Show("Такая запись уже существует в базе данных!", "Ошибка дублирования",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                string query;
                if (_isEdit)
                {
                    query = $"UPDATE `{_tableName}` SET Name = @name WHERE `{_idColumn}` = @id";
                }
                else
                {
                    query = $"INSERT INTO `{_tableName}` (Name) VALUES (@name)";
                }

                MySqlParameter[] parameters = {
                    new MySqlParameter("@name", txtName.Text.Trim()),
                    new MySqlParameter("@id", _id)
                };

                DatabaseHelper.ExecuteNonQuery(query, parameters);

                MessageBox.Show("Данные успешно сохранены!", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении: " + ex.Message, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}