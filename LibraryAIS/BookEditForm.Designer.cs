namespace LibraryAIS
{
    partial class BookEditForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BookEditForm));
            this.panelTop = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblBookTitle = new System.Windows.Forms.Label();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.lblAuthorOne = new System.Windows.Forms.Label();
            this.cmbAuthorOne = new System.Windows.Forms.ComboBox();
            this.lblAuthorTwo = new System.Windows.Forms.Label();
            this.cmbAuthorTwo = new System.Windows.Forms.ComboBox();
            this.lblPublisher = new System.Windows.Forms.Label();
            this.cmbPublisher = new System.Windows.Forms.ComboBox();
            this.lblYear = new System.Windows.Forms.Label();
            this.numYear = new System.Windows.Forms.NumericUpDown();
            this.lblCount = new System.Windows.Forms.Label();
            this.numCount = new System.Windows.Forms.NumericUpDown();
            this.lblAvailable = new System.Windows.Forms.Label();
            this.numAvailable = new System.Windows.Forms.NumericUpDown();
            this.lblCategory = new System.Windows.Forms.Label();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.lblImage = new System.Windows.Forms.Label();
            this.picBook = new System.Windows.Forms.PictureBox();
            this.btnSelectImage = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numYear)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAvailable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBook)).BeginInit();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(130)))), ((int)(((byte)(180)))));
            this.panelTop.Controls.Add(this.lblTitle);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(500, 50);
            this.panelTop.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(500, 50);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Добавление книги";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblBookTitle
            // 
            this.lblBookTitle.AutoSize = true;
            this.lblBookTitle.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblBookTitle.Location = new System.Drawing.Point(20, 65);
            this.lblBookTitle.Name = "lblBookTitle";
            this.lblBookTitle.Size = new System.Drawing.Size(72, 19);
            this.lblBookTitle.TabIndex = 1;
            this.lblBookTitle.Text = "Название:";
            // 
            // txtTitle
            // 
            this.txtTitle.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtTitle.Location = new System.Drawing.Point(20, 87);
            this.txtTitle.MaxLength = 100;
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(300, 25);
            this.txtTitle.TabIndex = 2;
            // 
            // lblAuthorOne
            // 
            this.lblAuthorOne.AutoSize = true;
            this.lblAuthorOne.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblAuthorOne.Location = new System.Drawing.Point(20, 120);
            this.lblAuthorOne.Name = "lblAuthorOne";
            this.lblAuthorOne.Size = new System.Drawing.Size(50, 19);
            this.lblAuthorOne.TabIndex = 3;
            this.lblAuthorOne.Text = "Автор:";
            // 
            // cmbAuthorOne
            // 
            this.cmbAuthorOne.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAuthorOne.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbAuthorOne.Location = new System.Drawing.Point(20, 142);
            this.cmbAuthorOne.Name = "cmbAuthorOne";
            this.cmbAuthorOne.Size = new System.Drawing.Size(300, 25);
            this.cmbAuthorOne.TabIndex = 4;
            // 
            // lblAuthorTwo
            // 
            this.lblAuthorTwo.AutoSize = true;
            this.lblAuthorTwo.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblAuthorTwo.Location = new System.Drawing.Point(20, 175);
            this.lblAuthorTwo.Name = "lblAuthorTwo";
            this.lblAuthorTwo.Size = new System.Drawing.Size(65, 19);
            this.lblAuthorTwo.TabIndex = 5;
            this.lblAuthorTwo.Text = "Соавтор:";
            // 
            // cmbAuthorTwo
            // 
            this.cmbAuthorTwo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAuthorTwo.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbAuthorTwo.Location = new System.Drawing.Point(20, 197);
            this.cmbAuthorTwo.Name = "cmbAuthorTwo";
            this.cmbAuthorTwo.Size = new System.Drawing.Size(300, 25);
            this.cmbAuthorTwo.TabIndex = 6;
            // 
            // lblPublisher
            // 
            this.lblPublisher.AutoSize = true;
            this.lblPublisher.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblPublisher.Location = new System.Drawing.Point(20, 230);
            this.lblPublisher.Name = "lblPublisher";
            this.lblPublisher.Size = new System.Drawing.Size(97, 19);
            this.lblPublisher.TabIndex = 7;
            this.lblPublisher.Text = "Издательство:";
            // 
            // cmbPublisher
            // 
            this.cmbPublisher.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPublisher.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbPublisher.Location = new System.Drawing.Point(20, 252);
            this.cmbPublisher.Name = "cmbPublisher";
            this.cmbPublisher.Size = new System.Drawing.Size(300, 25);
            this.cmbPublisher.TabIndex = 8;
            // 
            // lblYear
            // 
            this.lblYear.AutoSize = true;
            this.lblYear.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblYear.Location = new System.Drawing.Point(20, 285);
            this.lblYear.Name = "lblYear";
            this.lblYear.Size = new System.Drawing.Size(91, 19);
            this.lblYear.TabIndex = 9;
            this.lblYear.Text = "Год издания:";
            // 
            // numYear
            // 
            this.numYear.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.numYear.Location = new System.Drawing.Point(20, 307);
            this.numYear.Maximum = new decimal(new int[] {
            2030,
            0,
            0,
            0});
            this.numYear.Minimum = new decimal(new int[] {
            1900,
            0,
            0,
            0});
            this.numYear.Name = "numYear";
            this.numYear.Size = new System.Drawing.Size(100, 25);
            this.numYear.TabIndex = 10;
            this.numYear.Value = new decimal(new int[] {
            2024,
            0,
            0,
            0});
            // 
            // lblCount
            // 
            this.lblCount.AutoSize = true;
            this.lblCount.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblCount.Location = new System.Drawing.Point(130, 285);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(85, 19);
            this.lblCount.TabIndex = 11;
            this.lblCount.Text = "Количество:";
            // 
            // numCount
            // 
            this.numCount.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.numCount.Location = new System.Drawing.Point(130, 307);
            this.numCount.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numCount.Name = "numCount";
            this.numCount.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.numCount.Size = new System.Drawing.Size(80, 25);
            this.numCount.TabIndex = 12;
            this.numCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblAvailable
            // 
            this.lblAvailable.AutoSize = true;
            this.lblAvailable.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblAvailable.Location = new System.Drawing.Point(220, 285);
            this.lblAvailable.Name = "lblAvailable";
            this.lblAvailable.Size = new System.Drawing.Size(73, 19);
            this.lblAvailable.TabIndex = 13;
            this.lblAvailable.Text = "Доступно:";
            // 
            // numAvailable
            // 
            this.numAvailable.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.numAvailable.Location = new System.Drawing.Point(220, 307);
            this.numAvailable.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numAvailable.Name = "numAvailable";
            this.numAvailable.Size = new System.Drawing.Size(80, 25);
            this.numAvailable.TabIndex = 14;
            this.numAvailable.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblCategory
            // 
            this.lblCategory.AutoSize = true;
            this.lblCategory.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblCategory.Location = new System.Drawing.Point(20, 340);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new System.Drawing.Size(76, 19);
            this.lblCategory.TabIndex = 15;
            this.lblCategory.Text = "Категория:";
            // 
            // cmbCategory
            // 
            this.cmbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategory.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbCategory.Location = new System.Drawing.Point(20, 362);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(300, 25);
            this.cmbCategory.TabIndex = 16;
            // 
            // lblImage
            // 
            this.lblImage.AutoSize = true;
            this.lblImage.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblImage.Location = new System.Drawing.Point(350, 65);
            this.lblImage.Name = "lblImage";
            this.lblImage.Size = new System.Drawing.Size(99, 19);
            this.lblImage.TabIndex = 17;
            this.lblImage.Text = "Изображение:";
            // 
            // picBook
            // 
            this.picBook.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.picBook.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picBook.Location = new System.Drawing.Point(350, 87);
            this.picBook.Name = "picBook";
            this.picBook.Size = new System.Drawing.Size(130, 180);
            this.picBook.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picBook.TabIndex = 18;
            this.picBook.TabStop = false;
            // 
            // btnSelectImage
            // 
            this.btnSelectImage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(89)))), ((int)(((byte)(182)))));
            this.btnSelectImage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectImage.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnSelectImage.ForeColor = System.Drawing.Color.White;
            this.btnSelectImage.Location = new System.Drawing.Point(350, 275);
            this.btnSelectImage.Name = "btnSelectImage";
            this.btnSelectImage.Size = new System.Drawing.Size(130, 28);
            this.btnSelectImage.TabIndex = 19;
            this.btnSelectImage.Text = "Выбрать...";
            this.btnSelectImage.UseVisualStyleBackColor = false;
            this.btnSelectImage.Click += new System.EventHandler(this.btnSelectImage_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(20, 410);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(150, 35);
            this.btnSave.TabIndex = 20;
            this.btnSave.Text = "Сохранить";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Gray;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(180, 410);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(150, 35);
            this.btnCancel.TabIndex = 21;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // BookEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(500, 460);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnSelectImage);
            this.Controls.Add(this.picBook);
            this.Controls.Add(this.lblImage);
            this.Controls.Add(this.cmbCategory);
            this.Controls.Add(this.lblCategory);
            this.Controls.Add(this.numAvailable);
            this.Controls.Add(this.lblAvailable);
            this.Controls.Add(this.numCount);
            this.Controls.Add(this.lblCount);
            this.Controls.Add(this.numYear);
            this.Controls.Add(this.lblYear);
            this.Controls.Add(this.cmbPublisher);
            this.Controls.Add(this.lblPublisher);
            this.Controls.Add(this.cmbAuthorTwo);
            this.Controls.Add(this.lblAuthorTwo);
            this.Controls.Add(this.cmbAuthorOne);
            this.Controls.Add(this.lblAuthorOne);
            this.Controls.Add(this.txtTitle);
            this.Controls.Add(this.lblBookTitle);
            this.Controls.Add(this.panelTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BookEditForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Книга";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BookEditForm_FormClosing);
            this.Load += new System.EventHandler(this.BookEditForm_Load);
            this.panelTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numYear)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAvailable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBook)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblBookTitle;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Label lblAuthorOne;
        private System.Windows.Forms.ComboBox cmbAuthorOne;
        private System.Windows.Forms.Label lblAuthorTwo;
        private System.Windows.Forms.ComboBox cmbAuthorTwo;
        private System.Windows.Forms.Label lblPublisher;
        private System.Windows.Forms.ComboBox cmbPublisher;
        private System.Windows.Forms.Label lblYear;
        private System.Windows.Forms.NumericUpDown numYear;
        private System.Windows.Forms.Label lblCount;
        private System.Windows.Forms.NumericUpDown numCount;
        private System.Windows.Forms.Label lblAvailable;
        private System.Windows.Forms.NumericUpDown numAvailable;
        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.Label lblImage;
        private System.Windows.Forms.PictureBox picBook;
        private System.Windows.Forms.Button btnSelectImage;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
    }
}