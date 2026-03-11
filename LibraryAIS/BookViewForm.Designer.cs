namespace LibraryAIS
{
    partial class BookViewForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BookViewForm));
            this.panelTop = new System.Windows.Forms.Panel();
            this.lblHeadTitle = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.picBook = new System.Windows.Forms.PictureBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblAuthor = new System.Windows.Forms.Label();
            this.lblCategoryLabel = new System.Windows.Forms.Label();
            this.lblCategory = new System.Windows.Forms.Label();
            this.lblPublisherLabel = new System.Windows.Forms.Label();
            this.lblPublisher = new System.Windows.Forms.Label();
            this.lblYearLabel = new System.Windows.Forms.Label();
            this.lblYear = new System.Windows.Forms.Label();
            this.lblCountLabel = new System.Windows.Forms.Label();
            this.lblCount = new System.Windows.Forms.Label();
            this.lblAvailableLabel = new System.Windows.Forms.Label();
            this.lblAvailable = new System.Windows.Forms.Label();
            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBook)).BeginInit();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(130)))), ((int)(((byte)(180)))));
            this.panelTop.Controls.Add(this.lblHeadTitle);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(600, 50);
            this.panelTop.TabIndex = 0;
            // 
            // lblHeadTitle
            // 
            this.lblHeadTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblHeadTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblHeadTitle.ForeColor = System.Drawing.Color.White;
            this.lblHeadTitle.Location = new System.Drawing.Point(0, 0);
            this.lblHeadTitle.Name = "lblHeadTitle";
            this.lblHeadTitle.Size = new System.Drawing.Size(600, 50);
            this.lblHeadTitle.TabIndex = 0;
            this.lblHeadTitle.Text = "Информация о книге";
            this.lblHeadTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Gray;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(460, 360);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(120, 35);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Закрыть";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // picBook
            // 
            this.picBook.BackColor = System.Drawing.Color.WhiteSmoke;
            this.picBook.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picBook.Location = new System.Drawing.Point(20, 70);
            this.picBook.Name = "picBook";
            this.picBook.Size = new System.Drawing.Size(200, 280);
            this.picBook.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picBook.TabIndex = 2;
            this.picBook.TabStop = false;
            // 
            // lblTitle
            // 
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(240, 70);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(340, 60);
            this.lblTitle.TabIndex = 3;
            this.lblTitle.Text = "Название книги";
            // 
            // lblAuthor
            // 
            this.lblAuthor.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic);
            this.lblAuthor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblAuthor.Location = new System.Drawing.Point(240, 130);
            this.lblAuthor.Name = "lblAuthor";
            this.lblAuthor.Size = new System.Drawing.Size(340, 25);
            this.lblAuthor.TabIndex = 4;
            this.lblAuthor.Text = "Автор книги";
            // 
            // lblCategoryLabel
            // 
            this.lblCategoryLabel.AutoSize = true;
            this.lblCategoryLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblCategoryLabel.ForeColor = System.Drawing.Color.Gray;
            this.lblCategoryLabel.Location = new System.Drawing.Point(240, 170);
            this.lblCategoryLabel.Name = "lblCategoryLabel";
            this.lblCategoryLabel.Size = new System.Drawing.Size(85, 19);
            this.lblCategoryLabel.TabIndex = 5;
            this.lblCategoryLabel.Text = "Категория:";
            // 
            // lblCategory
            // 
            this.lblCategory.AutoSize = true;
            this.lblCategory.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblCategory.Location = new System.Drawing.Point(360, 170);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new System.Drawing.Size(70, 19);
            this.lblCategory.TabIndex = 6;
            this.lblCategory.Text = "Значение";
            // 
            // lblPublisherLabel
            // 
            this.lblPublisherLabel.AutoSize = true;
            this.lblPublisherLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblPublisherLabel.ForeColor = System.Drawing.Color.Gray;
            this.lblPublisherLabel.Location = new System.Drawing.Point(240, 200);
            this.lblPublisherLabel.Name = "lblPublisherLabel";
            this.lblPublisherLabel.Size = new System.Drawing.Size(108, 19);
            this.lblPublisherLabel.TabIndex = 7;
            this.lblPublisherLabel.Text = "Издательство:";
            // 
            // lblPublisher
            // 
            this.lblPublisher.AutoSize = true;
            this.lblPublisher.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblPublisher.Location = new System.Drawing.Point(360, 200);
            this.lblPublisher.Name = "lblPublisher";
            this.lblPublisher.Size = new System.Drawing.Size(70, 19);
            this.lblPublisher.TabIndex = 8;
            this.lblPublisher.Text = "Значение";
            // 
            // lblYearLabel
            // 
            this.lblYearLabel.AutoSize = true;
            this.lblYearLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblYearLabel.ForeColor = System.Drawing.Color.Gray;
            this.lblYearLabel.Location = new System.Drawing.Point(240, 230);
            this.lblYearLabel.Name = "lblYearLabel";
            this.lblYearLabel.Size = new System.Drawing.Size(100, 19);
            this.lblYearLabel.TabIndex = 9;
            this.lblYearLabel.Text = "Год издания:";
            // 
            // lblYear
            // 
            this.lblYear.AutoSize = true;
            this.lblYear.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblYear.Location = new System.Drawing.Point(360, 230);
            this.lblYear.Name = "lblYear";
            this.lblYear.Size = new System.Drawing.Size(41, 19);
            this.lblYear.TabIndex = 10;
            this.lblYear.Text = "2024";
            // 
            // lblCountLabel
            // 
            this.lblCountLabel.AutoSize = true;
            this.lblCountLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblCountLabel.ForeColor = System.Drawing.Color.Gray;
            this.lblCountLabel.Location = new System.Drawing.Point(240, 270);
            this.lblCountLabel.Name = "lblCountLabel";
            this.lblCountLabel.Size = new System.Drawing.Size(100, 19);
            this.lblCountLabel.TabIndex = 11;
            this.lblCountLabel.Text = "Всего в базе:";
            // 
            // lblCount
            // 
            this.lblCount.AutoSize = true;
            this.lblCount.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblCount.ForeColor = System.Drawing.Color.Black;
            this.lblCount.Location = new System.Drawing.Point(360, 270);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(25, 19);
            this.lblCount.TabIndex = 12;
            this.lblCount.Text = "10";
            // 
            // lblAvailableLabel
            // 
            this.lblAvailableLabel.AutoSize = true;
            this.lblAvailableLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblAvailableLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.lblAvailableLabel.Location = new System.Drawing.Point(240, 300);
            this.lblAvailableLabel.Name = "lblAvailableLabel";
            this.lblAvailableLabel.Size = new System.Drawing.Size(81, 19);
            this.lblAvailableLabel.TabIndex = 13;
            this.lblAvailableLabel.Text = "Доступно:";
            // 
            // lblAvailable
            // 
            this.lblAvailable.AutoSize = true;
            this.lblAvailable.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblAvailable.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.lblAvailable.Location = new System.Drawing.Point(360, 295);
            this.lblAvailable.Name = "lblAvailable";
            this.lblAvailable.Size = new System.Drawing.Size(34, 25);
            this.lblAvailable.TabIndex = 14;
            this.lblAvailable.Text = "10";
            // 
            // BookViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(600, 410);
            this.Controls.Add(this.lblAvailable);
            this.Controls.Add(this.lblAvailableLabel);
            this.Controls.Add(this.lblCount);
            this.Controls.Add(this.lblCountLabel);
            this.Controls.Add(this.lblYear);
            this.Controls.Add(this.lblYearLabel);
            this.Controls.Add(this.lblPublisher);
            this.Controls.Add(this.lblPublisherLabel);
            this.Controls.Add(this.lblCategory);
            this.Controls.Add(this.lblCategoryLabel);
            this.Controls.Add(this.lblAuthor);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.picBook);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.panelTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BookViewForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Просмотр книги";
            this.Load += new System.EventHandler(this.BookViewForm_Load);
            this.panelTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picBook)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblHeadTitle;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.PictureBox picBook;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblAuthor;
        private System.Windows.Forms.Label lblCategoryLabel;
        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.Label lblPublisherLabel;
        private System.Windows.Forms.Label lblPublisher;
        private System.Windows.Forms.Label lblYearLabel;
        private System.Windows.Forms.Label lblYear;
        private System.Windows.Forms.Label lblCountLabel;
        private System.Windows.Forms.Label lblCount;
        private System.Windows.Forms.Label lblAvailableLabel;
        private System.Windows.Forms.Label lblAvailable;
    }
}