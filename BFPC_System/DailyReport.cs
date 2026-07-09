using BPFC_System;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Windows.Forms;
using System.Linq;
using DevExpress.XtraCharts;
using System.Drawing;
using static BpfcDbContext;
using System.ComponentModel;

namespace BPFC_System
{
    public partial class DailyReport : XtraUserControl
    {
        private readonly string connectionString;
        private readonly DatabaseManager dbManager;
        private readonly BpfcDbContext dbContext;
        private string lineName;

        public DailyReport()
        {
            InitializeComponent();
            connectionString = ConfigHelper.GetConnectionString("strCon");
            dbManager = new DatabaseManager(connectionString);
            dbContext = new BpfcDbContext(connectionString);
            LoadPlantNames();
            LoadResults();

            dgvDailyReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dtpReportDate.Value = DateTime.Now.AddDays(-1);
            dtpReportDate.MaxDate = DateTime.Now;
            dtpReportDate.CustomFormat = "dddd, dd/MM/yyyy";
            dtpReportDate.Format = DateTimePickerFormat.Custom;
            this.DoubleBuffered = true;
        }

        private BindingList<ResultViewModel> CreateBindingList(IEnumerable<ResultViewModel> query)
        {
            return new BindingList<ResultViewModel>(query.ToList());
        }

        private void LoadResultsByLineName(string lineName)
        {
            dgvLineDetail.Columns.Clear();

            if (lineName != null)
            {
                DateTime selectedDate = dtpReportDate.Value.Date;

                var query = dbContext.GetResults(selectedDate)
                                    .Where(result => result.LineName == lineName);

                BindingList<ResultViewModel> bindingList = CreateBindingList(query);

                // Tạo DataTable mới chỉ với các cột mong muốn
                DataTable plantDataTable = new DataTable();
                plantDataTable.Columns.Add("LineName", typeof(string));
                plantDataTable.Columns.Add("Model", typeof(string));
                plantDataTable.Columns.Add("ArticleName", typeof(string));
                plantDataTable.Columns.Add("PartName", typeof(string));

                for (int i = 1; i <= 3; i++)
                {
                    plantDataTable.Columns.Add($"StandardTemp_{i}", typeof(string));
                    plantDataTable.Columns.Add($"ActualTemp_{i}", typeof(string));
                    plantDataTable.Columns.Add($"ResultTemp_{i}", typeof(string));
                    plantDataTable.Columns.Add($"StandardTime_{i}", typeof(string));
                    plantDataTable.Columns.Add($"ActualTime_{i}", typeof(string));
                    plantDataTable.Columns.Add($"ResultTime_{i}", typeof(string));
                    plantDataTable.Columns.Add($"StandardChemical_{i}", typeof(string));
                    plantDataTable.Columns.Add($"ActualChemical_{i}", typeof(string));
                    plantDataTable.Columns.Add($"ResultChemical_{i}", typeof(string));
                }

                // Lọc dữ liệu từ query và thêm vào DataTable mới
                foreach (ResultViewModel item in query)
                {
                    if (item.LineName == lineName)
                    {
                        DataRow newRow = plantDataTable.NewRow();
                        newRow["LineName"] = item.LineName;
                        newRow["Model"] = item.Model;
                        newRow["ArticleName"] = item.ArticleName;
                        newRow["PartName"] = item.PartName;

                        for (int i = 1; i <= 3; i++)
                        {
                            string standardTempString = item.GetType().GetProperty($"StandardTemp_{i}")?.GetValue(item, null)?.ToString();

                            newRow[$"StandardTemp_{i}"] = !string.IsNullOrEmpty(standardTempString) ? standardTempString + " ±5°C" : null;

                            newRow[$"ActualTemp_{i}"] = item.GetType().GetProperty($"ActualTemp_{i}")?.GetValue(item, null) != null

                                ? item.GetType().GetProperty($"ActualTemp_{i}")?.GetValue(item, null).ToString() + "°C" : null;

                            newRow[$"ResultTemp_{i}"] = item.GetType().GetProperty($"ResultTemp_{i}")?.GetValue(item, null)?.ToString();
                            newRow[$"StandardTime_{i}"] = item.GetType().GetProperty($"StandardTime_{i}")?.GetValue(item, null)?.ToString();
                            newRow[$"ActualTime_{i}"] = item.GetType().GetProperty($"ActualTime_{i}")?.GetValue(item, null)?.ToString();
                            newRow[$"ResultTime_{i}"] = item.GetType().GetProperty($"ResultTime_{i}")?.GetValue(item, null)?.ToString();
                            newRow[$"StandardChemical_{i}"] = item.GetType().GetProperty($"StandardChemical_{i}")?.GetValue(item, null)?.ToString();
                            newRow[$"ActualChemical_{i}"] = item.GetType().GetProperty($"ActualChemical_{i}")?.GetValue(item, null)?.ToString();
                            newRow[$"ResultChemical_{i}"] = item.GetType().GetProperty($"ResultChemical_{i}")?.GetValue(item, null)?.ToString();
                        }

                        plantDataTable.Rows.Add(newRow);
                    }
                }

                dgvLineDetail.DataSource = plantDataTable;

                foreach (DataGridViewColumn column in dgvLineDetail.Columns)
                {
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
                }

                SetupDgvLineDetail();
            }
            else
            {
                dgvLineDetail.DataSource = null;
            }
        }

