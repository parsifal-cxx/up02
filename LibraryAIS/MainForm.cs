using System;
using System.Windows.Forms;

namespace LibraryAIS
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Отображение информации о пользователе в статус-баре
            statusUser.Text = "Пользователь: " + CurrentUser.FullName;
            statusRole.Text = "Роль: " + CurrentUser.RoleName;

            ConfigureMenuByRole();
        }

        private void ConfigureMenuByRole()
        {
            // Сначала скрываем всё
            menuBooks.Visible = false;
            menuBorrowings.Visible = false;
            menuStudents.Visible = false;
            menuCatalogs.Visible = false;
            menuUsers.Visible = false;
            menuReports.Visible = false;

            // Скрываем подпункты справочников по умолчанию
            menuAuthors.Visible = false;
            menuCategories.Visible = false;
            menuPublishers.Visible = false;
            menuGroups.Visible = false;

            if (CurrentUser.IsAdmin)
            {
                // Администратор
                menuUsers.Visible = true;
                menuStudents.Visible = true;

                // Админ видит только справочник групп
                menuCatalogs.Visible = true;
                menuGroups.Visible = true;
            }
            else if (CurrentUser.IsLibrarian)
            {
                // Библиотекарь
                menuBorrowings.Visible = true;
                menuBooks.Visible = true;
                menuStudents.Visible = true;
                // Справочники ему не нужны
            }
            else if (CurrentUser.IsHeadLibrarian)
            {
                // Заведующий библиотекой
                menuBooks.Visible = true;
                menuReports.Visible = true;

                // Заведующий видит все справочники, КРОМЕ ГРУПП
                menuCatalogs.Visible = true;
                menuAuthors.Visible = true;
                menuCategories.Visible = true;
                menuPublishers.Visible = true;
            }
        }

        // === ОБРАБОТЧИКИ МЕНЮ ===

        private void menuBooks_Click(object sender, EventArgs e)
        {
            BooksForm booksForm = new BooksForm();
            booksForm.ShowDialog();
        }

        private void menuBorrowings_Click(object sender, EventArgs e)
        {
            BorrowingsForm borrowingsForm = new BorrowingsForm();
            borrowingsForm.ShowDialog();
        }

        private void menuStudents_Click(object sender, EventArgs e)
        {
            StudentsForm studentsForm = new StudentsForm();
            studentsForm.ShowDialog();
        }

        private void menuAuthors_Click(object sender, EventArgs e)
        {
            AuthorsForm authorsForm = new AuthorsForm();
            authorsForm.ShowDialog();
        }

        private void menuCategories_Click(object sender, EventArgs e)
        {
            CategoriesForm categoriesForm = new CategoriesForm();
            categoriesForm.ShowDialog();
        }

        private void menuPublishers_Click(object sender, EventArgs e)
        {
            PublishersForm publishersForm = new PublishersForm();
            publishersForm.ShowDialog();
        }

        private void menuGroups_Click(object sender, EventArgs e)
        {
            GroupsForm groupsForm = new GroupsForm();
            groupsForm.ShowDialog();
        }

        private void menuUsers_Click(object sender, EventArgs e)
        {
            UsersForm usersForm = new UsersForm();
            usersForm.ShowDialog();
        }

        private void menuReports_Click(object sender, EventArgs e)
        {
            ReportsForm reportsForm = new ReportsForm();
            reportsForm.ShowDialog();
        }

        private void menuExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!InputHelper.ConfirmClose())
            {
                e.Cancel = true;
            }
        }
    }
}