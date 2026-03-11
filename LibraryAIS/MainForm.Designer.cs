namespace LibraryAIS
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.menuBooks = new System.Windows.Forms.ToolStripMenuItem();
            this.menuBorrowings = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStudents = new System.Windows.Forms.ToolStripMenuItem();
            this.menuCatalogs = new System.Windows.Forms.ToolStripMenuItem();
            this.menuAuthors = new System.Windows.Forms.ToolStripMenuItem();
            this.menuCategories = new System.Windows.Forms.ToolStripMenuItem();
            this.menuPublishers = new System.Windows.Forms.ToolStripMenuItem();
            this.menuGroups = new System.Windows.Forms.ToolStripMenuItem();
            this.menuUsers = new System.Windows.Forms.ToolStripMenuItem();
            this.menuReports = new System.Windows.Forms.ToolStripMenuItem();
            this.menuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusUser = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusRole = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblWelcome = new System.Windows.Forms.Label();
            this.panelContent = new System.Windows.Forms.Panel();
            this.menuStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuBooks,
            this.menuBorrowings,
            this.menuStudents,
            this.menuCatalogs,
            this.menuUsers,
            this.menuReports,
            this.menuExit});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1000, 27);
            this.menuStrip.TabIndex = 0;
            // 
            // menuBooks
            // 
            this.menuBooks.Name = "menuBooks";
            this.menuBooks.Size = new System.Drawing.Size(58, 23);
            this.menuBooks.Text = "Книги";
            this.menuBooks.Click += new System.EventHandler(this.menuBooks_Click);
            // 
            // menuBorrowings
            // 
            this.menuBorrowings.Name = "menuBorrowings";
            this.menuBorrowings.Size = new System.Drawing.Size(118, 23);
            this.menuBorrowings.Text = "Заимствования";
            this.menuBorrowings.Click += new System.EventHandler(this.menuBorrowings_Click);
            // 
            // menuStudents
            // 
            this.menuStudents.Name = "menuStudents";
            this.menuStudents.Size = new System.Drawing.Size(82, 23);
            this.menuStudents.Text = "Студенты";
            this.menuStudents.Click += new System.EventHandler(this.menuStudents_Click);
            // 
            // menuCatalogs
            // 
            this.menuCatalogs.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuAuthors,
            this.menuCategories,
            this.menuPublishers,
            this.menuGroups});
            this.menuCatalogs.Name = "menuCatalogs";
            this.menuCatalogs.Size = new System.Drawing.Size(107, 23);
            this.menuCatalogs.Text = "Справочники";
            // 
            // menuAuthors
            // 
            this.menuAuthors.Name = "menuAuthors";
            this.menuAuthors.Size = new System.Drawing.Size(162, 24);
            this.menuAuthors.Text = "Авторы";
            this.menuAuthors.Click += new System.EventHandler(this.menuAuthors_Click);
            // 
            // menuCategories
            // 
            this.menuCategories.Name = "menuCategories";
            this.menuCategories.Size = new System.Drawing.Size(162, 24);
            this.menuCategories.Text = "Категории";
            this.menuCategories.Click += new System.EventHandler(this.menuCategories_Click);
            // 
            // menuPublishers
            // 
            this.menuPublishers.Name = "menuPublishers";
            this.menuPublishers.Size = new System.Drawing.Size(162, 24);
            this.menuPublishers.Text = "Издательства";
            this.menuPublishers.Click += new System.EventHandler(this.menuPublishers_Click);
            // 
            // menuGroups
            // 
            this.menuGroups.Name = "menuGroups";
            this.menuGroups.Size = new System.Drawing.Size(162, 24);
            this.menuGroups.Text = "Группы";
            this.menuGroups.Click += new System.EventHandler(this.menuGroups_Click);
            // 
            // menuUsers
            // 
            this.menuUsers.Name = "menuUsers";
            this.menuUsers.Size = new System.Drawing.Size(109, 23);
            this.menuUsers.Text = "Пользователи";
            this.menuUsers.Click += new System.EventHandler(this.menuUsers_Click);
            // 
            // menuReports
            // 
            this.menuReports.Name = "menuReports";
            this.menuReports.Size = new System.Drawing.Size(69, 23);
            this.menuReports.Text = "Отчёты";
            this.menuReports.Click += new System.EventHandler(this.menuReports_Click);
            // 
            // menuExit
            // 
            this.menuExit.Name = "menuExit";
            this.menuExit.Size = new System.Drawing.Size(61, 23);
            this.menuExit.Text = "Выход";
            this.menuExit.Click += new System.EventHandler(this.menuExit_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusUser,
            this.statusRole});
            this.statusStrip.Location = new System.Drawing.Point(0, 578);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1000, 22);
            this.statusStrip.TabIndex = 1;
            // 
            // statusUser
            // 
            this.statusUser.Name = "statusUser";
            this.statusUser.Size = new System.Drawing.Size(87, 17);
            this.statusUser.Text = "Пользователь:";
            // 
            // statusRole
            // 
            this.statusRole.Name = "statusRole";
            this.statusRole.Size = new System.Drawing.Size(37, 17);
            this.statusRole.Text = "Роль:";
            // 
            // lblWelcome
            // 
            this.lblWelcome.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblWelcome.Font = new System.Drawing.Font("Segoe UI", 18F);
            this.lblWelcome.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(130)))), ((int)(((byte)(180)))));
            this.lblWelcome.Location = new System.Drawing.Point(0, 27);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(1000, 551);
            this.lblWelcome.TabIndex = 2;
            this.lblWelcome.Text = "Добро пожаловать в АИС Библиотека!\r\n\r\nВыберите нужный раздел в меню.";
            this.lblWelcome.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelContent
            // 
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(0, 27);
            this.panelContent.Name = "panelContent";
            this.panelContent.Size = new System.Drawing.Size(1000, 551);
            this.panelContent.TabIndex = 3;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1000, 600);
            this.Controls.Add(this.lblWelcome);
            this.Controls.Add(this.panelContent);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.MinimumSize = new System.Drawing.Size(800, 500);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "АИС Библиотека";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem menuBooks;
        private System.Windows.Forms.ToolStripMenuItem menuBorrowings;
        private System.Windows.Forms.ToolStripMenuItem menuStudents;
        private System.Windows.Forms.ToolStripMenuItem menuCatalogs;
        private System.Windows.Forms.ToolStripMenuItem menuAuthors;
        private System.Windows.Forms.ToolStripMenuItem menuCategories;
        private System.Windows.Forms.ToolStripMenuItem menuPublishers;
        private System.Windows.Forms.ToolStripMenuItem menuGroups; // Новое поле
        private System.Windows.Forms.ToolStripMenuItem menuUsers;
        private System.Windows.Forms.ToolStripMenuItem menuReports;
        private System.Windows.Forms.ToolStripMenuItem menuExit;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel statusUser;
        private System.Windows.Forms.ToolStripStatusLabel statusRole;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.Panel panelContent;
    }
}