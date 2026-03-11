using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace LibraryAIS
{
    public static class DatabaseHelper
    {
        // Строка подключения. 
        // TreatTinyAsBoolean=false важен для корректной работы с числами (Count, AvailableNow)
        private static string connectionString = "Server=10.207.106.12;Database=db82;Uid=user82;Pwd=qt33;TreatTinyAsBoolean=false;";

        // Получение подключения
        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }

        // Тест подключения при запуске программы
        public static bool TestConnection()
        {
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    conn.Open();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Ошибка подключения к базе данных:\n" + ex.Message +
                    "\n\nПроверьте настройки подключения в DatabaseHelper.cs",
                    "Ошибка подключения",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return false;
            }
        }

        // Выполнение SELECT запроса (без параметров)
        public static DataTable ExecuteQuery(string query)
        {
            DataTable dt = new DataTable();
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    conn.Open();
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при выполнении запроса: " + ex.Message,
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return dt;
        }

        // Выполнение SELECT запроса с параметрами
        public static DataTable ExecuteQuery(string query, MySqlParameter[] parameters)
        {
            DataTable dt = new DataTable();
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при выполнении запроса: " + ex.Message,
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return dt;
        }

        // Выполнение INSERT, UPDATE, DELETE (без параметров)
        public static int ExecuteNonQuery(string query)
        {
            int result = 0;
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    result = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при выполнении запроса: " + ex.Message,
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return result;
        }

        // Выполнение INSERT, UPDATE, DELETE с параметрами
        public static int ExecuteNonQuery(string query, MySqlParameter[] parameters)
        {
            int result = 0;
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    result = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при выполнении запроса: " + ex.Message,
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return result;
        }

        // Получить скалярное значение (одно число или строку)
        public static object ExecuteScalar(string query, MySqlParameter[] parameters)
        {
            object result = null;
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    result = cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при выполнении запроса: " + ex.Message,
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return result;
        }

        // === НОВЫЕ МЕТОДЫ ДЛЯ ПРОВЕРКИ ДУБЛИКАТОВ ===

        // Проверка дублей для простых справочников (одно поле Name)
        public static bool IsDuplicate(string tableName, string columnName, string value, string idColumnName, int excludeId)
        {
            string query = $"SELECT COUNT(*) FROM `{tableName}` WHERE `{columnName}` = @value AND `{idColumnName}` != @id";

            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@value", value.Trim()),
                new MySqlParameter("@id", excludeId)
            };

            object result = ExecuteScalar(query, parameters);
            int count = result != null ? Convert.ToInt32(result) : 0;

            return count > 0;
        }

        // Универсальная проверка дублей для сложных случаев (составной ключ)
        // Принимает готовый SQL-запрос с COUNT(*)
        public static bool IsDuplicateComposite(string query, MySqlParameter[] parameters)
        {
            object result = ExecuteScalar(query, parameters);
            int count = result != null ? Convert.ToInt32(result) : 0;

            return count > 0;
        }
    }
}