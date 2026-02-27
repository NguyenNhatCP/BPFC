using DevExpress.XtraEditors;
using System;
using System.Data;
using System.Windows.Forms;
using ClosedXML.Excel;
using System.Collections.Generic;
using BPFC_System;
using System.Drawing;
using System.Configuration;
using static BPFC_System.frmBpfc;
using static BpfcDbContext;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Linq;

namespace BPFC_System
{
    public partial class frmRandomCheck : DevExpress.XtraEditors.XtraForm
    {
        private DataTable randomDataTable;
        private DataTable inputDataTable;
        private DatabaseManager dbManager;
        private readonly string connectionString;
        private int nextRowIndexToUpdate = 0;
        BpfcDbContext dbContext = new BpfcDbContext();

        public frmRandomCheck()
        {
            InitializeComponent();
            CreateRandomCheckDataTable();
            CreateInputDataTable();
            connectionString = ConfigHelper.GetConnectionString("strCon");
            dbManager = new DatabaseManager(connectionString);
            dtpDate.Format = DateTimePickerFormat.Custom;
            dtpDate.CustomFormat = "dd/MM/yyyy";
            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, dgvRandomCheck, new object[] { true });
            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, dgvInputData, new object[] { true });
            dtpDate.Value = DateTime.Now; 
        }
        private void UpdateMERecheckColumns()
        {
            // Tạo từ điển để lưu trữ giá trị ME Recheck cho từng máy
            var meRecheckValues = new Dictionary<string, (string Time, string Temperature)>();

            // Lấy dữ liệu từ dgvInputData
            foreach (DataGridViewRow inputRow in dgvInputData.Rows)
            {
                string machine = inputRow.Cells["Machine"].Value?.ToString();
                string time = inputRow.Cells["Thời gian\nTime"].Value?.ToString();
                string temperature = inputRow.Cells["Nhiệt độ\nTemperature"].Value?.ToString();

                if (!string.IsNullOrEmpty(temperature))
                {
                    temperature += "°C";
                }

                if (!string.IsNullOrEmpty(machine))
                {
                    meRecheckValues[machine] = (time, temperature);
                }
            }

            // Cập nhật dữ liệu vào dgvRandomCheck dựa trên Component (Outsole hoặc Upper)
            for (int i = nextRowIndexToUpdate; i < dgvRandomCheck.Rows.Count && i < nextRowIndexToUpdate + 2; i++)
            {
                DataGridViewRow randomRow = dgvRandomCheck.Rows[i];
                string component = randomRow.Cells["Component"].Value?.ToString().ToUpper();

                if (component == "OUTSOLE")
                {
                    if (meRecheckValues.TryGetValue("Máy 1 - Outsole", out var machine1Outsole))
                    {
                        if (string.IsNullOrEmpty(randomRow.Cells["ME Recheck Time 1"].Value?.ToString()) &&
                            string.IsNullOrEmpty(randomRow.Cells["ME Recheck Temp 1"].Value?.ToString()))
                        {
                            randomRow.Cells["ME Recheck Time 1"].Value = machine1Outsole.Time;
                            randomRow.Cells["ME Recheck Temp 1"].Value = machine1Outsole.Temperature;
                        }
                    }
                    if (meRecheckValues.TryGetValue("Máy 2 - Outsole", out var machine2Outsole))
                    {
                        if (string.IsNullOrEmpty(randomRow.Cells["ME Recheck Time 2"].Value?.ToString()) &&
                            string.IsNullOrEmpty(randomRow.Cells["ME Recheck Temp 2"].Value?.ToString()))
                        {
                            randomRow.Cells["ME Recheck Time 2"].Value = machine2Outsole.Time;
                            randomRow.Cells["ME Recheck Temp 2"].Value = machine2Outsole.Temperature;
                        }
                    }
                    if (meRecheckValues.TryGetValue("Máy 3 - Outsole", out var machine3Outsole))
                    {
                        if (string.IsNullOrEmpty(randomRow.Cells["ME Recheck Time 3"].Value?.ToString()) &&
                            string.IsNullOrEmpty(randomRow.Cells["ME Recheck Temp 3"].Value?.ToString()))
                        {
                            randomRow.Cells["ME Recheck Time 3"].Value = machine3Outsole.Time;
                            randomRow.Cells["ME Recheck Temp 3"].Value = machine3Outsole.Temperature;
                        }
                    }
                }
                else if (component == "UPPER")
                {
                    if (meRecheckValues.TryGetValue("Máy 1 - Upper", out var machine1Upper))
                    {
                        if (string.IsNullOrEmpty(randomRow.Cells["ME Recheck Time 1"].Value?.ToString()) &&
                            string.IsNullOrEmpty(randomRow.Cells["ME Recheck Temp 1"].Value?.ToString()))
                        {
                            randomRow.Cells["ME Recheck Time 1"].Value = machine1Upper.Time;
                            randomRow.Cells["ME Recheck Temp 1"].Value = machine1Upper.Temperature;
                        }
                    }
                    if (meRecheckValues.TryGetValue("Máy 2 - Upper", out var machine2Upper))
                    {
                        if (string.IsNullOrEmpty(randomRow.Cells["ME Recheck Time 2"].Value?.ToString()) &&
                            string.IsNullOrEmpty(randomRow.Cells["ME Recheck Temp 2"].Value?.ToString()))
                        {
                            randomRow.Cells["ME Recheck Time 2"].Value = machine2Upper.Time;
                            randomRow.Cells["ME Recheck Temp 2"].Value = machine2Upper.Temperature;
                        }
                    }
                    if (meRecheckValues.TryGetValue("Máy 3 - Upper", out var machine3Upper))
                    {
                        if (string.IsNullOrEmpty(randomRow.Cells["ME Recheck Time 3"].Value?.ToString()) &&
                            string.IsNullOrEmpty(randomRow.Cells["ME Recheck Temp 3"].Value?.ToString()))
                        {
                            randomRow.Cells["ME Recheck Time 3"].Value = machine3Upper.Time;
                            randomRow.Cells["ME Recheck Temp 3"].Value = machine3Upper.Temperature;
                        }
                    }
                }
            }

            // Cập nhật nextRowIndexToUpdate để chỉ vào 2 dòng tiếp theo
            nextRowIndexToUpdate += 2;

            // Xóa dữ liệu trong các cột Time, Temp và Chemical của dgvInputData
            foreach (DataGridViewRow inputRow in dgvInputData.Rows)
            {
                inputRow.Cells["Thời gian\nTime"].Value = null;
                inputRow.Cells["Nhiệt độ\nTemperature"].Value = null;
            }
        }
        private void btnInsert_Click(object sender, EventArgs e)
        {
            InsertToDataGridview();
        }
        private void InsertToDataGridview()
        {
            FillData();

            string articleName = txtArticle.Text;
            if (!string.IsNullOrEmpty(articleName))
            {
                if (cbxLine.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn dòng sản xuất.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                ArticleData articleData = dbManager.GetArticleData(articleName);
                if (articleData != null)
                {
                    FillDataFromArticleData(articleData);
                    FillActualChemicalData();
                    FillActualTimeAndTempData();
                    UpdateMERecheckColumns();
                    ProcessColumns(dgvRandomCheck);
                    cbxPlant.SelectedIndex = -1;
                    cbxLine.SelectedIndex = -1;
                    txtArticle.Clear();
                    cbxPlant.Focus();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy dữ liệu Article.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
        private void FillData()
        {
            DateTime selectedDate = dtpDate.Value;
            string selectedLine = cbxLine.SelectedItem?.ToString();
            string selectedArticle = txtArticle.Text;

            if (string.IsNullOrEmpty(selectedLine) || string.IsNullOrEmpty(selectedArticle))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Chỉ số cho component, bắt đầu từ "Outsole"
            string[] components = { "Outsole", "Upper" };
            int componentIndex = 0;

            for (int i = 0; i < randomDataTable.Rows.Count - 1; i += 2)
            {
                if (IsCellEmpty(dgvRandomCheck.Rows[i], "Date") &&
                    IsCellEmpty(dgvRandomCheck.Rows[i], "Line") &&
                    IsCellEmpty(dgvRandomCheck.Rows[i], "Article"))
                {
                    // Điền dữ liệu vào dòng thứ i
                    dgvRandomCheck.Rows[i].Cells["Date"].Value = selectedDate.ToString("dd/MM/yyyy");
                    dgvRandomCheck.Rows[i].Cells["Line"].Value = selectedLine;
                    dgvRandomCheck.Rows[i].Cells["Article"].Value = selectedArticle;
                    dgvRandomCheck.Rows[i].Cells["Component"].Value = components[componentIndex % 2];

                    // Nếu có dòng kế tiếp, điền dữ liệu vào dòng kế tiếp
                    if (i + 1 < randomDataTable.Rows.Count)
                    {
                        dgvRandomCheck.Rows[i + 1].Cells["Date"].Value = selectedDate.ToString("dd/MM/yyyy");
                        dgvRandomCheck.Rows[i + 1].Cells["Line"].Value = selectedLine;
                        dgvRandomCheck.Rows[i + 1].Cells["Article"].Value = selectedArticle;
                        dgvRandomCheck.Rows[i + 1].Cells["Component"].Value = components[(componentIndex + 1) % 2];
                    }

                    // Tăng componentIndex để thay đổi giữa "Outsole" và "Upper"
                    componentIndex += 2;

                    break;
                }
            }
        }

        // Phương thức kiểm tra ô có rỗng hay không
        private bool IsCellEmpty(DataGridViewRow row, string columnName)
        {
            return row.Cells[columnName].Value == null || string.IsNullOrEmpty(row.Cells[columnName].Value.ToString());
        }


        private void CreateInputDataTable()
        {
            inputDataTable = new DataTable("inputDataTable");
            inputDataTable.Columns.Add("ID");
            inputDataTable.Columns.Add("Machine");
            inputDataTable.Columns.Add("Thời gian\nTime");
            inputDataTable.Columns.Add("Nhiệt độ\nTemperature");
            dgvInputData.DataSource = inputDataTable;
            dgvInputData.RowHeadersVisible = false;
            // Tạo danh sách các giá trị cần thêm vào cột Machine
            List<string> machines = new List<string>
    {
        "Máy 1 - Outsole",
        "Máy 1 - Upper",
        "Máy 2 - Outsole",
        "Máy 2 - Upper",
        "Máy 3 - Outsole",
        "Máy 3 - Upper"
    };

            // Thêm các giá trị vào DataTable
            foreach (string machine in machines)
            {
                DataRow row = inputDataTable.NewRow();
                row["Machine"] = machine;
                inputDataTable.Rows.Add(row);
            }
            FormatDataGridView(dgvInputData);
            dgvInputData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvInputData.AllowUserToAddRows = false;
        }

        private void CreateRandomCheckDataTable()
        {
            // Khởi tạo DataTable và đặt tên các cột
            randomDataTable = new DataTable("randomDataTable");
            randomDataTable.Columns.Add("ID");
            randomDataTable.Columns.Add("Date");
            randomDataTable.Columns.Add("Line");
            randomDataTable.Columns.Add("Article");
            randomDataTable.Columns.Add("Model");
            randomDataTable.Columns.Add("Component");
            randomDataTable.Columns.Add("Standard Time 1");
            randomDataTable.Columns.Add("QIP Report Time 1");
            randomDataTable.Columns.Add("ME Recheck Time 1");
            randomDataTable.Columns.Add("Result Time 1");
            randomDataTable.Columns.Add("Standard Temp 1");
            randomDataTable.Columns.Add("QIP Report Temp 1");
            randomDataTable.Columns.Add("ME Recheck Temp 1");
            randomDataTable.Columns.Add("Result Temp 1");
            randomDataTable.Columns.Add("Standard Chemical 1");
            randomDataTable.Columns.Add("QIP Report Chemical 1");
            randomDataTable.Columns.Add("ME Recheck Chemical 1");
            randomDataTable.Columns.Add("Result Chemical 1");
            randomDataTable.Columns.Add("Standard Time 2");
            randomDataTable.Columns.Add("QIP Report Time 2");
            randomDataTable.Columns.Add("ME Recheck Time 2");
            randomDataTable.Columns.Add("Result Time 2");
            randomDataTable.Columns.Add("Standard Temp 2");
            randomDataTable.Columns.Add("QIP Report Temp 2");
            randomDataTable.Columns.Add("ME Recheck Temp 2");
            randomDataTable.Columns.Add("Result Temp 2");
            randomDataTable.Columns.Add("Standard Chemical 2");
            randomDataTable.Columns.Add("QIP Report Chemical 2");
            randomDataTable.Columns.Add("ME Recheck Chemical 2");
            randomDataTable.Columns.Add("Result Chemical 2");
            randomDataTable.Columns.Add("Standard Time 3");
            randomDataTable.Columns.Add("QIP Report Time 3");
            randomDataTable.Columns.Add("ME Recheck Time 3");
            randomDataTable.Columns.Add("Result Time 3");
            randomDataTable.Columns.Add("Standard Temp 3");
            randomDataTable.Columns.Add("QIP Report Temp 3");
            randomDataTable.Columns.Add("ME Recheck Temp 3");
            randomDataTable.Columns.Add("Result Temp 3");
            randomDataTable.Columns.Add("Standard Chemical 3");
            randomDataTable.Columns.Add("QIP Report Chemical 3");
            randomDataTable.Columns.Add("ME Recheck Chemical 3");
            randomDataTable.Columns.Add("Result Chemical 3");

            // Tạo 30 hàng và điền ID từ 1 đến 30
            for (int i = 0; i < 40; i++)
            {
                DataRow row = randomDataTable.NewRow();
                row["ID"] = i + 1; // Điền giá trị ID từ 1 đến 30
                randomDataTable.Rows.Add(row);
            }

            // Gán DataTable làm nguồn dữ liệu cho DataGridView
            dgvRandomCheck.DataSource = randomDataTable;
            FormatDataGridView(dgvRandomCheck);
            dgvRandomCheck.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            // Đặt lại định dạng cho cột Date
            dgvRandomCheck.Columns["Date"].DefaultCellStyle.Format = "dd/MM/yyyy";
        }
        private void txtArticle_TextChanged(object sender, EventArgs e)
        {
            txtArticle.Text = txtArticle.Text.ToUpper();
            txtArticle.SelectionStart = txtArticle.Text.Length;
        }

        private void dgvRandomCheck_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == dgvRandomCheck.Columns["Date"].Index && e.RowIndex >= 0)
            {
                if (e.Value != null && DateTime.TryParse(e.Value.ToString(), out DateTime dateValue))
                {
                    e.Value = dateValue.ToString("dd/MM/yyyy");
                    e.FormattingApplied = true;
                }
            }
        }
        private void dgvInputData_KeyDown(object sender, KeyEventArgs e)
        {
            // Kiểm tra nếu phím được nhấn là phím Enter
            if (e.KeyCode == Keys.Enter)
            {
                // Xác nhận rằng DataGridView đang có dòng được chọn
                if (dgvInputData.SelectedCells.Count > 0)
                {
                    // Gọi sự kiện Click của AcceptButton
                    AcceptButton.PerformClick();
                }
            }
        }

        private void FormatDataGridView(DataGridView dgv)
        {
            // Ẩn các cột có tên chứa "Result", "ME Recheck", và "ID"
            foreach (DataGridViewColumn column in dgv.Columns)
            {
                if (column.Name.Contains("ID"))
                {
                    column.Visible = false;
                }

                // Tắt sắp xếp khi ấn vào header của cột
                column.SortMode = DataGridViewColumnSortMode.NotSortable;

                // Căn lề giữa cho dữ liệu trong cột
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                // Đặt định dạng đậm cho tiêu đề
                column.HeaderCell.Style.Font = new System.Drawing.Font(dgv.Font, FontStyle.Bold);
            }

            // Tạo font từ Arial với kích thước mặc định
            System.Drawing.Font font = new System.Drawing.Font("Time New Roman", 9);

            // Đặt font cho header và nội dung của DataGridView
            dgv.ColumnHeadersDefaultCellStyle.Font = font;
            dgv.DefaultCellStyle.Font = font;
        }
        private void btnExport_ItemClick(object sender, EventArgs e)
        {
            string initialDirectory = @"S:\TEMP\ME";

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*",
                FileName = UniqueFileNameGenerator.GenerateFileName(),
                InitialDirectory = initialDirectory 
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;
                DataTable dt = GetDataTableFromDataGridView(dgvRandomCheck);
                ExcelExportRandomCheck exporter = new ExcelExportRandomCheck();
                exporter.SaveExcel(dt, filePath);
            }
        }
        private DataTable GetDataTableFromDataGridView(DataGridView dgv)
        {
            DataTable dt = new DataTable();

            // Thêm các cột từ DataGridView vào DataTable (loại bỏ cột "ID")
            foreach (DataGridViewColumn column in dgv.Columns)
            {
                if (column.Name != "ID") // Bỏ qua cột "ID"
                {
                    dt.Columns.Add(column.Name);
                }
            }

            // Thêm dữ liệu từ DataGridView vào DataTable
            foreach (DataGridViewRow row in dgv.Rows)
            {
                // Bỏ qua dòng mới được tạo tự động
                if (row.IsNewRow) continue;

                // Kiểm tra xem cột "Line" và "Article" có giá trị hay không
                if (!string.IsNullOrEmpty(row.Cells["Line"].Value?.ToString()) && !string.IsNullOrEmpty(row.Cells["Article"].Value?.ToString()))
                {
                    DataRow dataRow = dt.NewRow();
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        // Bỏ qua cột "ID"
                        if (cell.OwningColumn.Name != "ID")
                        {
                            dataRow[cell.OwningColumn.Name] = cell.Value?.ToString();
                        }
                    }
                    dt.Rows.Add(dataRow);
                }
            }

            return dt;
        }


        private void SetCellValueIfNotNull(DataGridViewCell cell, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                cell.Value = value;
            }
        }
        private void FillActualChemicalData()
        {
            string selectedLine = cbxLine.SelectedItem?.ToString();
            string selectedArticle = txtArticle.Text;
            DateTime selectedDate = dtpDate.Value;

            if (string.IsNullOrEmpty(selectedLine) || string.IsNullOrEmpty(selectedArticle))
            {
                MessageBox.Show("Vui lòng chọn chuyền và nhập Article.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            List<ResultViewModel> actualChemicalData = dbContext.GetAtucaChemicalForLineAndDate(selectedLine, selectedDate);

            if (actualChemicalData == null || actualChemicalData.Count == 0)
            {
                MessageBox.Show($"Không có dữ liệu hóa chất của Article {selectedArticle} ở chuyền {selectedLine} vào ngày {selectedDate}.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            foreach (var data in actualChemicalData)
            {
                if (data.ArticleName != selectedArticle || !(data.PartName == "Outsole" || data.PartName == "Upper") || data.LineName != selectedLine)
                {
                    continue;
                }

                foreach (DataGridViewRow row in dgvRandomCheck.Rows)
                {
                    if (row.Cells["Article"].Value?.ToString() == selectedArticle && row.Cells["Component"].Value?.ToString() == data.PartName && row.Cells["Line"].Value?.ToString() == selectedLine)
                    {
                        SetCellValueIfNotNull(row.Cells["QIP Report Chemical 1"], data.ActualChemical_1);
                        SetCellValueIfNotNull(row.Cells["QIP Report Chemical 2"], data.ActualChemical_2);
                        SetCellValueIfNotNull(row.Cells["QIP Report Chemical 3"], data.ActualChemical_3);
                    }
                }
            }
        }
        private void FillActualTimeAndTempData()
        {
            // Lấy thông tin từ các điều khiển trên giao diện
            DateTime selectedDate = dtpDate.Value;
            string selectedLine = cbxLine.SelectedItem?.ToString();
            string selectedArticle = txtArticle.Text;

            if (string.IsNullOrEmpty(selectedLine) || string.IsNullOrEmpty(selectedArticle))
            {
                MessageBox.Show("Vui lòng chọn chuyền và nhập Article.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Gọi phương thức từ dbContext để lấy dữ liệu Actual Time và Temp
            List<ResultViewModel> actualTimeAndTempData = dbContext.GetAtucalForLineAndDate(selectedLine, selectedDate);

            // Nếu không có dữ liệu trả về, hiển thị thông báo
            if (actualTimeAndTempData == null || actualTimeAndTempData.Count == 0)
            {
                MessageBox.Show($"Không có dữ liệu của Article {selectedArticle} ở chuyền {selectedLine} vào ngày {selectedDate}.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int filledRowCount = 0; // Biến đếm số lượng dòng đã điền thỏa mãn điều kiện

            // Duyệt ngược danh sách dữ liệu để xác định hai dòng cuối cùng thỏa mãn điều kiện
            for (int i = actualTimeAndTempData.Count - 1; i >= 0; i--)
            {
                var data = actualTimeAndTempData[i];

                // Chỉ điền dữ liệu nếu ArticleName khớp với selectedArticle và LineName khớp với selectedLine
                if (data.ArticleName == selectedArticle && data.LineName == selectedLine)
                {
                    // Tìm hàng tương ứng trong DataGridView dựa trên PartName
                    foreach (DataGridViewRow row in dgvRandomCheck.Rows)
                    {
                        // Kiểm tra nếu Line trên DataGridView trùng với LineName
                        if (row.Cells["Line"].Value?.ToString() == selectedLine &&
                            row.Cells["Article"].Value?.ToString() == data.ArticleName &&
                            row.Cells["Component"].Value?.ToString() == data.PartName)
                        {
                            // Điền dữ liệu Actual Time vào các cột QIP Report Time tương ứng
                            row.Cells["QIP Report Time 1"].Value = data.ActualTime_1;
                            row.Cells["QIP Report Time 2"].Value = data.ActualTime_2;
                            row.Cells["QIP Report Time 3"].Value = data.ActualTime_3;

                            // Điền dữ liệu Actual Temp vào các cột QIP Report Temp tương ứng, kèm theo dấu °C
                            row.Cells["QIP Report Temp 1"].Value = data.ActualTemp_1.HasValue ? data.ActualTemp_1.Value.ToString() + " °C" : "";
                            row.Cells["QIP Report Temp 2"].Value = data.ActualTemp_2.HasValue ? data.ActualTemp_2.Value.ToString() + " °C" : "";
                            row.Cells["QIP Report Temp 3"].Value = data.ActualTemp_3.HasValue ? data.ActualTemp_3.Value.ToString() + " °C" : "";

                            filledRowCount++; // Tăng biến đếm sau khi điền dữ liệu vào dòng

                            // Kiểm tra nếu đã điền vào 2 dòng thì thoát vòng lặp
                            if (filledRowCount >= 2)
                            {
                                return;
                            }
                        }
                    }
                }
            }
        }
        private void ProcessColumns(DataGridView dataGridView)
        {
            for (int i = 1; i <= 3; i++)
            {
                // Xử lý cột Time
                ProcessColumn(dataGridView, $"Standard Time {i}", $"ME Recheck Time {i}", $"Result Time {i}");

                // Xử lý cột Temp
                ProcessColumn(dataGridView, $"Standard Temp {i}", $"ME Recheck Temp {i}", $"Result Temp {i}");

                // Xử lý cột Chemical
                ProcessColumn(dataGridView, $"Standard Chemical {i}", $"ME Recheck Chemical {i}", $"Result Chemical {i}");
            }
        }

        private void ProcessColumn(DataGridView dataGridView, string standardColumnName, string meRecheckColumnName, string resultColumnName)
        {
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (!row.IsNewRow)
                {
                    DataGridViewCell stdCell = row.Cells[standardColumnName];
                    DataGridViewCell meRecheckCell = row.Cells[meRecheckColumnName];
                    DataGridViewCell resultCell = row.Cells[resultColumnName];

                    if (resultColumnName.Contains("Time"))
                    {
                        CompareTimeValues(meRecheckCell, stdCell, resultCell);
                    }
                    else if (resultColumnName.Contains("Temp"))
                    {
                        CompareTemperatureValues(meRecheckCell, stdCell, resultCell);
                    }
                    else if (resultColumnName.Contains("Chemical"))
                    {
                        CompareChemicalValues(meRecheckCell, stdCell, resultCell);
                    }
                }
            }
        }

        private void CompareChemicalValues(DataGridViewCell actualCell, DataGridViewCell stdCell, DataGridViewCell resultCell)
        {
            string actualValue = actualCell.Value?.ToString()?.Trim();
            string stdValue = stdCell.Value?.ToString()?.Trim();

            if (string.IsNullOrEmpty(actualValue) && string.IsNullOrEmpty(stdValue))
            {
                resultCell.Value = null; // Cả hai đều null hoặc trống, trả về null
            }
            else if (string.IsNullOrEmpty(actualValue) || string.IsNullOrEmpty(stdValue))
            {
                resultCell.Value = "FAIL"; // Một trong hai là null hoặc trống, trả về FAIL
            }
            else
            {
                if (string.Equals(actualValue.Replace(" ", ""), stdValue.Replace(" ", ""), StringComparison.OrdinalIgnoreCase))
                {
                    resultCell.Value = "PASS"; // So sánh chuỗi và trả về PASS nếu giống nhau
                    resultCell.Style.ForeColor = SystemColors.ControlText;
                }
                else
                {
                    resultCell.Value = "FAIL"; // Trả về FAIL nếu không giống nhau
                }
            }
        }

        private void CompareTimeValues(DataGridViewCell actualCell, DataGridViewCell stdCell, DataGridViewCell resultCell)
        {
            string actualValue = actualCell.Value?.ToString()?.Trim();
            string stdValue = stdCell.Value?.ToString()?.Trim();

            // Kiểm tra nếu cả hai giá trị đều là rỗng hoặc null
            if (string.IsNullOrEmpty(actualValue) && string.IsNullOrEmpty(stdValue))
            {
                resultCell.Value = null;
            }
            else if (string.IsNullOrEmpty(actualValue) || string.IsNullOrEmpty(stdValue))
            {
                resultCell.Value = "FAIL";
            }
            else
            {
                if (CompareTime(actualValue, stdValue))
                {
                    resultCell.Value = "PASS";
                    resultCell.Style.ForeColor = SystemColors.ControlText;
                }
                else
                {
                    resultCell.Value = "FAIL";
                }
            }
        }

        // Hàm so sánh thời gian
        private bool CompareTime(string actualValue, string stdValue)
        {
            // Kiểm tra định dạng của dữ liệu trong cột "Standard"
            if (Regex.IsMatch(stdValue, @"^(\d{1,2}:)?\d{1,2}:\d{2}-\d{1,2}:?\d{2}$"))
            {
                string[] stdParts = stdValue.Split('-');
                string stdStartValue = stdParts[0].Trim();
                string stdEndValue = stdParts[1].Trim();

                // So sánh giá trị thời gian
                TimeSpan actualTime;
                if (TimeSpan.TryParse(actualValue, out actualTime))
                {
                    TimeSpan startTime = TimeSpan.Parse(stdStartValue);
                    TimeSpan endTime = TimeSpan.Parse(stdEndValue);

                    return actualTime >= startTime && actualTime <= endTime;
                }
            }

            // Nếu dữ liệu không phù hợp, trả về false
            return false;
        }
        private void CompareTemperatureValues(DataGridViewCell actualCell, DataGridViewCell stdCell, DataGridViewCell resultCell)
        {
            string actualValue = actualCell.Value?.ToString()?.Trim();
            string stdValue = stdCell.Value?.ToString()?.Trim();

            // Kiểm tra nếu cả hai giá trị đều là rỗng hoặc null
            if (string.IsNullOrEmpty(actualValue) && string.IsNullOrEmpty(stdValue))
            {
                resultCell.Value = null; // Không thực hiện so sánh
                return;
            }
            else if (string.IsNullOrEmpty(actualValue) || string.IsNullOrEmpty(stdValue))
            {
                resultCell.Value = "FAIL"; // Nếu một trong hai giá trị là rỗng hoặc null, gán kết quả là FAIL
            }
            else
            {
                // Loại bỏ dấu "±5", "°C" trên giá trị tiêu chuẩn và "°C" ở giá trị thực tế
                stdValue = stdValue.Replace("±5", "").Replace("°C", "").Trim();
                actualValue = actualValue.Replace("°C", "").Trim();

                // Tiến hành phân tích giá trị tiêu chuẩn
                if (float.TryParse(stdValue, out float stdTemp) && float.TryParse(actualValue, out float actualTemp))
                {
                    // So sánh giá trị và gán PASS nếu thỏa mãn điều kiện
                    if (Math.Abs(actualTemp - stdTemp) <= 5)
                    {
                        resultCell.Value = "PASS";
                        resultCell.Style.ForeColor = SystemColors.ControlText;
                    }
                    else
                    {
                        resultCell.Value = "FAIL"; // Gán FAIL nếu không thỏa mãn điều kiện
                    }
                }
                else
                {
                    resultCell.Value = "FAIL"; // Trả về FAIL nếu không thể chuyển đổi giá trị sang kiểu float
                }
            }
        }

        private void FillDataFromArticleData(ArticleData articleData)
        {
            foreach (DataGridViewRow row in dgvRandomCheck.Rows)
            {
                if (row.Cells["Article"].Value != null && row.Cells["Article"].Value.ToString() == txtArticle.Text)
                {
                    if (row.Cells["Component"].Value != null)
                    {
                        string component = row.Cells["Component"].Value.ToString();

                        if (component == "Upper")
                        {
                            row.Cells["Model"].Value = articleData.Model;

                            row.Cells["Standard Time 1"].Value = articleData.Time1Upper;
                            SetCellFloatValueWithTemp(row.Cells["Standard Temp 1"], articleData.Temp1Upper);
                            row.Cells["Standard Chemical 1"].Value = articleData.Chemical1Upper;
                            row.Cells["ME Recheck Chemical 1"].Value = articleData.Chemical1Upper;

                            row.Cells["Standard Time 2"].Value = articleData.Time2Upper;
                            SetCellFloatValueWithTemp(row.Cells["Standard Temp 2"], articleData.Temp2Upper);
                            row.Cells["Standard Chemical 2"].Value = articleData.Chemical2Upper;
                            row.Cells["ME Recheck Chemical 2"].Value = articleData.Chemical2Upper;

                            row.Cells["Standard Time 3"].Value = articleData.Time3Upper;
                            SetCellFloatValueWithTemp(row.Cells["Standard Temp 3"], articleData.Temp3Upper);
                            row.Cells["Standard Chemical 3"].Value = articleData.Chemical3Upper;
                            row.Cells["ME Recheck Chemical 3"].Value = articleData.Chemical3Upper;
                        }
                        else if (component == "Outsole")
                        {
                            row.Cells["Model"].Value = articleData.Model;

                            row.Cells["Standard Time 1"].Value = articleData.Time1Outsole;
                            SetCellFloatValueWithTemp(row.Cells["Standard Temp 1"], articleData.Temp1Outsole);
                            row.Cells["Standard Chemical 1"].Value = articleData.Chemical1Outsole;
                            row.Cells["ME Recheck Chemical 1"].Value = articleData.Chemical1Outsole;

                            row.Cells["Standard Time 2"].Value = articleData.Time2Outsole;
                            SetCellFloatValueWithTemp(row.Cells["Standard Temp 2"], articleData.Temp2Outsole);
                            row.Cells["Standard Chemical 2"].Value = articleData.Chemical2Outsole;
                            row.Cells["ME Recheck Chemical 2"].Value = articleData.Chemical2Outsole;

                            row.Cells["Standard Time 3"].Value = articleData.Time3Outsole;
                            SetCellFloatValueWithTemp(row.Cells["Standard Temp 3"], articleData.Temp3Outsole);
                            row.Cells["Standard Chemical 3"].Value = articleData.Chemical3Outsole;
                            row.Cells["ME Recheck Chemical 3"].Value = articleData.Chemical3Outsole;
                        }
                    }
                }
            }
        }

        private void SetCellFloatValueWithTemp(DataGridViewCell cell, float? floatValue)
        {
            if (floatValue.HasValue)
            {
                cell.Value = floatValue.Value.ToString("0.##") + " ±5°C";
            }
            else
            {
                cell.Value = "";
            }
        }

        private void cbxPlant_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxPlant.SelectedItem != null)
            {
                string selectedPlant = cbxPlant.SelectedItem.ToString().Substring(6);

                if (!string.IsNullOrEmpty(selectedPlant))
                {
                    string plantName = selectedPlant.Replace("XƯỞNG ", "");

                    List<string> lines = dbManager.LoadProductionLines(plantName);

                    if (lines.Count > 0)
                    {
                        cbxLine.DataSource = lines;
                    }
                    else
                    {
                        MessageBox.Show("Không có Chuyền tương ứng với xưởng này.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void frmRandomCheck_Load(object sender, EventArgs e)
        {
            List<string> plantNames = dbManager.GetPlantNames();
            foreach (var plantName in plantNames)
            {
                cbxPlant.Items.Add("Xưởng " + plantName);
            }
            cbxPlant.Focus();
        }

        private void dgvRandomCheck_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // Lấy dòng được chọn từ RowHeader
                DataGridViewRow selectedRow = dgvRandomCheck.Rows[e.RowIndex];

                // Lấy Line và Article từ dòng được chọn
                string selectedLine = selectedRow.Cells["Line"].Value?.ToString();
                string selectedArticle = selectedRow.Cells["Article"].Value?.ToString();

                // Tìm dòng liền kề có Line và Article trùng khớp
                DataGridViewRow nextRow = FindNextRowWithSameLineAndArticle(selectedRow.Index, selectedLine, selectedArticle);

                if (nextRow != null)
                {
                    // Chọn hai dòng liền kề
                    selectedRow.Selected = true;
                    nextRow.Selected = true;
                }
            }
        }

        private DataGridViewRow FindNextRowWithSameLineAndArticle(int rowIndex, string line, string article)
        {
            for (int i = rowIndex + 1; i < dgvRandomCheck.Rows.Count; i++)
            {
                DataGridViewRow nextRow = dgvRandomCheck.Rows[i];

                // Kiểm tra Line và Article của dòng tiếp theo
                string nextLine = nextRow.Cells["Line"].Value?.ToString();
                string nextArticle = nextRow.Cells["Article"].Value?.ToString();

                if (nextLine == line && nextArticle == article)
                {
                    return nextRow;
                }
            }

            return null;
        }

        private void dgvInputData_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra xem cột được chỉnh sửa có phải là cột "Thời gian\nTime" không
            if (dgvInputData.Columns[e.ColumnIndex].Name == "Thời gian\nTime")
            {
                // Lấy giá trị trong ô chỉnh sửa
                object cellValue = dgvInputData.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                if (cellValue != null)
                {
                    // Chuyển đổi giá trị thành chuỗi
                    string timeValue = cellValue.ToString();

                    // Kiểm tra xem giá trị đã có dấu ":" chưa
                    if (!timeValue.Contains(":"))
                    {
                        // Nếu không có dấu ":" thì thêm vào ở vị trí thứ 2 từ trái sang
                        if (timeValue.Length >= 2)
                        {
                            timeValue = timeValue.Insert(1, ":");
                            if (timeValue.Count(c => c == ':') == 1)
                            {
                                int colonIndex = timeValue.IndexOf(':');
                                string hourPart = timeValue.Substring(0, colonIndex);
                                string minutePart = timeValue.Substring(colonIndex + 1);
                                // Xác định lại độ dài của giờ và phút, chỉ giữ lại 2 chữ số
                                hourPart = hourPart.Length > 2 ? hourPart.Substring(0, 2) : hourPart;
                                minutePart = minutePart.Length > 2 ? minutePart.Substring(0, 2) : minutePart;
                                // Ghép lại chuỗi giờ và phút
                                dgvInputData.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = hourPart + ":" + minutePart;
                            }
                        }
                    }
                }
            }
        }

        private void dgvInputData_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;

            // Kiểm tra xem cột hiện tại có phải là cột "Machine" hay không
            if (dgv.CurrentCell.ColumnIndex == dgv.Columns["Machine"].Index)
            {
                // Vô hiệu hóa chỉnh sửa trực tiếp trong ô nhập liệu của cột "Machine"
                e.Control.Enabled = false;
            }
            else
            {
                // Cho phép chỉnh sửa trực tiếp trong các ô nhập liệu của các cột khác
                e.Control.Enabled = true;
            }

            // Kiểm tra xem sự kiện được kích hoạt bởi việc ấn Tab hay không
            if (e.Control is TextBox)
            {
                TextBox textBox = e.Control as TextBox;

                // Ngăn chặn trỏ chuột dừng lại tại cột "Machine" khi ấn Tab
                textBox.TabStop = !(dgv.CurrentCell.ColumnIndex == dgv.Columns["Machine"].Index);
            }
        }

        private void dgvInputData_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;

            // Kiểm tra xem ô được chọn có thuộc cột "Machine" hay không
            if (dgv.Columns[e.ColumnIndex].Name == "Machine")
            {
                // Nếu ô được chọn thuộc cột "Machine", chuyển tiếp đến ô tiếp theo
                SendKeys.Send("{TAB}");
            }
        }
    }
}
