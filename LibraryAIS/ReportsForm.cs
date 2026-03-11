using System;
using System.Data;
using System.Windows.Forms;
// Библиотека для работы с Excel (должна быть добавлена в References!)
using Excel = Microsoft.Office.Interop.Excel;

namespace LibraryAIS
{
    public partial class ReportsForm : Form
    {
        public ReportsForm()
        {
            InitializeComponent();
        }

        private void ReportsForm_Load(object sender, EventArgs e)
        {
            cmbReportType.Items.Clear();
            cmbReportType.Items.Add("Отчёт о закупке книг");
            cmbReportType.Items.Add("Отчёт о списании книг");
            cmbReportType.Items.Add("Отчёт о заимствованиях");
            cmbReportType.SelectedIndex = 0;

            dtpDateFrom.Value = DateTime.Now.AddMonths(-1);
            dtpDateTo.Value = DateTime.Now;
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (cmbReportType.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите тип отчёта!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string dateFrom = dtpDateFrom.Value.ToString("yyyy-MM-dd");
            string dateTo = dtpDateTo.Value.ToString("yyyy-MM-dd");

            DataTable dt = null;

            switch (cmbReportType.SelectedIndex)
            {
                case 0: dt = GeneratePurchaseReport(dateFrom, dateTo); break;
                case 1: dt = GenerateWriteOffReport(dateFrom, dateTo); break;
                case 2: dt = GenerateBorrowingsReport(dateFrom, dateTo); break;
            }

            if (dt != null)
            {
                dgvReport.DataSource = dt;
                MessageBox.Show("Отчёт сформирован!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // === НОВЫЙ МЕТОД ЭКСПОРТА ===
        private void btnExport_Click(object sender, EventArgs e)
        {
            if (dgvReport.Rows.Count == 0)
            {
                MessageBox.Show("Нет данных для экспорта! Сначала сформируйте отчёт.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Диалог сохранения файла
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Excel файлы (*.xlsx)|*.xlsx|Все файлы (*.*)|*.*";
            saveDialog.Title = "Сохранение отчёта";
            saveDialog.FileName = "Отчет_" + DateTime.Now.ToString("dd_MM_yyyy");

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                Excel.Application excelApp = null;
                Excel.Workbook workbook = null;
                Excel.Worksheet worksheet = null;

                try
                {
                    // Создаем приложение Excel
                    excelApp = new Excel.Application();
                    excelApp.Visible = false; // Работаем в фоне

                    // Создаем книгу
                    workbook = excelApp.Workbooks.Add();
                    worksheet = (Excel.Worksheet)workbook.Sheets[1];

                    // 1. Записываем заголовки колонок
                    for (int i = 0; i < dgvReport.Columns.Count; i++)
                    {
                        worksheet.Cells[1, i + 1] = dgvReport.Columns[i].HeaderText;
                        // Делаем заголовки жирными
                        ((Excel.Range)worksheet.Cells[1, i + 1]).Font.Bold = true;
                    }

                    // 2. Записываем данные
                    for (int i = 0; i < dgvReport.Rows.Count; i++)
                    {
                        for (int j = 0; j < dgvReport.Columns.Count; j++)
                        {
                            // i + 2, потому что 1-я строка занята заголовками
                            // j + 1, потому что в Excel нумерация с 1
                            worksheet.Cells[i + 2, j + 1] = dgvReport.Rows[i].Cells[j].Value?.ToString();
                        }
                    }

                    // Автоматическая ширина колонок
                    worksheet.Columns.AutoFit();

                    // Сохраняем файл
                    workbook.SaveAs(saveDialog.FileName);
                    MessageBox.Show("Файл успешно сохранен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при экспорте: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    // Освобождаем ресурсы
                    if (workbook != null)
                    {
                        workbook.Close(false);
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                    }
                    if (excelApp != null)
                    {
                        excelApp.Quit();
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
                    }
                    workbook = null;
                    excelApp = null;
                    GC.Collect(); // Принудительная сборка мусора
                }
            }
        }

        private DataTable GeneratePurchaseReport(string dateFrom, string dateTo)
        {
            string query = @"SELECT 
                            p.Date as 'Дата поступления',
                            b.Title as 'Название книги', 
                            CONCAT(a.Surname, ' ', a.Name) as 'Автор',
                            pub.Name as 'Издательство',
                            p.Count as 'Количество',
                            b.Year as 'Год издания'
                            FROM purchases p
                            JOIN books b ON p.BookID = b.BookID
                            LEFT JOIN authors a ON b.AuthorOne = a.AuthorID
                            LEFT JOIN publishers pub ON b.Publisher = pub.PublisherID
                            WHERE p.Date BETWEEN '" + dateFrom + "' AND '" + dateTo + @"'
                            ORDER BY p.Date DESC";

            return DatabaseHelper.ExecuteQuery(query);
        }

        private DataTable GenerateWriteOffReport(string dateFrom, string dateTo)
        {
            string query = @"SELECT 
                            w.Date as 'Дата списания',
                            b.Title as 'Название книги', 
                            w.Count as 'Списано (шт)',
                            w.Reason as 'Причина списания',
                            b.AvailableNow as 'Остаток на полках'
                            FROM writeoffs w
                            JOIN books b ON w.BookID = b.BookID
                            WHERE w.Date BETWEEN '" + dateFrom + "' AND '" + dateTo + @"'
                            ORDER BY w.Date DESC";

            return DatabaseHelper.ExecuteQuery(query);
        }

        private DataTable GenerateBorrowingsReport(string dateFrom, string dateTo)
        {
            string query = @"SELECT 
                            CONCAT(s.Surname, ' ', s.Name) as 'Студент',
                            b.Title as 'Книга',
                            br.BorrowDate as 'Дата заимствования',
                            br.ReturnDate as 'Дата возврата',
                            br.Count as 'Количество',
                            g.Name as 'Группа'
                            FROM borrowings br
                            LEFT JOIN students s ON br.StudentID = s.StudentID
                            LEFT JOIN books b ON br.BookID = b.BookID
                            LEFT JOIN `groups` g ON s.`Group` = g.GroupID
                            WHERE STR_TO_DATE(br.BorrowDate, '%d.%m.%Y') >= '" + dateFrom + @"'
                            AND STR_TO_DATE(br.BorrowDate, '%d.%m.%Y') <= '" + dateTo + @"'
                            ORDER BY STR_TO_DATE(br.BorrowDate, '%d.%m.%Y') DESC";

            return DatabaseHelper.ExecuteQuery(query);
        }

        private void btnClose_Click(object sender, EventArgs e) { this.Close(); }
    }
}