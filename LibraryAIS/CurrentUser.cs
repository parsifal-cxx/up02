using System;

namespace LibraryAIS
{
    // Класс для хранения информации о текущем пользователе
    public static class CurrentUser
    {
        public static int UserID { get; set; }
        public static string Login { get; set; }
        public static string Name { get; set; }
        public static string Surname { get; set; }
        public static string Patronymic { get; set; }
        public static int RoleID { get; set; }
        public static string RoleName { get; set; }

        // Очистка данных при выходе
        public static void Clear()
        {
            UserID = 0;
            Login = "";
            Name = "";
            Surname = "";
            Patronymic = "";
            RoleID = 0;
            RoleName = "";
        }

        // Полное ФИО
        public static string FullName
        {
            get
            {
                return Surname + " " + Name + " " + Patronymic;
            }
        }

        // Проверка роли - Администратор
        public static bool IsAdmin
        {
            get { return RoleID == 1; }
        }

        // Проверка роли - Библиотекарь
        public static bool IsLibrarian
        {
            get { return RoleID == 2; }
        }

        // Проверка роли - Заведующий библиотекой
        public static bool IsHeadLibrarian
        {
            get { return RoleID == 3; }
        }
    }
}