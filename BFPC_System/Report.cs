using BPFC_System;
using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Windows.Forms;
using System.Linq;
using System.Globalization;
using DocumentFormat.OpenXml;
using System.Reflection;
using OfficeOpenXml;
using System.Drawing;


namespace BPFC_System
{
    public partial class frmReport : DevExpress.XtraEditors.XtraForm
    {
        private readonly string connectionString;
        private readonly BpfcDbContext dbContext;
        private List<ToolStripMenuItem> plantNameItems;
        private string selectedPlantName;
        private readonly DatabaseManager dbManager;
        private DataTable dailyDataTable = new DataTable();
        private DataTable plantDataTable = new DataTable();
        private DataTable monthlyDataTable = new DataTable();
        private Dictionary<ToolStripMenuItem, DataTable> menuItemDataTableMapping;
        private ToolStripMenuItem monthlyReportItem = new ToolStripMenuItem("Monthly Report");
        private ToolStripMenuItem dailyReportItem = new ToolStripMenuItem("Daily Report");
        private Dictionary<string, DataTable> itemNameDataTableMapping = new Dictionary<string, DataTable>();
        private Dictionary<string, DataTable> plantDataTableMapping = new Dictionary<string, DataTable>();

        public frmReport()
        {
            InitializeComponent();
            connectionString = ConfigHelper.GetConnectionString("strCon");
            dbManager = new DatabaseManager(connectionString);
            dbContext = new BpfcDbContext(connectionString);
            dgvReport.ReadOnly = true;

            LoadPlantNames();
            LoadMenu();

            InitializeDataGridView();
            monthlyDataTable = new DataTable();
            dailyDataTable = new DataTable();
            plantDataTable = new DataTable();

            menuItemDataTableMapping = new Dictionary<ToolStripMenuItem, DataTable>();
            dtpReportDate.CustomFormat = "dddd dd/MM/yyyy";
            dtpReportDate.MinDate = new DateTime(1900, 1, 1);
            dtpReportDate.MaxDate = new DateTime(2100, 12, 31);

            SetInitialDateTimeValue();
            EnableDoubleBufferingForControls(this);
        }

        private void SetInitialDateTimeValue()
        {
            DateTime today = DateTime.Now;

            // Kiểm tra hôm nay là thứ mấy
            if (today.DayOfWeek == DayOfWeek.Monday)
            {
                // Nếu hôm nay là thứ 2, đặt giá trị thành 2 ngày trước (ngày thứ 7)
                dtpReportDate.Value = today.AddDays(-2);
            }
            else
            {
                // Ngược lại, đặt giá trị thành 1 ngày trước
                dtpReportDate.Value = today.AddDays(-1);
            }
        }

        public bool ShowHomeButton
        {
            set { btnHome.Visible = value; }
        }

        private void EnableDoubleBufferingForControls(Control parentControl)
        {
            foreach (Control control in parentControl.Controls)
            {
                if (control is TextBox || control is Label || control is Panel || control is TableLayoutPanel)
                {
                    control.EnableDoubleBuffering();
                }

                if (control.HasChildren)
                {
                    EnableDoubleBufferingForControls(control);
                }
            }
        }

        private void InitializeDataGridView()
        {
            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, dgvReport, new object[] { true });

            dgvReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            dgvReport.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dgvReport.DefaultCellStyle.Font = new System.Drawing.Font("Times New Roman", 10);
            dgvReport.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Time New Roman", 9, System.Drawing.FontStyle.Bold);
            dgvReport.RowTemplate.Height = 30;
            dgvReport.ColumnHeadersDefaultCellStyle.Padding = new Padding(10, 5, 10, 5);
            dgvReport.EnableHeadersVisualStyles = false;
            dgvReport.RowHeadersDefaultCellStyle.BackColor = System.Drawing.Color.White;
            dgvReport.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dgvReport.RowHeadersVisible = false;

            // Ẩn cột FirstLetter nếu tồn tại
        }

        private void LoadMenu()
        {
            plantNameItems = new List<ToolStripMenuItem>();

            monthlyReportItem.Click += MonthlyReportItem_Click;
            dailyReportItem.Click += DailyReportItem_Click;

            menuStrip1.Items.Add(monthlyReportItem);
            menuStrip1.Items.Add(dailyReportItem);

            List<string> plantNames = dbManager.GetPlantNames();
            plantNames.Sort();
            foreach (string plantName in plantNames)
            {
                ToolStripMenuItem plantNameItem = new ToolStripMenuItem($"Plant {plantName}");
                plantNameItem.Click += PlantNameItem_Click;
                plantNameItems.Add(plantNameItem);
                menuStrip1.Items.Add(plantNameItem);
            }

            InitializeDataGridView();
        }

        private void ColorAlternate1RowPairs()
        {
            // Sự kiện RowPrePaint được gọi trước khi dòng được vẽ, cho phép bạn thực hiện tô màu xen kẽ
            dgvReport.RowPrePaint += (sender, e) =>
            {
                // Chỉ xử lý khi dòng thực tế (loại bỏ dòng Header)
                if (e.RowIndex >= 0)
                {
                    // Xác định màu nền dựa trên số lẻ chẵn của dòng
                    Color backgroundColor = e.RowIndex % 2 == 0 ? Color.AliceBlue : Color.White;

                    // Tô màu nền cho toàn bộ dòng
                    dgvReport.Rows[e.RowIndex].DefaultCellStyle.BackColor = backgroundColor;
                }
            };
        }

        private void ColorAlternateRowPairs()
        {
            // Sự kiện RowPrePaint được gọi trước khi dòng được vẽ, cho phép bạn thực hiện tô màu xen kẽ
            dgvReport.RowPrePaint += (sender, e) =>
            {
                // Chỉ xử lý khi dòng thực tế (loại bỏ dòng Header)
                if (e.RowIndex >= 0)
                {
                    int pairsPerColor = 2; // Số dòng mỗi màu

                    // Xác định màu nền dựa trên cặp dòng
                    Color backgroundColor = (e.RowIndex / pairsPerColor) % 2 == 0 ? Color.AliceBlue : Color.White;

                    // Tô màu nền cho toàn bộ dòng
                    dgvReport.Rows[e.RowIndex].DefaultCellStyle.BackColor = backgroundColor;
                }
            };
        }
        private void DisableColumnSorting(DataGridView dataGridView)
        {
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
        private void FocusTodayColumn(DataGridView dataGridView)
        {
            // Lấy ngày hiện tại dưới dạng "dd/MM/yyyy"
            string todayColumnName = DateTime.Now.ToString("dd/MM/yyyy");

            // Duyệt qua tất cả các cột để tìm cột có tên là ngày hiện tại
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                if (column.HeaderText == todayColumnName)
                {
                    // Đặt focus vào cột tìm được
                    column.DataGridView.CurrentCell = column.DataGridView.Rows[0].Cells[column.Index];
                    break;
                }
            }
        }