        private void SetupDgvLineDetail()
        {
            dgvLineDetail.RowHeadersVisible = false;
            // Kiểm tra xem có dữ liệu và các cột có giá trị không null
            if (dgvLineDetail.Rows.Count > 0 && dgvLineDetail.Columns.Count > 0)
            {
                dgvLineDetail.Columns["LineName"].HeaderText = "Line";
                dgvLineDetail.Columns["Model"].HeaderText = "Model";
                dgvLineDetail.Columns["ArticleName"].HeaderText = "Article";
                dgvLineDetail.Columns["PartName"].HeaderText = "Component";

                SetupColumnHeaderText("StandardTemp_1", "Temperature\nStandard 1");
                SetupColumnHeaderText("ActualTemp_1", "Temperature\nActual 1");
                SetupColumnHeaderText("ResultTemp_1", "Temperature\nResult 1");
                SetupColumnHeaderText("StandardTemp_2", "Temperature\nStandard 2");
                SetupColumnHeaderText("ActualTemp_2", "Temperature\nActual 2");
                SetupColumnHeaderText("ResultTemp_2", "Temperature\nResult 2");
                SetupColumnHeaderText("StandardTemp_3", "Temperature\nStandard 3");
                SetupColumnHeaderText("ActualTemp_3", "Temperature\nActual 3");
                SetupColumnHeaderText("ResultTemp_3", "Temperature\nResult 3");

                SetupColumnHeaderText("StandardTime_1", "Time\nStandard 1");
                SetupColumnHeaderText("ActualTime_1", "Time\nActual 1");
                SetupColumnHeaderText("ResultTime_1", "Time\nResult 1");
                SetupColumnHeaderText("StandardTime_2", "Time\nStandard 2");
                SetupColumnHeaderText("ActualTime_2", "Time\nActual 2");
                SetupColumnHeaderText("ResultTime_2", "Time\nResult 2");
                SetupColumnHeaderText("StandardTime_3", "Time\nStandard 3");
                SetupColumnHeaderText("ActualTime_3", "Time\nActual 3");
                SetupColumnHeaderText("ResultTime_3", "Time\nResult 3");

                SetupColumnHeaderText("StandardChemical_1", "Chemical\nStandard 1");
                SetupColumnHeaderText("ActualChemical_1", "Chemical\nActual 1");
                SetupColumnHeaderText("ResultChemical_1", "Chemical\nResult 1");
                SetupColumnHeaderText("StandardChemical_2", "Chemical\nStandard 2");
                SetupColumnHeaderText("ActualChemical_2", "Chemical\nActual 2");
                SetupColumnHeaderText("ResultChemical_2", "Chemical\nResult 2");
                SetupColumnHeaderText("StandardChemical_3", "Chemical\nStandard 3");
                SetupColumnHeaderText("ActualChemical_3", "Chemical\nActual 3");
                SetupColumnHeaderText("ResultChemical_3", "Chemical\nResult 3");

                dgvLineDetail.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgvLineDetail.BorderStyle = BorderStyle.Fixed3D;
                dgvLineDetail.DefaultCellStyle.Font = new Font("Times New Roman", 9);
                dgvLineDetail.ColumnHeadersDefaultCellStyle.Font = new Font("Time New Roman", 9);
                dgvLineDetail.RowTemplate.Height = 30;
                dgvLineDetail.ColumnHeadersDefaultCellStyle.Padding = new Padding(10, 5, 10, 5);
            }
            else
            {
                // Hiển thị thông báo hoặc thực hiện các xử lý khác khi không có dữ liệu
                MessageBox.Show("No data available.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void SetupColumnHeaderText(string columnName, string headerText)
        {
            if (dgvLineDetail.Columns.Contains(columnName))
            {
                dgvLineDetail.Columns[columnName].HeaderText = headerText;
            }
        }


        private void LoadPlantResults()
        {
            if (cbxPlantNames.SelectedItem != null)
            {
                string selectedPlantName = cbxPlantNames.SelectedItem.ToString().Substring(6);

                List<int> selectedLineIDs = dbManager.GetProductionLineIDs(selectedPlantName);

                if (selectedLineIDs.Count > 0)
                {
                    DateTime selectedDate = dtpReportDate.Value.Date;

                    List<string> allLineNames = dbManager.LoadProductionLines(selectedPlantName);

                    DataTable dataTable = new DataTable();

                    dataTable.Columns.Add("LineName", typeof(string));
                    dataTable.Columns.Add("FinalTimeResult", typeof(string));
                    dataTable.Columns.Add("FinalTempResult", typeof(string));
                    dataTable.Columns.Add("FinalChemicalResult", typeof(string));
                    dataTable.Columns.Add("LineResult", typeof(string));

                    foreach (string lineName in allLineNames)
                    {
                        DataRow newRow = dataTable.NewRow();
                        newRow["LineName"] = lineName;
                        dataTable.Rows.Add(newRow);
                    }

                    var query = dbContext.GetResults(selectedDate)
                        .Where(result => selectedLineIDs.Contains(result.LineID))
                        .ToList();

                    foreach (DataRow row in dataTable.Rows)
                    {
                        string lineName = row["LineName"].ToString();

                        row["FinalTimeResult"] = CalculateTimeResult(query, lineName);
                        row["FinalTempResult"] = CalculateTempResult(query, lineName);
                        row["FinalChemicalResult"] = CalculateChemicalResult(query, lineName);
                        row["LineResult"] = CompareFinalResultsByLineName(query, lineName);
                    }

                    dgvDailyReport.DataSource = dataTable;

                    dgvDailyReport.RowHeadersVisible = false;

                    dgvDailyReport.Columns["LineName"].HeaderText = "Line";
                    dgvDailyReport.Columns["FinalTimeResult"].HeaderText = "Thời gian";
                    dgvDailyReport.Columns["FinalTempResult"].HeaderText = "Nhiệt độ";
                    dgvDailyReport.Columns["FinalChemicalResult"].HeaderText = "Hóa chất";
                    dgvDailyReport.Columns["LineResult"].HeaderText = "Kết quả";

                    dgvDailyReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dgvDailyReport.BorderStyle = BorderStyle.Fixed3D;
                    dgvDailyReport.DefaultCellStyle.Font = new Font("Times New Roman", 9);
                    dgvDailyReport.ColumnHeadersDefaultCellStyle.Font = new Font("Time New Roman", 9);
                    dgvDailyReport.RowTemplate.Height = 30;
                    dgvDailyReport.ColumnHeadersDefaultCellStyle.Padding = new Padding(10, 5, 10, 5);

                    foreach (DataGridViewColumn column in dgvDailyReport.Columns)
                    {
                        column.SortMode = DataGridViewColumnSortMode.NotSortable;
                    }
                }
                else
                {
                    dgvDailyReport.DataSource = null;
                    MessageBox.Show("Không tìm thấy LineID nào cho xưởng đã chọn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                dgvDailyReport.DataSource = null;
                MessageBox.Show("Vui lòng chọn một xưởng để hiển thị dữ liệu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private double CalculatePassRate(DataTable dataTable)
        {
            int passCount = 0;
            int totalRowsWithLineName = 0;

            foreach (DataRow row in dataTable.Rows)
            {
                string lineResult = row["LineResult"] as string;
                string lineName = row["LineName"] as string;

                // Kiểm tra nếu LineResult chứa từ "PASS" và LineName không rỗng
                if (!string.IsNullOrEmpty(lineResult) && lineResult.IndexOf("PASS", StringComparison.OrdinalIgnoreCase) != -1 && !string.IsNullOrEmpty(lineName))
                {
                    passCount++;
                }

                // Kiểm tra nếu có dữ liệu ở cột LineName và không phải là NULL
                if (!string.IsNullOrEmpty(lineName))
                {
                    totalRowsWithLineName++;
                }
            }

            // Hiển thị thông tin trên console
            Console.WriteLine($"Total Rows with LineName: {totalRowsWithLineName}");
            Console.WriteLine($"Pass Count: {passCount}");

            // Tính tỷ lệ PASS
            if (totalRowsWithLineName > 0)
            {
                double passRate = (double)passCount / totalRowsWithLineName;
                Console.WriteLine($"Pass Rate: {passRate}");
                return passRate;
            }
            else
            {
                Console.WriteLine("Pass Rate: 0.0");
                return 0.0;
            }
        }
        private void LoadResults()
        {
            ChartControl chart = new ChartControl();

            // Create Series for the bar chart
            Series series = new Series("", ViewType.Bar);
            series.LegendTextPattern = "";

            foreach (var plantItem in cbxPlantNames.Items)
            {
                DataTable plantDataTable = new DataTable();

                // Add columns to DataTable
                plantDataTable.Columns.Add("LineID", typeof(int));
                plantDataTable.Columns.Add("LineName", typeof(string));
                plantDataTable.Columns.Add("FinalTimeResult", typeof(string));
                plantDataTable.Columns.Add("FinalTempResult", typeof(string));
                plantDataTable.Columns.Add("FinalChemicalResult", typeof(string));
                plantDataTable.Columns.Add("LineResult", typeof(string));

                string selectedPlantName = plantItem.ToString().Substring(6);

                List<int> selectedLineIDs = dbManager.GetProductionLineIDs(selectedPlantName);

                if (selectedLineIDs.Count > 0)
                {
                    List<string> allLineNames = dbManager.LoadProductionLines(selectedPlantName);

                    DateTime selectedDate = dtpReportDate.Value.Date;

                    // Add all LineNames to the DataTable
                    foreach (string lineName in allLineNames)
                    {
                        DataRow newRow = plantDataTable.NewRow();
                        newRow["LineName"] = lineName;
                        plantDataTable.Rows.Add(newRow);
                    }

                    // Query results for the selected date and lines
                    var query = dbContext.GetResults(selectedDate)
                        .ToList()
                        .Where(result => selectedLineIDs.Contains(result.LineID))
                        .ToList();

                    // Populate data in the DataTable
                    foreach (DataRow row in plantDataTable.Rows)
                    {
                        string lineName = row["LineName"].ToString();

                        row["LineID"] = query.FirstOrDefault(item => item.LineName == lineName)?.LineID ?? 0;
                        row["FinalTimeResult"] = CalculateTimeResult(query, lineName);
                        row["FinalTempResult"] = CalculateTempResult(query, lineName);
                        row["FinalChemicalResult"] = CalculateChemicalResult(query, lineName);
                        row["LineResult"] = CompareFinalResultsByLineName(query, lineName);
                    }

                    // Calculate pass rate for the current plant
                    double passRate = CalculatePassRate(plantDataTable);
                    int passRatePercentage = (int)(passRate * 100);

                    // Add data point to the series
                    series.Points.Add(new SeriesPoint(selectedPlantName.Replace("Xưởng", "").Trim(), passRatePercentage));

                    // Color the data point based on pass rate
                    foreach (SeriesPoint point in series.Points)
                    {
                        point.Color = (point.Values[0] < 100) ? Color.Red : Color.Green;
                    }
                }
                else
                {
                    dgvDailyReport.DataSource = null;
                    MessageBox.Show($"Không tìm thấy LineID nào cho xưởng {selectedPlantName}.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                // Add series to the chart
                chart.Series.Add(series);

                // Customize the appearance and layout of the chart
                ((XYDiagram)chart.Diagram).AxisY.Label.TextPattern = "{V}%";
                ((XYDiagram)chart.Diagram).AxisX.Title.Text = "Plant Names";
                ((XYDiagram)chart.Diagram).AxisY.Title.Text = "Pass Rate";
                ((XYDiagram)chart.Diagram).AxisY.WholeRange.MaxValue = 100;
                chart.Dock = DockStyle.Fill;

                // Clear existing controls in pnlChart and add the chart
                pnlChart.Controls.Clear();
                pnlChart.Controls.Add(chart);
            }
        }

        private void dgvDailyReport_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = dgvDailyReport.Rows[e.RowIndex];
                lineName = selectedRow.Cells["LineName"].Value.ToString();
                LoadResultsByLineName(lineName);
                SetupDgvLineDetail();
            }
        }

        private void cbxPlantNames_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadPlantResults();
            dgvLineDetail.DataSource = null;

            string firstFailLineName = FindFirstFailLineName(dgvDailyReport);

            if (!string.IsNullOrEmpty(firstFailLineName))
            {
                LoadResultsByLineName(firstFailLineName);
                SetupDgvLineDetail();
            }
        }

        private void dgvDailyReport_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                DataGridViewCell cell = dgvDailyReport.Rows[e.RowIndex].Cells[e.ColumnIndex];

                if (cell.Value != null && cell.Value.ToString() == "FAIL")
                {
                    cell.Style.BackColor = Color.LightSteelBlue;
                    cell.Style.ForeColor = Color.Red;
                }
                if (cell.Value != null && cell.Value.ToString() == "PASS")
                {
                    cell.Style.BackColor = Color.AliceBlue;
                }

            }
        }

        private void dgvLineDetail_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                DataGridViewCell cell = dgvLineDetail.Rows[e.RowIndex].Cells[e.ColumnIndex];

                if (cell.Value != null && cell.Value.ToString() == "FAIL")
                {
                    cell.Style.BackColor = Color.LightSteelBlue;
                    cell.Style.ForeColor = Color.Red;
                }
                if (cell.Value != null && cell.Value.ToString() == "PASS")
                {
                    cell.Style.BackColor = Color.AliceBlue;
                }
            }
        }

        // Hàm kiểm tra kết quả thời gian
        private string CalculateTimeResult(List<BpfcDbContext.ResultViewModel> query, string lineName)
        {
            bool hasFail = false;
            bool hasPass = false;
            bool hasNull = false;
            bool lineNameNotFound = true;

            foreach (var item in query)
            {
                if (item.LineName == lineName)
                {
                    lineNameNotFound = false; // Đánh dấu là đã tìm thấy LineName giống với lineName

                    for (int i = 1; i <= 3; i++)
                    {
                        // Kiểm tra Heat trước
                        if (item.ResultTime_Heat == "FAIL")
                        {
                            hasFail = true;
                            break;
                        }
                        else if (item.ResultTime_Heat == "PASS")
                        {
                            hasPass = true;
                        }
                        else if (string.IsNullOrEmpty(item.ResultTime_Heat))
                        {
                            hasNull = true;
                        }

                        // Kiểm tra Temp 1 -> 3
                        string resultTimeColumnName = $"ResultTime_{i}";
                        string timeResult = item.GetType().GetProperty(resultTimeColumnName)?.GetValue(item, null)?.ToString();

                        if (timeResult == "FAIL")
                        {
                            hasFail = true;
                            break; // Nếu có ít nhất một cột thời gian là FAIL, thoát vòng lặp
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

            // Kiểm tra xem có LineName giống với lineName không, nếu không thì trả về FAIL
            if (lineNameNotFound)
            {
                return null;
            }

            // Kiểm tra các điều kiện và trả về kết quả tương ứng
            if (hasFail)
            {
                return "FAIL";
            }
            else if (hasPass)
            {
                return "PASS";
            }
            else if (hasNull)
            {
                return "PASS";
            }
            else
            {
                return "Invalid";
            }
        }
        private string CalculateTempResult(List<BpfcDbContext.ResultViewModel> query, string lineName)
        {
            bool hasFail = false;
            bool hasPass = false;
            bool hasNull = false;
            bool lineNameNotFound = true;

            foreach (var item in query)
            {
                if (item.LineName == lineName)
                {
                    lineNameNotFound = false; // Đã tìm thấy LineName tương ứng
                    // Kiểm tra Heat trước
                    if (item.ResultTemp_Heat == "FAIL")
                    {
                        hasFail = true;
                        break;
                    }
                    else if (item.ResultTemp_Heat == "PASS")
                    {
                        hasPass = true;
                    }
                    else if (string.IsNullOrEmpty(item.ResultTemp_Heat))
                    {
                        hasNull = true;
                    }

                    // Kiểm tra Temp 1 -> 3
                    for (int i = 1; i <= 3; i++)
                    {
                        string resultTempColumnName = $"ResultTemp_{i}";
                        string tempResult = item.GetType().GetProperty(resultTempColumnName)?.GetValue(item, null)?.ToString();

                        if (tempResult == "FAIL")
                        {
                            hasFail = true;
                            break; // Nếu có ít nhất một cột nhiệt độ là FAIL, thoát vòng lặp
                        }
                        else if (tempResult == "PASS")
                        {
                            hasPass = true;
                        }
                        else if (string.IsNullOrEmpty(tempResult)) // Kiểm tra null hoặc chuỗi rỗng
                        {
                            hasNull = true;
                        }
                    }
                }
            }

            if (lineNameNotFound)
            {
                return null; // Nếu không tìm thấy LineName, trả về FAIL
            }
            else if (hasFail)
            {
                return "FAIL"; // Nếu có ít nhất một cột nhiệt độ là FAIL, trả về FAIL
            }
            else if (hasPass)
            {
                return "PASS"; // Nếu có ít nhất một cột nhiệt độ là PASS hoặc tất cả đều NULL, trả về PASS
            }
            else if (hasNull)
            {
                return "PASS"; // Nếu có ít nhất một cột nhiệt độ là NULL, các cột còn lại đều PASS, trả về PASS
            }
            else
            {
                return "invalid"; // Trường hợp còn lại, trả về Invalid
            }
        }

        // Hàm kiểm tra kết quả hóa chất
        private string CalculateChemicalResult(List<BpfcDbContext.ResultViewModel> query, string lineName)
        {
            bool hasFail = false;
            bool hasPass = false;
            bool hasNull = false;
            bool lineNameNotFound = true;

            foreach (var item in query)
            {
                if (item.LineName == lineName)
                {
                    lineNameNotFound = false; // Đã tìm thấy LineName tương ứng
                    
                    for (int i = 1; i <= 3; i++)
                    {
                        string resultChemicalColumnName = $"ResultChemical_{i}";
                        string chemicalResult = item.GetType().GetProperty(resultChemicalColumnName)?.GetValue(item, null)?.ToString();

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

            if (lineNameNotFound)
            {
                return null; // Nếu không tìm thấy LineName, trả về FAIL
            }
            else if (hasFail)
            {
                return "FAIL"; // Nếu có ít nhất một cột hóa chất là FAIL, trả về FAIL
            }
            else if (hasPass)
            {
                return "PASS"; // Nếu có ít nhất một cột hóa chất là PASS hoặc tất cả đều NULL, trả về PASS
            }
            else if (hasNull)
            {
                return "PASS"; // Nếu có ít nhất một cột hóa chất là NULL, các cột còn lại đều PASS, trả về PASS
            }
            else
            {
                return "Invalid"; // Trường hợp còn lại, trả về Invalid
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
                    else if ((!string.IsNullOrEmpty(finalTimeResult) || !string.IsNullOrEmpty(finalTempResult) || !string.IsNullOrEmpty(finalChemicalResult)) &&
                             (string.IsNullOrEmpty(finalTimeResult) || string.IsNullOrEmpty(finalTempResult) || string.IsNullOrEmpty(finalChemicalResult)))
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
                return "FAIL"; // Nếu có ít nhất một dòng có kết quả cuối cùng là NULL, các dòng còn lại đều PASS, trả về FAIL
            }
            else
            {
                return null; // Trường hợp còn lại, trả về NULL
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
            cbxPlantNames.Items.Clear();
            cbxPlantNames.Items.AddRange(plantNames.ToArray());
            cbxPlantNames.SelectedIndex = -1;
        }

        private string FindFirstFailLineName(DataGridView dataGridView)
        {
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (row.Cells["LineResult"].Value != null && row.Cells["LineResult"].Value.ToString() == "FAIL")
                {
                    return row.Cells["LineName"].Value.ToString();
                }
            }

            return null; 
        }
        private void dtpReportDate_ValueChanged(object sender, EventArgs e)
        {
            if (cbxPlantNames.Items.Count > 0)
            {
                cbxPlantNames.SelectedIndex = 0;
            }

            LoadResults();

            LoadPlantResults();

            string firstFailLineName = FindFirstFailLineName(dgvDailyReport);

            if (!string.IsNullOrEmpty(firstFailLineName))
            {   
                LoadResultsByLineName(firstFailLineName);
                SetupDgvLineDetail();
            }
            else
            {
                dgvLineDetail.DataSource = null;
            }
        }
    }
}