        private void MonthlyReportItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (monthlyReportItem != null && monthlyDataTable != null)
                {
                    dgvReport.Columns.Clear();
                    dgvReport.DataSource = null;

                    LoadMonthlyReportData();

                    if (menuItemDataTableMapping != null)
                    {
                        menuItemDataTableMapping[monthlyReportItem] = monthlyDataTable;
                    }
                    else
                    {
                        MessageBox.Show("menuItemDataTableMapping is null.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("monthlyReportItem or monthlyDataTable is null.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            DisableColumnSorting(dgvReport);

            ColorAlternate1RowPairs();
        }
        private void DailyReportItem_Click(object sender, EventArgs e)
        {
            try
            {
                DailyPlantResults();

                if (dailyReportItem != null && dailyDataTable != null)
                {
                    menuItemDataTableMapping[dailyReportItem] = dailyDataTable;
                }
                else
                {
                    // Xử lý khi dailyReportItem hoặc dailyDataTable là null, ví dụ:
                    MessageBox.Show("dailyReportItem or dailyDataTable is null.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            ColorAlternate1RowPairs();
        }

        private void PlantNameItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem clickedItem = (ToolStripMenuItem)sender;
            string fullPlantName = clickedItem.Text;

            selectedPlantName = fullPlantName.Replace("Plant ", "");

            // Làm mới DataGridView trước khi thay đổi dữ liệu
            dgvReport.Columns.Clear();
            dgvReport.DataSource = null;

            DataTable plantDataTable = CreatePlantDataTable(selectedPlantName);
            plantDataTableMapping[selectedPlantName] = plantDataTable;

            SetColumnDisplayIndexes(dgvReport);
            PlantResults();
            ColorAlternateRowPairs();
        }

        private void GenerateDataTablesForMenuItems()
        {
            foreach (ToolStripMenuItem item in menuStrip1.Items)
            {
                if (item != monthlyReportItem && item != dailyReportItem)
                {
                    string fullPlantName = item.Text;
                    selectedPlantName = fullPlantName.Replace("Plant ", "");

                    DataTable plantDataTable = CreatePlantDataTable(selectedPlantName);

                    menuItemDataTableMapping[item] = plantDataTable;
                    itemNameDataTableMapping[item.Text] = plantDataTable;

                    shouldDisplayDataOnDataGridView = false;
                    PlantResults();
                    shouldDisplayDataOnDataGridView = true;
                }
            }
        }

        private bool shouldDisplayDataOnDataGridView = true;


        private void BtnExport_Click(object sender, EventArgs e)
        {
            try
            {
                ExportDataToExcel(() =>
                {
                    MessageBox.Show("Dữ liệu đã được xuất thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xuất dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void ExportDataToExcel(Action callback)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = true,
                InitialDirectory = @"D:\Report"
            };

            DateTime selectedDate = dtpReportDate.Value;
            string defaultFileName = $"APH Digital Auto Reporting – BPFC & Heating Machine Temperature Compliance {selectedDate.ToString("yyyyMMdd")}.xlsx";
            saveFileDialog.FileName = defaultFileName;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;
                object originalDataSource = dgvReport.DataSource;
                dgvReport.SuspendLayout();

                using (ExcelExporter excelExporter = new ExcelExporter(filePath))
                {
                    Dictionary<string, DataTable> exportData = new Dictionary<string, DataTable>();

                    DataTable monthlyDataTable = LoadMonthlyReportData();
                    exportData["Monthly Report"] = monthlyDataTable;

                    DataTable dailyDataTable = DailyPlantResults();
                    exportData["Daily Report"] = dailyDataTable;

                    GenerateDataTablesForMenuItems();
                    foreach (var item in plantNameItems)
                    {
                        if (menuItemDataTableMapping.TryGetValue(item, out DataTable plantDataTable))
                        {
                            exportData[item.Text] = plantDataTable;
                        }
                    }

                    foreach (var pair in exportData)
                    {
                        string sheetName = pair.Key;
                        DataTable dataTable = pair.Value;

                        if (!ShouldSkipSheet(sheetName))
                        {
                            ExcelPackage excelPackage = excelExporter.GetExcelPackage();
                            excelExporter.AddDataTableToSheet(dataTable, sheetName);
                        }
                    }

                    excelExporter.SaveWorkbookWithUniqueName(filePath);
                }

                dgvReport.DataSource = originalDataSource;
                dgvReport.ResumeLayout();

                callback();
            }
        }

        private bool ShouldSkipSheet(string sheetName)
        {
            return !(sheetName.Contains("Monthly Report") || sheetName.Contains("Daily Report") || sheetName.StartsWith("Plant"));
        }


        private void PlantResults()
        {
            if (!string.IsNullOrEmpty(selectedPlantName))
            {
                List<int> selectedLineIDs = dbManager.GetProductionLineIDs(selectedPlantName);

                if (selectedLineIDs.Count > 0)
                {
                    DateTime selectedDate = dtpReportDate.Value.Date;

                    // Trích xuất dữ liệu từ cơ sở dữ liệu
                    var query = dbContext.GetResults(selectedDate)
                        .Where(result => selectedLineIDs.Contains(result.LineID));

                    if (query.Any())
                    {
                        // Tạo DataTable từ danh sách kết quả
                        DataTable plantDataTable = query.AsDataTable();

                        // Chỉ hiển thị dữ liệu trên DataGridView nếu được phép
                        if (shouldDisplayDataOnDataGridView)
                        {
                            dgvReport.Columns.Clear();

                            dgvReport.DataSource = plantDataTable;
                            Freeze5Columns(dgvReport);

                            SetColumnHeaders();

                            SortDataTableByLineName(plantDataTable);

                            SetColumnDisplayIndexes(dgvReport);

                            List<DataGridViewColumn> columnsToRemove = new List<DataGridViewColumn>();

                            foreach (DataGridViewColumn column in dgvReport.Columns)
                            {
                                column.SortMode = DataGridViewColumnSortMode.NotSortable;

                                // Check if the column name is in the list of columns to exclude
                                List<string> excludedColumns = new List<string>
                        {
                            "FinalTimeResult", "FinalTempResult", "FinalChemicalResult",
                            "LineResult"
                        };

                                if (excludedColumns.Contains(column.Name))
                                {
                                    // Add the unwanted columns to the list
                                    columnsToRemove.Add(column);
                                }
                            }

                            foreach (DataGridViewColumn columnToRemove in columnsToRemove)
                            {
                                dgvReport.Columns.Remove(columnToRemove);
                            }
                        }
                    }
                }
            }
        }

        private DataTable LoadMonthlyReportData()
        {
            DataTable monthlyReportDataTable = null;

            try
            {
                if (monthlyDataTable == null)
                {
                    monthlyDataTable = new DataTable();
                }
                else
                {
                    monthlyDataTable.Clear();
                }

                DailyPlantResults();

                if (dgvReport.Rows.Count > 0)
                {
                    List<string> dayColumnNames;
                    DateTime selectedMonth = dtpReportDate.Value;

                    monthlyReportDataTable = CreateMonthlyReportDataTable(selectedMonth, out dayColumnNames);

                    PopulateMonthlyReportDataTable(monthlyReportDataTable, dayColumnNames);

                    DisplayMonthlyReportInDataGridView(monthlyReportDataTable);

                    FocusTodayColumn(dgvReport);

                    Freeze3Columns(dgvReport);

                    monthlyDataTable = monthlyReportDataTable;
                }
                else
                {
                    ShowNoDataWarning("Monthly Report");
                }
            }
            catch (Exception ex)
            {
                ShowNoDataWarning("Monthly Report");
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return monthlyReportDataTable;
        }
        private void Freeze3Columns(DataGridView dataGridView)
        {
            // Số lượng cột bạn muốn đóng băng
            int numberOfFrozenColumns = 3;

            // Duyệt qua số lượng cột cần đóng băng và đặt thuộc tính Frozen
            for (int i = 0; i < numberOfFrozenColumns && i < dataGridView.Columns.Count; i++)
            {
                dataGridView.Columns[i].Frozen = true;
            }
        }
        private void Freeze5Columns(DataGridView dataGridView)
        {
            // Số lượng cột bạn muốn đóng băng
            int numberOfFrozenColumns = 5;

            // Duyệt qua số lượng cột cần đóng băng và đặt thuộc tính Frozen
            for (int i = 0; i < numberOfFrozenColumns && i < dataGridView.Columns.Count; i++)
            {
                dataGridView.Columns[i].Frozen = true;
            }
        }

        private DataTable CreateMonthlyReportDataTable(DateTime selectedMonth, out List<string> dayColumnNames)
        {
            dayColumnNames = new List<string>();

            DataTable monthlyReportDataTable = new DataTable();

            monthlyReportDataTable.Columns.Add("LineID", typeof(int));
            monthlyReportDataTable.Columns.Add("PlantName", typeof(string));
            monthlyReportDataTable.Columns.Add("LineName", typeof(string));

            // Lấy số ngày trong tháng được chọn
            int numberOfDaysInMonth = DateTime.DaysInMonth(selectedMonth.Year, selectedMonth.Month);

            for (int day = 1; day <= numberOfDaysInMonth; day++)
            {
                try
                {
                    // Lấy ngày trong tháng được chọn
                    DateTime currentDate = new DateTime(selectedMonth.Year, selectedMonth.Month, day);

                    // Tạo tên cột mới
                    string columnName = currentDate.ToString("dd-MM-yyyy");

                    // Kiểm tra xem tên cột có giá trị không rỗng
                    if (!string.IsNullOrEmpty(columnName))
                    {
                        // Kiểm tra xem cột đã tồn tại trong DataTable chưa
                        if (!monthlyReportDataTable.Columns.Contains(columnName))
                        {
                            // Nếu chưa tồn tại, thêm cột mới
                            monthlyReportDataTable.Columns.Add(columnName, typeof(string));
                            dayColumnNames.Add(columnName);

                        }
                        else
                        {
                            Console.WriteLine($"Column with name {columnName} already exists.");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Column name is null or empty for day {day}.");
                    }
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    Console.WriteLine($"Error creating DateTime for day {day}: {ex.Message}");
                }
            }
            return monthlyReportDataTable;
        }

        private void PopulateMonthlyReportDataTable(DataTable monthlyReportDataTable, List<string> dayColumnNames)
        {
            // Lấy danh sách LineName từ DbContext
            List<(string LineName, int LineID)> lineNames = dbContext.GetLineNamesWithID()
                .OrderBy(x => x.LineName, new LineNameComparer())
                .ThenBy(x => x.LineID)
                .ToList();

            // Lấy tháng được chọn từ DateTimePicker
            DateTime selectedMonth = dtpReportDate.Value;

            // Lấy ngày đầu tiên và ngày cuối cùng của tháng được chọn
            DateTime startDate = new DateTime(selectedMonth.Year, selectedMonth.Month, 1);
            DateTime endDate = startDate.AddMonths(1).AddDays(-1);

            // Lấy dữ liệu từ cơ sở dữ liệu từ ngày đầu tháng đến ngày cuối tháng
            foreach ((string lineName, int lineID) in lineNames)
            {
                // Lấy giá trị LineResult từ cơ sở dữ liệu
                Dictionary<DateTime, string> lineResults = dbManager.GetLineResultForDateAndLineName(lineName, startDate, endDate);

                // Tìm dòng trong DataTable tương ứng với LineName
                DataRow[] rows = monthlyReportDataTable.Select($"LineName = '{lineName}'");

                if (rows.Length > 0)
                {
                    DataRow dataRow = rows[0];

                    // Duyệt qua các cột để điền dữ liệu
                    foreach (DataColumn column in monthlyReportDataTable.Columns)
                    {
                        if (column.ColumnName != "PlantName" && column.ColumnName != "LineName" && column.ColumnName != "LineID")
                        {
                            // Kiểm tra nếu tên cột giống với ReportDate
                            if (DateTime.TryParseExact(column.ColumnName, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime columnDate))
                            {
                                // Tìm giá trị tương ứng trong dữ liệu cơ sở dữ liệu
                                if (lineResults.TryGetValue(columnDate, out string lineResult))
                                {
                                    dataRow[column.ColumnName] = lineResult;
                                }
                                else
                                {
                                    // Nếu không có dữ liệu, điền giá trị trống hoặc 0 (tuỳ thuộc vào kiểu dữ liệu cột)
                                    dataRow[column.ColumnName] = DBNull.Value;
                                }
                            }
                        }
                    }
                }
                else
                {
                    // Nếu không tìm thấy dòng tương ứng, có thể bạn muốn thêm một dòng mới vào DataTable
                    DataRow newRow = monthlyReportDataTable.NewRow();
                    newRow["LineName"] = lineName;
                    newRow["PlantName"] = "Plant " + lineName.Substring(0, 1);
                    newRow["LineID"] = lineID;

                    // Duyệt qua các cột để điền dữ liệu
                    foreach (DataColumn column in monthlyReportDataTable.Columns)
                    {
                        if (column.ColumnName != "PlantName" && column.ColumnName != "LineName" && column.ColumnName != "LineID")
                        {
                            DateTime columnDate = DateTime.ParseExact(column.ColumnName, "dd-MM-yyyy", CultureInfo.InvariantCulture);

                            // Tìm giá trị tương ứng trong dữ liệu cơ sở dữ liệu
                            if (lineResults.TryGetValue(columnDate, out string lineResult))
                            {
                                newRow[column.ColumnName] = lineResult;
                            }
                            else
                            {
                                newRow[column.ColumnName] = DBNull.Value;
                            }
                        }
                    }

                    // Thêm dòng mới vào DataTable
                    monthlyReportDataTable.Rows.Add(newRow);
                }
            }
            SortDataTableByLineName(monthlyReportDataTable);

            // Tạo và chèn 4 dòng mới vào DataTable
            AddNewRowsToMonthlyReportDataTable(monthlyReportDataTable, dayColumnNames);

            FillOffDayForPastSundayColumns(monthlyReportDataTable);
        }

        private void FillOffDayForPastSundayColumns(DataTable monthlyReportDataTable)
        {
            DateTime currentDate = DateTime.Now.Date;

            foreach (DataColumn column in monthlyReportDataTable.Columns)
            {
                string columnName = column.ColumnName;

                // Kiểm tra xem cột có định dạng ngày không
                if (DateTime.TryParseExact(columnName, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime columnDate))
                {
                    // Kiểm tra xem ngày đó có phải là Chủ Nhật và trước ngày hiện tại không
                    if (columnDate.DayOfWeek == DayOfWeek.Sunday && columnDate < currentDate)
                    {
                        // Điền "OFF DAY" vào tất cả các ô trong cột
                        foreach (DataRow row in monthlyReportDataTable.Rows)
                        {
                            row[columnName] = "OFF DAY";
                        }
                    }
                    else if (monthlyReportDataTable.Rows.Count >= 5)
                    {
                        DateTime yesterday = DateTime.Now.Date.AddDays(-1);
                        var totalLineQtyValue = monthlyReportDataTable.Rows[3][column];
                        if (columnDate < yesterday && (totalLineQtyValue == null || totalLineQtyValue == DBNull.Value || totalLineQtyValue.ToString() == "0"))
                        {
                            // Điền "OFF DAY" vào tất cả các ô trong cột
                            foreach (DataRow row in monthlyReportDataTable.Rows)
                            {
                                row[column.ColumnName] = "OFF DAY";
                            }
                        }
                    }
                }
            }
        }

        // Tạo một lớp so sánh tùy chỉnh để sắp xếp theo thứ tự đúng của LineName
        public class LineNameComparer : IComparer<string>
        {
            public int Compare(string x, string y)
            {
                // Tách phần chữ và phần số từ tên LineName
                string prefixX = new string(x.TakeWhile(char.IsLetter).ToArray());
                string prefixY = new string(y.TakeWhile(char.IsLetter).ToArray());

                int numberX, numberY;

                // Nếu không thể chuyển đổi phần số thành số nguyên, giữ nguyên thứ tự
                if (!int.TryParse(new string(x.SkipWhile(char.IsLetter).ToArray()), out numberX) ||
                    !int.TryParse(new string(y.SkipWhile(char.IsLetter).ToArray()), out numberY))
                {
                    return x.CompareTo(y);
                }

                // So sánh theo phần chữ và sau đó theo số
                int prefixComparison = string.Compare(prefixX, prefixY, StringComparison.OrdinalIgnoreCase);
                if (prefixComparison != 0)
                {
                    return prefixComparison;
                }

                return numberX.CompareTo(numberY);
            }
        }

        private void AddNewRowsToMonthlyReportDataTable(DataTable monthlyReportDataTable, List<string> dayColumnNames)
        {
            // Chèn 4 dòng mới vào DataTable
            DataRow newRow1 = monthlyReportDataTable.NewRow();
            newRow1["PlantName"] = "";
            newRow1["LineName"] = "Compliance %";
            monthlyReportDataTable.Rows.InsertAt(newRow1, 0);

            DataRow newRow2 = monthlyReportDataTable.NewRow();
            newRow2["PlantName"] = "";
            newRow2["LineName"] = "Total Pass";
            monthlyReportDataTable.Rows.InsertAt(newRow2, 1);

            DataRow newRow3 = monthlyReportDataTable.NewRow();
            newRow3["PlantName"] = "";
            newRow3["LineName"] = "Total Fail";
            monthlyReportDataTable.Rows.InsertAt(newRow3, 2);

            DataRow newRow4 = monthlyReportDataTable.NewRow();
            newRow4["PlantName"] = "";
            newRow4["LineName"] = "Total Line QTY";
            monthlyReportDataTable.Rows.InsertAt(newRow4, 3);

            // Đặt giá trị cho 4 dòng mới trong vòng lặp
            foreach (string columnName in dayColumnNames)
            {
                monthlyReportDataTable.Rows[0][columnName] = CalculateCompliancePercentage(monthlyReportDataTable, columnName).ToString("0.0") + "%";
                monthlyReportDataTable.Rows[1][columnName] = CountPassInColumn(monthlyReportDataTable, columnName).ToString();
                monthlyReportDataTable.Rows[2][columnName] = CalculateTotalFail(monthlyReportDataTable, columnName).ToString();
                monthlyReportDataTable.Rows[3][columnName] = CalculateTotalLineQuantity(monthlyReportDataTable, columnName).ToString();
            }
        }

        private void DisplayMonthlyReportInDataGridView(DataTable monthlyReportDataTable)
        {
            // Hiển thị DataTable trong DataGridView
            dgvReport.DataSource = monthlyReportDataTable;

            dgvReport.Columns["PlantName"].HeaderText = "";
            dgvReport.Columns["LineName"].HeaderText = "";
            dgvReport.Columns["PlantName"].DisplayIndex = 0;
            dgvReport.Refresh();

        }
        private void ShowNoDataWarning(string reportType)
        {
            MessageBox.Show($"Không có dữ liệu để tạo {reportType} Report.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private DataTable DailyPlantResults()
        {
            DataTable dailyDataTable = new DataTable();
            ConfigureDailyDataTableColumns(dailyDataTable);

            List<string> plantNames = dbManager.GetPlantNames();

            if (plantNames.Count > 0)
            {
                foreach (string selectedPlantName in plantNames)
                {
                    ProcessPlantResults(selectedPlantName, dailyDataTable);
                }

                // Sắp xếp dailyDataTable bằng phương thức mới
                SortDataTableByLineName(dailyDataTable);

                BindDailyDataTableToGridView(dailyDataTable);
            }
            else
            {
                MessageBox.Show("Không tìm thấy dữ liệu cho bất kỳ xưởng nào.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            return dailyDataTable;
        }
        private void SortDataTableByLineName(DataTable dataTable)
        {
            // Kiểm tra xem cột 'FirstLetter' đã tồn tại hay chưa
            if (!dataTable.Columns.Contains("FirstLetter"))
            {
                // Thêm cột mới để lưu trữ chữ cái đầu tiên của LineName
                dataTable.Columns.Add("FirstLetter", typeof(char));
            }

            // Kiểm tra xem cột 'LineNumber' đã tồn tại hay chưa
            if (!dataTable.Columns.Contains("LineNumber"))
            {
                // Thêm cột mới để lưu trữ số phía sau của LineName
                dataTable.Columns.Add("LineNumber", typeof(int));
            }

            // Tách chữ cái đầu tiên và chuyển đổi phần số phía sau thành số nguyên
            foreach (DataRow row in dataTable.Rows)
            {
                string lineName = row["LineName"].ToString();
                row["FirstLetter"] = lineName.Substring(0, 1)[0]; // Lấy chữ cái đầu tiên
                row["LineNumber"] = Convert.ToInt32(lineName.Substring(2));
            }

            // Sắp xếp dataTable theo FirstLetter và LineNumber
            dataTable.DefaultView.Sort = "FirstLetter ASC, LineNumber ASC";
            dataTable.DefaultView.ToTable();
        }

        private void ConfigureDailyDataTableColumns(DataTable dailyDataTable)
        {
            dailyDataTable.Columns.Add("PlantName", typeof(string));
            dailyDataTable.Columns.Add("LineID", typeof(int));
            dailyDataTable.Columns.Add("LineName", typeof(string));
            dailyDataTable.Columns.Add("FinalTimeResult", typeof(string));
            dailyDataTable.Columns.Add("FinalTempResult", typeof(string));
            dailyDataTable.Columns.Add("FinalChemicalResult", typeof(string));
            dailyDataTable.Columns.Add("LineResult", typeof(string));
            dailyDataTable.Columns["PlantName"].SetOrdinal(0);
        }

        private void ProcessPlantResults(string selectedPlantName, DataTable dailyDataTable)
        {
            List<int> selectedLineIDs = dbManager.GetProductionLineIDs(selectedPlantName);

            if (selectedLineIDs.Count > 0)
            {
                DateTime selectedDate = dtpReportDate.Value.Date;

                var query = dbContext.GetResults(selectedDate)
                    .Where(result => selectedLineIDs.Contains(result.LineID))
                    .ToList();

                List<string> uniqueLineNames = query.Select(item => item.LineName).Distinct().ToList();

                foreach (string lineName in uniqueLineNames)
                {
                    DataRow newRow = dailyDataTable.NewRow();
                    newRow["PlantName"] = "Plant " + selectedPlantName;
                    newRow["LineName"] = lineName;
                    newRow["LineID"] = query.First(item => item.LineName == lineName).LineID;
                    newRow["FinalTimeResult"] = CalculateTimeResult(query, lineName);
                    newRow["FinalTempResult"] = CalculateTempResult(query, lineName);
                    newRow["FinalChemicalResult"] = CalculateChemicalResult(query, lineName);
                    newRow["LineResult"] = CompareFinalResultsByLineName(query, lineName);
                    dailyDataTable.Rows.Add(newRow);
                }
            }
        }

        private void BindDailyDataTableToGridView(DataTable dailyDataTable)
        {
            // Kiểm tra và bỏ frozen nếu có
            foreach (DataGridViewColumn column in dgvReport.Columns)
            {
                column.Frozen = false;
            }

            dgvReport.DataSource = dailyDataTable;
            dgvReport.Columns["PlantName"].HeaderText = "Plant";
            dgvReport.Columns["LineID"].Visible = false;
            dgvReport.Columns["FirstLetter"].Visible = false;
            dgvReport.Columns["LineNumber"].Visible = false;

            dgvReport.Columns["LineName"].HeaderText = "Line";
            dgvReport.Columns["FinalTimeResult"].HeaderText = "Thời gian";
            dgvReport.Columns["FinalTempResult"].HeaderText = "Nhiệt độ";
            dgvReport.Columns["FinalChemicalResult"].HeaderText = "Hóa chất";
            dgvReport.Columns["LineResult"].HeaderText = "Kết quả";

            foreach (DataGridViewColumn column in dgvReport.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            AddNewRowsToDailyReportDataTable(dailyDataTable);

            // Lưu dữ liệu vào cơ sở dữ liệu
            SaveDataTableToDatabase(dailyDataTable);
        }

        private void SaveDataTableToDatabase(DataTable dataTable)
        {
            var rowsToSave = dataTable.AsEnumerable()
                .Where(row => !string.IsNullOrEmpty(row.Field<string>("PlantName")) &&
                               row.Field<string>("LineName").Length < 5)
                .ToList();

            if (rowsToSave.Count > 0)
            {
                DataTable filteredDataTable = rowsToSave.CopyToDataTable();
                dbManager.SaveOrUpdateResultsToDatabase(filteredDataTable, dtpReportDate.Value.Date);
            }
        }

        private void AddNewRowsToDailyReportDataTable(DataTable dailyReportDataTable)
        {
            // Chèn 4 dòng mới vào DataTable
            DataRow newRow1 = dailyReportDataTable.NewRow();
            newRow1["PlantName"] = "";
            newRow1["LineName"] = "Compliance %";
            dailyReportDataTable.Rows.InsertAt(newRow1, 0);

            DataRow newRow2 = dailyReportDataTable.NewRow();
            newRow2["PlantName"] = "";
            newRow2["LineName"] = "Total Pass";
            dailyReportDataTable.Rows.InsertAt(newRow2, 1);

            DataRow newRow3 = dailyReportDataTable.NewRow();
            newRow3["PlantName"] = "";
            newRow3["LineName"] = "Total Fail";
            dailyReportDataTable.Rows.InsertAt(newRow3, 2);

            DataRow newRow4 = dailyReportDataTable.NewRow();
            newRow4["PlantName"] = "";
            newRow4["LineName"] = "Total Line QTY";
            dailyReportDataTable.Rows.InsertAt(newRow4, 3);

            foreach (DataColumn column in dailyReportDataTable.Columns)
            {
                string columnName = column.ColumnName;

                if (columnName != "LineID")
                {
                    if (columnName.Contains("Result"))
                    {
                        dailyReportDataTable.Rows[0][columnName] = CalculateCompliancePercentage(dailyReportDataTable, columnName).ToString("0.0") + "%";
                        dailyReportDataTable.Rows[1][columnName] = CountPassInColumn(dailyReportDataTable, columnName).ToString();
                        dailyReportDataTable.Rows[2][columnName] = CalculateTotalFail(dailyReportDataTable, columnName).ToString();
                        dailyReportDataTable.Rows[3][columnName] = CalculateTotalLineQuantity(dailyReportDataTable, columnName).ToString();
                    }
                }
            }
        }

        private void dtpReportDate_ValueChanged(object sender, EventArgs e)
        {
            DailyPlantResults();
            dtpReportDate.Update();
            Console.WriteLine($"DateTimePicker new value: {dtpReportDate.Value}");
        }
        private void dgvDaily_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && e.RowIndex < dgvReport.Rows.Count && e.ColumnIndex < dgvReport.Columns.Count)
            {
                if (e.ColumnIndex == dgvReport.Columns["ActualTemp_1"]?.Index ||
                    e.ColumnIndex == dgvReport.Columns["ActualTemp_2"]?.Index ||
                    e.ColumnIndex == dgvReport.Columns["ActualTemp_3"]?.Index)
                {
                    if (e.Value != null && float.TryParse(e.Value.ToString(), out float temperature))
                    {
                        e.Value = temperature.ToString() + " °C";
                        e.FormattingApplied = true;
                    }
                }
                else if (e.ColumnIndex == dgvReport.Columns["StandardTemp_1"]?.Index ||
                         e.ColumnIndex == dgvReport.Columns["StandardTemp_2"]?.Index ||
                         e.ColumnIndex == dgvReport.Columns["StandardTemp_3"]?.Index)
                {
                    if (e.Value != null && float.TryParse(e.Value.ToString(), out float temperature))
                    {
                        // Chỉ thêm chuỗi " ±5°C" khi giá trị không phải là null hoặc chuỗi rỗng
                        if (!string.IsNullOrEmpty(e.Value.ToString()))
                        {
                            e.Value = temperature.ToString() + " ±5°C";
                            e.FormattingApplied = true;
                        }
                    }
                }
                else if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
                {
                    DataGridViewCell cell = dgvReport.Rows[e.RowIndex].Cells[e.ColumnIndex];

                    if (cell.Value != null && cell.Value.ToString() == "FAIL")
                    {
                        cell.Style.BackColor = System.Drawing.Color.LightGoldenrodYellow;
                        cell.Style.ForeColor = System.Drawing.Color.Red;
                    }
                    if (cell.Value != null && cell.Value.ToString() == "PASS")
                    {
                        cell.Style.BackColor = System.Drawing.Color.LightGoldenrodYellow;
                        cell.Style.ForeColor = System.Drawing.Color.Black;
                    }

                    if (cell.Value != null && cell.Value.ToString() == "OFF DAY")
                    {
                        cell.Style.BackColor = System.Drawing.Color.Orange;
                    }

                    if (e.RowIndex <= 3)
                    {
                        string cellValue = cell.Value?.ToString();

                        if (cellValue == "0" || cellValue == "0.0%")
                        {
                            int nonZeroCount = 0;

                            for (int i = 0; i < Math.Min(4, dgvReport.Rows.Count); i++)
                            {
                                string valueToCheck = dgvReport.Rows[i].Cells[e.ColumnIndex].Value?.ToString();
                                if (!string.IsNullOrEmpty(valueToCheck) && (valueToCheck != "0" && valueToCheck != "0.0%"))
                                {
                                    nonZeroCount++;
                                    break;
                                }
                            }

                            if (nonZeroCount == 0)
                            {
                                cell.Value = string.Empty;
                            }
                        }
                    }
                }
            }
        }

        private DataTable CreatePlantDataTable(string plantName)
        {
            // Tạo DataTable mới cho xưởng đã chọn
            DataTable plantDataTable = new DataTable();
            plantDataTable.Columns.Add("LineID", typeof(int));
            plantDataTable.Columns.Add("LineNumber", typeof(int));
            plantDataTable.Columns.Add("LineName", typeof(object));
            plantDataTable.Columns.Add("FirstLetter", typeof(char));
            plantDataTable.Columns.Add("Model", typeof(object));
            plantDataTable.Columns.Add("ArticleName", typeof(object));
            plantDataTable.Columns.Add("PartName", typeof(object));
            plantDataTable.Columns.Add("StandardTemp_Heat", typeof(object));
            plantDataTable.Columns.Add("ActualTemp_Heat", typeof(object));
            plantDataTable.Columns.Add("ResultTemp_Heat", typeof(object));
            plantDataTable.Columns.Add("StandardTemp_1", typeof(object));
            plantDataTable.Columns.Add("ActualTemp_1", typeof(object));
            plantDataTable.Columns.Add("ResultTemp_1", typeof(object));
            plantDataTable.Columns.Add("StandardTemp_2", typeof(object));
            plantDataTable.Columns.Add("ActualTemp_2", typeof(object));
            plantDataTable.Columns.Add("ResultTemp_2", typeof(object));
            plantDataTable.Columns.Add("StandardTemp_3", typeof(object));
            plantDataTable.Columns.Add("ActualTemp_3", typeof(object));
            plantDataTable.Columns.Add("ResultTemp_3", typeof(object));
            plantDataTable.Columns.Add("StandardTime_Heat", typeof(object));
            plantDataTable.Columns.Add("ActualTime_Heat", typeof(object));
            plantDataTable.Columns.Add("ResultTime_Heat", typeof(object));
            plantDataTable.Columns.Add("StandardTime_1", typeof(object));
            plantDataTable.Columns.Add("ActualTime_1", typeof(object));
            plantDataTable.Columns.Add("ResultTime_1", typeof(object));
            plantDataTable.Columns.Add("StandardTime_2", typeof(object));
            plantDataTable.Columns.Add("ActualTime_2", typeof(object));
            plantDataTable.Columns.Add("ResultTime_2", typeof(object));
            plantDataTable.Columns.Add("StandardTime_3", typeof(object));
            plantDataTable.Columns.Add("ActualTime_3", typeof(object));
            plantDataTable.Columns.Add("ResultTime_3", typeof(object));
            plantDataTable.Columns.Add("StandardChemical_1", typeof(object));
            plantDataTable.Columns.Add("ActualChemical_1", typeof(object));
            plantDataTable.Columns.Add("ResultChemical_1", typeof(object));
            plantDataTable.Columns.Add("StandardChemical_2", typeof(object));
            plantDataTable.Columns.Add("ActualChemical_2", typeof(object));
            plantDataTable.Columns.Add("ResultChemical_2", typeof(object));
            plantDataTable.Columns.Add("StandardChemical_3", typeof(object));
            plantDataTable.Columns.Add("ActualChemical_3", typeof(object));
            plantDataTable.Columns.Add("ResultChemical_3", typeof(object));

            List<int> selectedLineIDs = dbManager.GetProductionLineIDs(plantName);
            DateTime selectedDate = dtpReportDate.Value.Date;

            var query = dbContext.GetResults(selectedDate)
                .Where(result => selectedLineIDs.Contains(result.LineID));

            foreach (var item in query)
            {
                DataRow row = plantDataTable.NewRow();
                row["LineID"] = item.LineID;
                row["LineName"] = item.LineName;
                row["Model"] = item.Model;
                row["ArticleName"] = item.ArticleName;
                row["PartName"] = item.PartName;

                // Temp columns
                SetColumnValue(row, "StandardTemp_Heat", item.StandardTemp_Heat);
                SetColumnValue(row, "ActualTemp_Heat", item.ActualTemp_Heat);
                SetColumnValue(row, "ResultTemp_Heat", item.ResultTemp_Heat);
                SetColumnValue(row, "StandardTemp_1", item.StandardTemp_1);
                SetColumnValue(row, "ActualTemp_1", item.ActualTemp_1);
                SetColumnValue(row, "ResultTemp_1", item.ResultTemp_1);
                SetColumnValue(row, "StandardTemp_2", item.StandardTemp_2);
                SetColumnValue(row, "ActualTemp_2", item.ActualTemp_2);
                SetColumnValue(row, "ResultTemp_2", item.ResultTemp_2);
                SetColumnValue(row, "StandardTemp_3", item.StandardTemp_3);
                SetColumnValue(row, "ActualTemp_3", item.ActualTemp_3);
                SetColumnValue(row, "ResultTemp_3", item.ResultTemp_3);

                // Time columns
                SetColumnValue(row, "StandardTime_Heat", item.StandardTime_Heat);
                SetColumnValue(row, "ActualTime_Heat", item.ActualTime_Heat);
                SetColumnValue(row, "ResultTime_Heat", item.ResultTime_Heat);
                SetColumnValue(row, "StandardTime_1", item.StandardTime_1);
                SetColumnValue(row, "ActualTime_1", item.ActualTime_1);
                SetColumnValue(row, "ResultTime_1", item.ResultTime_1);
                SetColumnValue(row, "StandardTime_2", item.StandardTime_2);
                SetColumnValue(row, "ActualTime_2", item.ActualTime_2);
                SetColumnValue(row, "ResultTime_2", item.ResultTime_2);
                SetColumnValue(row, "StandardTime_3", item.StandardTime_3);
                SetColumnValue(row, "ActualTime_3", item.ActualTime_3);
                SetColumnValue(row, "ResultTime_3", item.ResultTime_3);

                // Chemical columns
                SetColumnValue(row, "StandardChemical_1", item.StandardChemical_1);
                SetColumnValue(row, "ActualChemical_1", item.ActualChemical_1);
                SetColumnValue(row, "ResultChemical_1", item.ResultChemical_1);
                SetColumnValue(row, "StandardChemical_2", item.StandardChemical_2);
                SetColumnValue(row, "ActualChemical_2", item.ActualChemical_2);
                SetColumnValue(row, "ResultChemical_2", item.ResultChemical_2);
                SetColumnValue(row, "StandardChemical_3", item.StandardChemical_3);
                SetColumnValue(row, "ActualChemical_3", item.ActualChemical_3);
                SetColumnValue(row, "ResultChemical_3", item.ResultChemical_3);

                plantDataTable.Rows.Add(row);
                SetColumnOrder(plantDataTable);
                SortDataTableByLineName(plantDataTable);
            }

            return plantDataTable;
        }

        private void SetColumnValue(DataRow row, string columnName, object value)
        {
            // Check for null before assigning the value
            row[columnName] = value ?? DBNull.Value;
        }


        private void SetColumnHeaders()
        {
            dgvReport.Columns["LineID"].Visible = false;
            dgvReport.Columns["LineName"].HeaderText = "Line";
            dgvReport.Columns["Model"].HeaderText = "Model";
            dgvReport.Columns["ArticleName"].HeaderText = "Article";
            dgvReport.Columns["PartName"].HeaderText = "Component";

            dgvReport.Columns["StandardTime_Heat"].HeaderText = "Time\nStandard Heat";
            dgvReport.Columns["ActualTime_Heat"].HeaderText = "Time\nAcutal Heating";
            dgvReport.Columns["ResultTime_Heat"].HeaderText = "Time\nResult Heating";

            dgvReport.Columns["StandardTemp_Heat"].HeaderText = "Temperature\nStandard Heating";
            dgvReport.Columns["ActualTemp_Heat"].HeaderText = "Temperature\nAcutal Heating";
            dgvReport.Columns["ResultTemp_Heat"].HeaderText = "Temperature\nResult Heating";

            dgvReport.Columns["StandardTime_1"].HeaderText = "Time\nStandard 1";
            dgvReport.Columns["ActualTime_1"].HeaderText = "Time\nAcutal 1";
            dgvReport.Columns["ResultTime_1"].HeaderText = "Time\nResult 1";

            dgvReport.Columns["StandardTemp_1"].HeaderText = "Temperature\nStandard 1";
            dgvReport.Columns["ActualTemp_1"].HeaderText = "Temperature\nAcutal 1";
            dgvReport.Columns["ResultTemp_1"].HeaderText = "Temperature\nResult 1";

            dgvReport.Columns["StandardChemical_1"].HeaderText = "Chemical\nStandard 1";
            dgvReport.Columns["ActualChemical_1"].HeaderText = "Chemical\nAcutal 1";
            dgvReport.Columns["ResultChemical_1"].HeaderText = "Chemical\nResult 1";

            dgvReport.Columns["StandardTime_2"].HeaderText = "Time\nStandard 2";
            dgvReport.Columns["ActualTime_2"].HeaderText = "Time\nAcutal 2";
            dgvReport.Columns["ResultTime_2"].HeaderText = "Time\nResult 2";
            dgvReport.Columns["StandardTemp_2"].HeaderText = "Temperature\nStandard 2";
            dgvReport.Columns["ActualTemp_2"].HeaderText = "Temperature\nAcutal 2";
            dgvReport.Columns["ResultTemp_2"].HeaderText = "Temperature\nResult 2";
            dgvReport.Columns["StandardChemical_2"].HeaderText = "Chemical\nStandard 2";
            dgvReport.Columns["ActualChemical_2"].HeaderText = "Chemical\nAcutal 2";
            dgvReport.Columns["ResultChemical_2"].HeaderText = "Chemical\nResult 2";

            dgvReport.Columns["StandardTime_3"].HeaderText = "Time\nStandard 3";
            dgvReport.Columns["ActualTime_3"].HeaderText = "Time\nAcutal 3";
            dgvReport.Columns["ResultTime_3"].HeaderText = "Time\nResult 3";
            dgvReport.Columns["StandardTemp_3"].HeaderText = "Temperature\nStandard 3";
            dgvReport.Columns["ActualTemp_3"].HeaderText = "Temperature\nAcutal 3";
            dgvReport.Columns["ResultTemp_3"].HeaderText = "Temperature\nResult 3";
            dgvReport.Columns["StandardChemical_3"].HeaderText = "Chemical\nStandard 3";
            dgvReport.Columns["ActualChemical_3"].HeaderText = "Chemical\nAcutal 3";
            dgvReport.Columns["ResultChemical_3"].HeaderText = "Chemical\nResult 3";
        }

        private void SetColumnOrder(DataTable dataTable)
        {
            // Đưa LineName, Model,... lên đầu
            SetColumnOrdinal(dataTable, "LineName", 0);
            SetColumnOrdinal(dataTable, "Model", 1);
            SetColumnOrdinal(dataTable, "ArticleName", 2);
            SetColumnOrdinal(dataTable, "PartName", 3);

            // Nhóm Heat Time và Temp
            SetColumnOrdinal(dataTable, "StandardTime_Heat", 4);
            SetColumnOrdinal(dataTable, "ActualTime_Heat", 5);
            SetColumnOrdinal(dataTable, "ResultTime_Heat", 6);
            SetColumnOrdinal(dataTable, "StandardTemp_Heat", 7);
            SetColumnOrdinal(dataTable, "ActualTemp_Heat", 8);
            SetColumnOrdinal(dataTable, "ResultTemp_Heat", 9);

            // Nhóm số 1: Time_1 và Temp_1
            SetColumnOrdinal(dataTable, "StandardTime_1", 10);
            SetColumnOrdinal(dataTable, "ActualTime_1", 11);
            SetColumnOrdinal(dataTable, "ResultTime_1", 12);
            SetColumnOrdinal(dataTable, "StandardTemp_1", 13);
            SetColumnOrdinal(dataTable, "ActualTemp_1", 14);
            SetColumnOrdinal(dataTable, "ResultTemp_1", 15);

            // Nhóm số 1: Chemical_1
            SetColumnOrdinal(dataTable, "StandardChemical_1", 16);
            SetColumnOrdinal(dataTable, "ActualChemical_1", 17);
            SetColumnOrdinal(dataTable, "ResultChemical_1", 18);

            // Nhóm số 2: Time_2 và Temp_2
            SetColumnOrdinal(dataTable, "StandardTime_2", 19);
            SetColumnOrdinal(dataTable, "ActualTime_2", 20);
            SetColumnOrdinal(dataTable, "ResultTime_2", 21);
            SetColumnOrdinal(dataTable, "StandardTemp_2", 22);
            SetColumnOrdinal(dataTable, "ActualTemp_2", 23);
            SetColumnOrdinal(dataTable, "ResultTemp_2", 24);

            // Nhóm số 2: Chemical_2
            SetColumnOrdinal(dataTable, "StandardChemical_2", 25);
            SetColumnOrdinal(dataTable, "ActualChemical_2", 26);
            SetColumnOrdinal(dataTable, "ResultChemical_2", 27);

            // Nhóm số 3: Time_3 và Temp_3
            SetColumnOrdinal(dataTable, "StandardTime_3", 28);
            SetColumnOrdinal(dataTable, "ActualTime_3", 29);
            SetColumnOrdinal(dataTable, "ResultTime_3", 30);
            SetColumnOrdinal(dataTable, "StandardTemp_3", 31);
            SetColumnOrdinal(dataTable, "ActualTemp_3", 32);
            SetColumnOrdinal(dataTable, "ResultTemp_3", 33);

            // Nhóm số 3: Chemical_3
            SetColumnOrdinal(dataTable, "StandardChemical_3", 34);
            SetColumnOrdinal(dataTable, "ActualChemical_3", 35);
            SetColumnOrdinal(dataTable, "ResultChemical_3", 36);

            // Cuối cùng nếu có LineID, cho vào cuối cùng (hoặc theo ý bạn)
            if (dataTable.Columns.Contains("LineID"))
            {
                SetColumnOrdinal(dataTable, "LineID", 37);
            }
        }
        private void SetColumnOrdinal(DataTable dataTable, string columnName, int newOrdinal)
        {
            DataColumn column = dataTable.Columns[columnName];

            // Kiểm tra nếu newOrdinal không lớn hơn số lượng cột và không âm
            if (newOrdinal >= 0 && newOrdinal < dataTable.Columns.Count)
            {
                column.SetOrdinal(newOrdinal);
            }
            else
            {
                // Xử lý khi newOrdinal không hợp lệ
                Console.WriteLine($"Invalid ordinal value: {newOrdinal}");
            }
        }
        private void SetColumnDisplayIndexes(DataGridView dataGridView)
        {
            SetColumnDisplayIndex(dataGridView, "LineName", 0);
            SetColumnDisplayIndex(dataGridView, "Model", 1);
            SetColumnDisplayIndex(dataGridView, "ArticleName", 2);
            SetColumnDisplayIndex(dataGridView, "PartName", 3);

            // Nhóm Temp_Heat
            SetColumnDisplayIndex(dataGridView, "StandardTemp_Heat", 4);
            SetColumnDisplayIndex(dataGridView, "ActualTemp_Heat", 5);
            SetColumnDisplayIndex(dataGridView, "ResultTemp_Heat", 6);

            // Nhóm Time_Heat
            SetColumnDisplayIndex(dataGridView, "StandardTime_Heat", 7);
            SetColumnDisplayIndex(dataGridView, "ActualTime_Heat", 8);
            SetColumnDisplayIndex(dataGridView, "ResultTime_Heat", 9);

            // Nhóm Temp_1
            SetColumnDisplayIndex(dataGridView, "StandardTemp_1", 10);
            SetColumnDisplayIndex(dataGridView, "ActualTemp_1", 11);
            SetColumnDisplayIndex(dataGridView, "ResultTemp_1", 12);

            // Nhóm Time_1
            SetColumnDisplayIndex(dataGridView, "StandardTime_1", 13);
            SetColumnDisplayIndex(dataGridView, "ActualTime_1", 14);
            SetColumnDisplayIndex(dataGridView, "ResultTime_1", 15);

            // Nhóm Chemical_1
            SetColumnDisplayIndex(dataGridView, "StandardChemical_1", 16);
            SetColumnDisplayIndex(dataGridView, "ActualChemical_1", 17);
            SetColumnDisplayIndex(dataGridView, "ResultChemical_1", 18);

            // Nhóm Temp_2
            SetColumnDisplayIndex(dataGridView, "StandardTemp_2", 19);
            SetColumnDisplayIndex(dataGridView, "ActualTemp_2", 20);
            SetColumnDisplayIndex(dataGridView, "ResultTemp_2", 21);

            // Nhóm Time_2
            SetColumnDisplayIndex(dataGridView, "StandardTime_2", 22);
            SetColumnDisplayIndex(dataGridView, "ActualTime_2", 23);
            SetColumnDisplayIndex(dataGridView, "ResultTime_2", 24);

            // Nhóm Chemical_2
            SetColumnDisplayIndex(dataGridView, "StandardChemical_2", 25);
            SetColumnDisplayIndex(dataGridView, "ActualChemical_2", 26);
            SetColumnDisplayIndex(dataGridView, "ResultChemical_2", 27);

            // Nhóm Temp_3
            SetColumnDisplayIndex(dataGridView, "StandardTemp_3", 28);
            SetColumnDisplayIndex(dataGridView, "ActualTemp_3", 29);
            SetColumnDisplayIndex(dataGridView, "ResultTemp_3", 30);

            // Nhóm Time_3
            SetColumnDisplayIndex(dataGridView, "StandardTime_3", 31);
            SetColumnDisplayIndex(dataGridView, "ActualTime_3", 32);
            SetColumnDisplayIndex(dataGridView, "ResultTime_3", 33);

            // Nhóm Chemical_3
            SetColumnDisplayIndex(dataGridView, "StandardChemical_3", 34);
            SetColumnDisplayIndex(dataGridView, "ActualChemical_3", 35);
            SetColumnDisplayIndex(dataGridView, "ResultChemical_3", 36);
        }

        private void SetColumnDisplayIndex(DataGridView dataGridView, string columnName, int newDisplayIndex)
        {
            if (dataGridView.Columns.Contains(columnName))
            {
                DataGridViewColumn column = dataGridView.Columns[columnName];
                column.DisplayIndex = newDisplayIndex;
            }

            if (dataGridView.Columns.Contains("FirstLetter"))
            {
                dataGridView.Columns["FirstLetter"].Visible = false;
            }

            // Ẩn cột LineNumber nếu tồn tại
            if (dataGridView.Columns.Contains("LineNumber"))
            {
                dataGridView.Columns["LineNumber"].Visible = false;
            }

        }

        private void LoadPlantNames()
        {
            List<string> plantNames = dbManager.GetPlantNames();
            for (int i = 0; i < plantNames.Count; i++)
            {
                plantNames[i] = "Xưởng " + plantNames[i];
            }

            plantNames.Sort();
        }

        private string CalculateTimeResult(List<BpfcDbContext.ResultViewModel> query, string lineName)
        {
            bool hasFail = false;
            bool hasPass = false;
            bool hasNull = false;

            foreach (var item in query)
            {
                if (item.LineName == lineName)
                {
                    for (int i = 1; i <= 3; i++)
                    {
                        string resultTimeColumnName = $"ResultTime_{i}";
                        string timeResult = item.GetType().GetProperty(resultTimeColumnName)?.GetValue(item, null)?.ToString(); // Sử dụng "?." để tránh lỗi null

                        if (timeResult == "FAIL")
                        {
                            hasFail = true;
                            break;
                        }
                        else if (timeResult == "PASS")
                        {
                            hasPass = true;
                        }
                        else if (string.IsNullOrEmpty(timeResult))
                        {
                            hasNull = true;
                        }
                    }
                }
            }

            if (hasFail)
            {
                return "FAIL"; // Nếu có ít nhất một cột thời gian là FAIL, trả về FAIL
            }
            else if (hasPass)
            {
                return "PASS"; // Nếu có ít nhất một cột thời gian là PASS hoặc tất cả đều NULL, trả về PASS
            }
            else if (hasNull)
            {
                return null; // Nếu có ít nhất một cột thời gian là NULL, các cột còn lại đều PASS, trả về NULL
            }
            else
            {
                return "PASS"; // Trường hợp còn lại, trả về PASS
            }
        }

        // Hàm kiểm tra kết quả nhiệt độ
        private string CalculateTempResult(List<BpfcDbContext.ResultViewModel> query, string lineName)
        {
            bool hasFail = false;
            bool hasPass = false;
            bool hasNull = false;

            foreach (var item in query)
            {
                if (item.LineName == lineName)
                {
                    for (int i = 1; i <= 3; i++)
                    {
                        string resultTempColumnName = $"ResultTemp_{i}";
                        string tempResult = item.GetType().GetProperty(resultTempColumnName)?.GetValue(item, null)?.ToString();

                        if (tempResult == "FAIL")
                        {
                            hasFail = true;
                            break;
                        }
                        else if (tempResult == "PASS")
                        {
                            hasPass = true;
                        }
                        else if (string.IsNullOrEmpty(tempResult))
                        {
                            hasNull = true;
                        }
                    }
                }
            }

            if (hasFail)
            {
                return "FAIL"; // Nếu có ít nhất một cột nhiệt độ là FAIL, trả về FAIL
            }
            else if (hasPass)
            {
                return "PASS"; // Nếu có ít nhất một cột nhiệt độ là PASS hoặc tất cả đều NULL, trả về PASS
            }
            else if (hasNull)
            {
                return null; // Nếu có ít nhất một cột nhiệt độ là NULL, các cột còn lại đều PASS, trả về NULL
            }
            else
            {
                return "PASS"; // Trường hợp còn lại, trả về PASS
            }
        }

        // Hàm kiểm tra kết quả hóa chất
        private string CalculateChemicalResult(List<BpfcDbContext.ResultViewModel> query, string lineName)
        {
            bool hasFail = false;
            bool hasPass = false;
            bool hasNull = false;

            foreach (var item in query)
            {
                if (item.LineName == lineName)
                {
                    for (int i = 1; i <= 3; i++)
                    {
                        string resultChemicalColumnName = $"ResultChemical_{i}";
                        string chemicalResult = item.GetType().GetProperty(resultChemicalColumnName)?.GetValue(item, null)?.ToString(); // Sử dụng "?." để tránh lỗi null

                        if (chemicalResult == "FAIL")
                        {
                            hasFail = true;
                            break; // Nếu có ít nhất một cột hóa chất là FAIL, thoát vòng lặp
                        }
                        else if (chemicalResult == "PASS")
                        {
                            hasPass = true;
                        }
                        else if (string.IsNullOrEmpty(chemicalResult)) // Kiểm tra null hoặc chuỗi rỗng
                        {
                            hasNull = true;
                        }
                    }
                }
            }

            if (hasFail)
            {
                return "FAIL";
            }
            else if (hasPass)
            {
                return "PASS"; // Nếu có ít nhất một cột hóa chất là PASS hoặc tất cả đều NULL, trả về PASS
            }
            else if (hasNull)
            {
                return null; // Nếu có ít nhất một cột hóa chất là NULL, các cột còn lại đều PASS, trả về NULL
            }
            else
            {
                return "PASS"; // Trường hợp còn lại, trả về PASS
            }
        }

        // Hàm so sánh kết quả cuối cùng theo LineName
        private string CompareFinalResultsByLineName(List<BpfcDbContext.ResultViewModel> query, string lineName)
        {
            bool hasFail = false;
            bool hasPass = false;
            bool hasNull = false;

            foreach (var item in query)
            {
                if (item.LineName == lineName)
                {
                    string finalTimeResult = CalculateTimeResult(query, lineName);
                    string finalTempResult = CalculateTempResult(query, lineName);
                    string finalChemicalResult = CalculateChemicalResult(query, lineName);

                    if (finalTimeResult == "FAIL" || finalTempResult == "FAIL" || finalChemicalResult == "FAIL")
                    {
                        hasFail = true;
                        break;
                    }
                    else if (finalTimeResult == "PASS" || finalTempResult == "PASS" || finalChemicalResult == "PASS")
                    {
                        hasPass = true;
                    }
                    else if (string.IsNullOrEmpty(finalTimeResult) && string.IsNullOrEmpty(finalTempResult) && string.IsNullOrEmpty(finalChemicalResult))
                    {
                        hasNull = true;
                    }
                }
            }

            if (hasFail)
            {
                return "FAIL"; // Nếu có ít nhất một dòng có kết quả cuối cùng là FAIL, trả về FAIL
            }
            else if (hasPass)
            {
                return "PASS"; // Nếu có ít nhất một dòng có kết quả cuối cùng là PASS hoặc tất cả đều NULL, trả về PASS
            }
            else if (hasNull)
            {
                return null; // Nếu có ít nhất một dòng có kết quả cuối cùng là NULL, các dòng còn lại đều PASS, trả về NULL
            }
            else
            {
                return "PASS"; // Trường hợp còn lại, trả về PASS
            }
        }
        private float CalculateCompliancePercentage(DataTable dataTable, string columnName)
        {
            if (!string.IsNullOrEmpty(columnName)) // Thêm kiểm tra tên cột
            {
                int passCount = CountPassInColumn(dataTable, columnName);
                int totalLineQuantity = CalculateTotalLineQuantity(dataTable, columnName);

                // Kiểm tra tránh chia cho 0
                if (totalLineQuantity == 0)
                {
                    return 0;
                }

                // Tính toán tỷ lệ tuân thủ
                float compliancePercentage = ((float)passCount / totalLineQuantity) * 100;

                return compliancePercentage;
            }
            else
            {
                // Xử lý trường hợp tên cột là null hoặc rỗng
                Console.WriteLine("Column name is null or empty.");
                return 0; // Hoặc giá trị mặc định khác tùy vào yêu cầu của bạn
            }
        }

        private int CountPassInColumn(DataTable dataTable, string columnName)
        {
            int passCount = 0;

            if (!string.IsNullOrEmpty(columnName)) // Thêm kiểm tra tên cột
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    if (row[columnName] != null && row[columnName].ToString().Equals("PASS", StringComparison.OrdinalIgnoreCase))
                    {
                        passCount++;
                    }
                }
            }

            return passCount;
        }

        private int CalculateTotalFail(DataTable dataTable, string columnName)
        {
            int failCount = 0;

            if (!string.IsNullOrEmpty(columnName)) // Thêm kiểm tra tên cột
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    if (row[columnName] != null && row[columnName].ToString().Equals("FAIL", StringComparison.OrdinalIgnoreCase))
                    {
                        failCount++;
                    }
                }
            }

            return failCount;
        }

        private int CalculateTotalLineQuantity(DataTable dataTable, string columnName)
        {
            int qtyCount = 0;

            if (!string.IsNullOrEmpty(columnName)) // Thêm kiểm tra tên cột
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    // Thay đổi điều kiện kiểm tra để đếm cả "PASS" và "FAIL"
                    if (row[columnName] != null && (row[columnName].ToString().Equals("FAIL", StringComparison.OrdinalIgnoreCase) || row[columnName].ToString().Equals("PASS", StringComparison.OrdinalIgnoreCase)))
                    {
                        qtyCount++;
                    }
                }
            }

            return qtyCount;
        }

        private void frmReport_Load(object sender, EventArgs e)
        {
            DailyPlantResults();
            ColorAlternate1RowPairs();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmHome formHome = new frmHome();
            formHome.Show();
        }
    }
}