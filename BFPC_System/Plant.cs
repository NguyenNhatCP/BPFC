using DevExpress.XtraEditors;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;
using static BPFC_System.frmBpfc;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Linq;
using BPFC_System;
using static BpfcDbContext;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.ExtendedProperties;
using DevExpress.XtraCharts.Design;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace BPFC_System
{
    public partial class frmPlant : XtraForm
    {
        private BufferedGraphicsContext bufferedGraphicsContext;
        private BufferedGraphics bufferedGraphics;
        private readonly string connectionString;
        private DateTime lastLogTime = DateTime.Now;
        BpfcDbContext dbContext = new BpfcDbContext();

        public frmPlant()
        {
            InitializeComponent();
            connectionString = ConfigHelper.GetConnectionString("strCon");
            DatabaseManager dbManager = new DatabaseManager(connectionString);

            DoubleBuffered = true;
            dtpDate.MaxDate = DateTime.Now;
            dtpDate.Value = DateTime.Now;
            DisplayArticleNames();

            EnableDoubleBufferingForControls(this); LookAndFeel.UseWindowsXPTheme = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);

            StartPeriodicTask();
            bufferedGraphicsContext = BufferedGraphicsManager.Current;
        }

        private async void StartPeriodicTask()
        {
            while (true)
            {
                // Gọi phương thức bạn muốn định kỳ
                DisplayArticleNames();

                await Task.Delay(60000);
            }
        }

        private void DisplayArticleNames()
        {
            lvArticleToCreate.Items.Clear();
            DatabaseManager dbManager = new DatabaseManager(connectionString);

            List<string> articleNames = dbManager.GetArticleNamesToCreate();

            // Hiển thị danh sách ArticleName lên ListView
            foreach (string articleName in articleNames)
            {
                ListViewItem item = new ListViewItem(articleName);
                lvArticleToCreate.Items.Add(item);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            bufferedGraphics = bufferedGraphicsContext.Allocate(this.CreateGraphics(), this.ClientRectangle);

            bufferedGraphics.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            this.Paint += frmPlant_Paint;
        }

        private void tableLayoutPanel1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            Control currentControl = ActiveForm.ActiveControl;

            if (currentControl != null && currentControl is TextBox)
            {
                int currentRow = tableLayoutPanel1.GetRow(currentControl);
                int currentCol = tableLayoutPanel1.GetColumn(currentControl);

                int newRow = currentRow;
                int newCol = currentCol;

                switch (e.KeyCode)
                {
                    case Keys.Up:
                        newRow = Math.Max(currentRow - 1, 0);
                        break;
                    case Keys.Down:
                        newRow = Math.Min(currentRow + 1, tableLayoutPanel1.RowCount - 1);
                        break;
                    case Keys.Left:
                        newCol = Math.Max(currentCol - 1, 0);
                        break;
                    case Keys.Right:
                        newCol = Math.Min(currentCol + 1, tableLayoutPanel1.ColumnCount - 1);
                        break;
                }

                if (newRow != currentRow || newCol != currentCol)
                {
                    MoveFocusToTextBox(newRow, newCol);
                    e.IsInputKey = true;
                }
            }
        }

        private void MoveFocusToTextBox(int row, int col)
        {
            Control newFocusedControl = tableLayoutPanel1.GetControlFromPosition(col, row);

            if (newFocusedControl != null && newFocusedControl is TextBox)
            {
                newFocusedControl.Focus();
            }
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

        private void frmPlant_Paint(object sender, PaintEventArgs e)
        {
            DrawBackground(bufferedGraphics.Graphics);

            bufferedGraphics.Render(e.Graphics);
        }

        private void DrawBackground(Graphics g)
        {
            // Tạo các màu cho gradient
            Color color1 = Color.Black; // Màu đen
            Color color2 = Color.Navy; // Màu navy
            Color color3 = Color.DarkBlue; // Màu xanh

            // Tạo một LinearGradientBrush
            using (LinearGradientBrush brush = new LinearGradientBrush(this.ClientRectangle, color1, color1, LinearGradientMode.Horizontal))
            {
                ColorBlend colorBlend = new ColorBlend();
                colorBlend.Colors = new Color[] { color1, color2, color3, color2 };
                colorBlend.Positions = new float[] { 0.0f, 0.4f, 0.6f, 1.0f };

                brush.InterpolationColors = colorBlend;

                g.FillRectangle(brush, this.ClientRectangle);
            }
        }

        private void ShowCenteredMessageBox(string message, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            Form currentForm = this;

            MessageBox.Show(currentForm, message, caption, buttons, icon);
        }

        private void cbxLines_SelectedIndexChanged(object sender, EventArgs e)
        {
            string lineName = cbxLines.Text;
            txtArticle.Text = "";

            DateTime selectedDate;

            if (togSwitch.IsOn)
            {
                // Nếu togSwitch được bật, sử dụng ngày được chọn ở dtpDate
                selectedDate = dtpDate.Value;
            }
            else if (dtpDate.Value.DayOfWeek == DayOfWeek.Monday)
            {
                // Nếu hôm nay là thứ 2, lấy 2 ngày trước
                selectedDate = dtpDate.Value.AddDays(-2);
            }
            else
            {
                // Ngược lại, lấy 1 ngày trước
                selectedDate = dtpDate.Value.AddDays(-1);
            }
            ClearTextBoxes();
            LoadDataFromDatabase(lineName, selectedDate);
            txtArticle.Focus();
        }

        private void ColorizeTextBoxes()
        {
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                if (control is TextBox textBox)
                {
                    if (togSwitch.IsOn) 
                    {
                        textBox.BackColor = Color.Wheat; 
                    }
                    else 
                    {
                        if (textBox.Name.StartsWith("txtActual"))
                        {
                            textBox.BackColor = SystemColors.Window; // Màu Window cho txtActual
                        }
                        else if (textBox.Name.StartsWith("txtStd") || textBox.Name.StartsWith("txtResult"))
                        {
                            textBox.BackColor = Color.Silver; // Màu Silver cho txtStd và txtResult
                        }
                    }
                }
            }
        }


        private void LoadDataFromDatabase(string lineName, DateTime selectedDate)
        {
            List<ResultViewModel> results = dbContext.GetAtucalForLineAndDate(lineName, selectedDate);
            DisplayData(results);
        }

        private void DisplayData(List<ResultViewModel> results)
        {
            if (results.Count > 0)
            {
                if (togSwitch.IsOn)
                {
                    DisplayActualData(results.Where(r => r.PartName == "Upper").FirstOrDefault(), "Upper");
                    DisplayActualData(results.Where(r => r.PartName == "Outsole").FirstOrDefault(), "Outsole");
                    if (txtArticle != null)
                    {
                        txtActualTemp1Upper_Leave(txtActualTemp1Upper, EventArgs.Empty);
                        txtActualTemp1Outsole_Leave(txtActualTemp1Outsole, EventArgs.Empty);
                        txtActualTemp2Upper_Leave(txtActualTemp2Upper, EventArgs.Empty);
                        txtActualTemp2Outsole_Leave(txtActualTemp2Outsole, EventArgs.Empty);
                        txtActualTemp3Upper_Leave(txtActualTemp3Upper, EventArgs.Empty);
                        txtActualTemp3Outsole_Leave(txtActualTemp3Outsole, EventArgs.Empty);
                        txtActualTime1Upper_Leave(txtActualTime1Upper, EventArgs.Empty);
                        txtActualTime1Outsole_Leave(txtActualTime1Outsole, EventArgs.Empty);
                        txtActualTime2Upper_Leave(txtActualTime2Upper, EventArgs.Empty);
                        txtActualTime2Outsole_Leave(txtActualTime2Outsole, EventArgs.Empty);
                        txtActualTime3Upper_Leave(txtActualTime3Upper, EventArgs.Empty);
                        txtActualTime3Outsole_Leave(txtActualTime3Outsole, EventArgs.Empty);
                    }
                }
                else
                {
                    DisplayActualData(results.Where(r => r.PartName == "Upper").FirstOrDefault(), "Upper");
                    DisplayActualData(results.Where(r => r.PartName == "Outsole").FirstOrDefault(), "Outsole");
                }
            }
        }

        private void DisplayActualData(ResultViewModel result, string partName)
        {
            if (result != null)
            {
                txtArticle.Text = result.ArticleName?.ToString();
                txtModel.Text = result.Model;

                if (partName != null)
                {
                    switch (partName)
                    {
                        case "Upper":
                            Debug.WriteLine("Displaying Upper data");

                            txtActualTemp1Upper.Text = (result.ActualTemp_1.HasValue && result.ActualTemp_1 != 0) ? result.ActualTemp_1.ToString() : string.Empty;
                            txtActualTemp2Upper.Text = (result.ActualTemp_2.HasValue && result.ActualTemp_2 != 0) ? result.ActualTemp_2.ToString() : string.Empty;
                            txtActualTemp3Upper.Text = (result.ActualTemp_3.HasValue && result.ActualTemp_3 != 0) ? result.ActualTemp_3.ToString() : string.Empty;

                            txtStdTemp1Upper.Text = result.StandardTemp_1 != null ? result.StandardTemp_1.ToString() + " ±5" : string.Empty;
                            txtStdTemp2Upper.Text = result.StandardTemp_2 != null ? result.StandardTemp_2.ToString() + " ±5" : string.Empty;
                            txtStdTemp3Upper.Text = result.StandardTemp_3 != null ? result.StandardTemp_3.ToString() + " ±5" : string.Empty;

                            txtActualTime1Upper.Text = result.ActualTime_1 ?? string.Empty;
                            txtActualTime2Upper.Text = result.ActualTime_2 ?? string.Empty;
                            txtActualTime3Upper.Text = result.ActualTime_3 ?? string.Empty;

                            txtStdTime1Upper.Text = result.StandardTime_1 != null ? result.StandardTime_1.ToString() : string.Empty;
                            txtStdTime2Upper.Text = result.StandardTime_2 != null ? result.StandardTime_2.ToString() : string.Empty;
                            txtStdTime3Upper.Text = result.StandardTime_3 != null ? result.StandardTime_3.ToString() : string.Empty;

                            break;

                        case "Outsole":
                            Debug.WriteLine("Displaying Outsole data");

                            txtActualTemp1Outsole.Text = (result.ActualTemp_1.HasValue && result.ActualTemp_1 != 0) ? result.ActualTemp_1.ToString() : string.Empty;
                            txtActualTemp2Outsole.Text = (result.ActualTemp_2.HasValue && result.ActualTemp_2 != 0) ? result.ActualTemp_2.ToString() : string.Empty;
                            txtActualTemp3Outsole.Text = (result.ActualTemp_3.HasValue && result.ActualTemp_3 != 0) ? result.ActualTemp_3.ToString() : string.Empty;

                            txtStdTemp1Outsole.Text = result.StandardTemp_1 != null ? result.StandardTemp_1.ToString() + " ±5" : string.Empty;
                            txtStdTemp2Outsole.Text = result.StandardTemp_2 != null ? result.StandardTemp_2.ToString() + " ±5" : string.Empty;
                            txtStdTemp3Outsole.Text = result.StandardTemp_3 != null ? result.StandardTemp_3.ToString() + " ±5" : string.Empty;

                            txtActualTime1Outsole.Text = result.ActualTime_1 ?? string.Empty;
                            txtActualTime2Outsole.Text = result.ActualTime_2 ?? string.Empty;
                            txtActualTime3Outsole.Text = result.ActualTime_3 ?? string.Empty;

                            txtStdTime1Outsole.Text = result.StandardTime_1 != null ? result.StandardTime_1.ToString() : string.Empty;
                            txtStdTime2Outsole.Text = result.StandardTime_2 != null ? result.StandardTime_2.ToString() : string.Empty;
                            txtStdTime3Outsole.Text = result.StandardTime_3 != null ? result.StandardTime_3.ToString() : string.Empty;

                            break;

                        default:
                            Debug.WriteLine("Unknown PartName");
                            break;
                    }
                }
                else
                {
                    Debug.WriteLine("PartName is null");
                }
            }
            else
            {
                Debug.WriteLine("Result is null");
            }
        }

        private void txtArticle_Leave(object sender, EventArgs e)
        {
            btnCheckArticle_Click(sender, e);
        }


        public string UserName { get; set; }
        private void frmPlant_Load(object sender, EventArgs e)
        {
            DatabaseManager dbManager = new DatabaseManager(connectionString);

            this.KeyPreview = true;

            string headerText = lblHeader.Text;

            DisplayActivity(headerText, DateTime.Now);

            string plantNameWithXuong = headerText.Replace("XƯỞNG", "").Trim();

            if (!string.IsNullOrEmpty(plantNameWithXuong))
            {
                string plantName = plantNameWithXuong.Replace("XƯỞNG ", "");

                List<string> lines = dbManager.LoadProductionLines(plantName);

                if (lines.Count > 0)
                {
                    cbxLines.DataSource = lines;
                 //   dbManager.SortProductionLines(cbxLines);
                }
                else
                {
                    ShowCenteredMessageBox("Không có Chuyền tương ứng với xưởng này.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            lblUserPlant.Text = Globals.Username;
        }



        private void btnCheckArticle_Click(object sender, EventArgs e)
        {
            string article = txtArticle.Text.Trim();

            if (!string.IsNullOrEmpty(article))
            {
                DatabaseManager dbManager = new DatabaseManager(connectionString);

                bool articleExists = dbManager.ArticleExists(article);

                if (articleExists)
                {
                    txtActualTemp1Outsole.Focus();
                    CheckAndDisplayStandardData(article);
                }
                else
                {
                    DialogResult result = MessageBox.Show("Article chưa được cập nhật tiêu chuẩn BPFC!\nBạn có muốn thêm nó vào danh sách?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        // Kiểm tra xem Article có tồn tại không trước khi lưu
                        if (!dbManager.ArticleExists(article))
                        {
                            dbManager.SaveArticleToCreate(article);
                            MessageBox.Show("Đã lưu vào danh sách!", "Thông báo", MessageBoxButtons.OK);
                            DisplayArticleNames();
                            txtArticle.Clear();
                        }
                        else
                        {
                            MessageBox.Show("Article đã tồn tại trong danh sách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
        }

        private void CheckAndDisplayStandardData(string article)
        {
            DatabaseManager dbManager = new DatabaseManager(connectionString);
            ArticleData articleData = dbManager.GetArticleData(article);

            if (articleData != null)
            {
                if (AreAllStandardValuesNullOrEmpty(articleData))
                {
                    DialogResult result = MessageBox.Show("Article chưa được cập nhật tiêu chuẩn BPFC!\nBạn có muốn thêm nó vào danh sách?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning); txtArticle.Clear();
                    if (result == DialogResult.Yes)
                    {
                        // Lưu Article vào cơ sở dữ liệu
                        string articleName = txtArticle.Text.Trim();

                        // Kiểm tra xem Article có tồn tại không trước khi lưu
                        if (!string.IsNullOrEmpty(articleName))
                        {
                            dbManager.SaveArticleToCreate(articleName);
                            MessageBox.Show("Đã lưu vào danh sách!", "Thông báo", MessageBoxButtons.OK);
                            DisplayArticleNames();
                            txtArticle.Clear();
                        }
                    }
                }
                DisplayArticleData(articleData);
            }
        }

        private bool AreAllStandardValuesNullOrEmpty(ArticleData articleData)
        {
            // Kiểm tra tất cả các thuộc tính có giá trị null hoặc không có giá trị
            bool temp1UpperNullOrEmpty = !articleData.Temp1Upper.HasValue;
            bool time1UpperNullOrEmpty = string.IsNullOrWhiteSpace(articleData.Time1Upper);
            bool temp2UpperNullOrEmpty = !articleData.Temp2Upper.HasValue;
            bool time2UpperNullOrEmpty = string.IsNullOrWhiteSpace(articleData.Time2Upper);
            bool temp3UpperNullOrEmpty = !articleData.Temp3Upper.HasValue;
            bool time3UpperNullOrEmpty = string.IsNullOrWhiteSpace(articleData.Time3Upper);
            bool temp1OutsoleNullOrEmpty = !articleData.Temp1Outsole.HasValue;
            bool time1OutsoleNullOrEmpty = string.IsNullOrWhiteSpace(articleData.Time1Outsole);
            bool temp2OutsoleNullOrEmpty = !articleData.Temp2Outsole.HasValue;
            bool time2OutsoleNullOrEmpty = string.IsNullOrWhiteSpace(articleData.Time2Outsole);
            bool temp3OutsoleNullOrEmpty = !articleData.Temp3Outsole.HasValue;
            bool time3OutsoleNullOrEmpty = string.IsNullOrWhiteSpace(articleData.Time3Outsole);

            // Kiểm tra xem tất cả các thuộc tính có giá trị null hoặc không có giá trị
            return temp1UpperNullOrEmpty && time1UpperNullOrEmpty && temp2UpperNullOrEmpty && time2UpperNullOrEmpty &&
                   temp3UpperNullOrEmpty && time3UpperNullOrEmpty && temp1OutsoleNullOrEmpty && time1OutsoleNullOrEmpty &&
                   temp2OutsoleNullOrEmpty && time2OutsoleNullOrEmpty && temp3OutsoleNullOrEmpty && time3OutsoleNullOrEmpty;
        }


        private void DisplayArticleData(ArticleData articleData)
        {
            lblCreatedAt.Text = $"Thời gian tạo: {articleData.CreatedAt}";
            lblCreatedBy.Text = $"BPFC được nhập bởi: {articleData.CreatedBy}";

            txtModel.Text = articleData.Model;

            txtStdTime1Upper.Text = articleData.Time1Upper;
            txtStdTimeHeat_Upper.Text = articleData.TimeHeatUpper;
            txtStdTime1Outsole.Text = articleData.Time1Outsole;

            txtStdTemp1Upper.Text = (articleData.Temp1Upper.HasValue && articleData.Temp1Upper.Value != 0)
                ? $"{articleData.Temp1Upper.Value} ±5" : "";

            txtStdTempHeat_Upper.Text = (articleData.TempHeatUpper.HasValue && articleData.TempHeatUpper.Value != 0)
                ? $"{articleData.TempHeatUpper.Value} ±5" : "";

            txtStdTemp2Upper.Text = (articleData.Temp2Upper.HasValue && articleData.Temp2Upper.Value != 0)
                ? $"{articleData.Temp2Upper.Value} ±5" : "";

            txtStdTime2Upper.Text = articleData.Time2Upper;

            txtStdTemp3Outsole.Text = (articleData.Temp3Outsole.HasValue && articleData.Temp3Outsole.Value != 0)
                ? $"{articleData.Temp3Outsole.Value} ±5" : "";

            txtStdTime3Upper.Text = articleData.Time3Upper;

            txtStdTemp1Outsole.Text = (articleData.Temp1Outsole.HasValue && articleData.Temp1Outsole.Value != 0)
                ? $"{articleData.Temp1Outsole.Value} ±5" : "";

            txtStdTemp2Outsole.Text = (articleData.Temp2Outsole.HasValue && articleData.Temp2Outsole.Value != 0)
                ? $"{articleData.Temp2Outsole.Value} ±5" : "";

            txtStdTime2Outsole.Text = articleData.Time2Outsole;

            txtStdTemp3Upper.Text = (articleData.Temp3Upper.HasValue && articleData.Temp3Upper.Value != 0)
                ? $"{articleData.Temp3Upper.Value} ±5" : "";

            txtStdTime3Outsole.Text = articleData.Time3Outsole;
        }
        private bool IsAnyResultTextBoxNull()
        {
            if (string.IsNullOrEmpty(txtResultTemp1Upper.Text) ||
                string.IsNullOrEmpty(txtResultTemp1Outsole.Text) ||
                string.IsNullOrEmpty(txtResultTemp2Upper.Text) ||
                string.IsNullOrEmpty(txtResultTemp2Outsole.Text) ||
                string.IsNullOrEmpty(txtResultTemp3Upper.Text) ||
                string.IsNullOrEmpty(txtResultTemp3Outsole.Text) ||
                string.IsNullOrEmpty(txtResultTime1Upper.Text) ||
                string.IsNullOrEmpty(txtResultTime1Outsole.Text) ||
                string.IsNullOrEmpty(txtResultTime2Upper.Text) ||
                string.IsNullOrEmpty(txtResultTime2Outsole.Text) ||
                string.IsNullOrEmpty(txtResultTime3Upper.Text) ||
                string.IsNullOrEmpty(txtResultTime3Outsole.Text))
            {
                return true;
            }

            return false;
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            btnSave.Focus();

            string articleName = txtArticle.Text.Trim();
            DatabaseManager dbManager = new DatabaseManager(connectionString);
            bool articleExists = dbManager.ArticleExists(articleName);

            string stdTime_Heat = txtStdTimeHeat_Upper.Text?.Trim() ?? string.Empty;
            string tempTextRaw = txtStdTempHeat_Upper.Text;

            float? stdTemp_Heat = null;

            // Nếu có nhập nhiệt độ thì xử lý
            if (!string.IsNullOrWhiteSpace(tempTextRaw))
            {
                // Lọc ký tự, chỉ giữ số, dấu chấm, dấu âm
                string tempText = Regex.Match(tempTextRaw, @"-?\d+(\.\d+)?").Value;

                if (string.IsNullOrWhiteSpace(tempText))
                {
                    MessageBox.Show("Không tìm thấy giá trị nhiệt độ hợp lệ. Vui lòng nhập số hoặc để trống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!float.TryParse(tempText, out float parsedTemp))
                {
                    MessageBox.Show("Giá trị nhiệt độ không hợp lệ. Vui lòng nhập số hoặc để trống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                stdTemp_Heat = parsedTemp;
            }

            // Truyền giá trị float? vào phương thức xử lý (cho phép null)
            dbManager.UpdateArticlePart_Heat(articleName, stdTemp_Heat, stdTime_Heat);
            if (IsAnyResultTextBoxNull())
            {
                if (articleExists)
                {
                    txtActualTemp1Upper_Leave(txtActualTemp1Upper, EventArgs.Empty);
                    txtActualTemp1Outsole_Leave(txtActualTemp1Outsole, EventArgs.Empty);
                    txtActualTemp2Upper_Leave(txtActualTemp2Upper, EventArgs.Empty);
                    txtActualTemp2Outsole_Leave(txtActualTemp2Outsole, EventArgs.Empty);
                    txtActualTemp3Upper_Leave(txtActualTemp3Upper, EventArgs.Empty);
                    txtActualTemp3Outsole_Leave(txtActualTemp3Outsole, EventArgs.Empty);
                    txtActualTime1Upper_Leave(txtActualTime1Upper, EventArgs.Empty);
                    txtActualTime1Outsole_Leave(txtActualTime1Outsole, EventArgs.Empty);
                    txtActualTime2Upper_Leave(txtActualTime2Upper, EventArgs.Empty);
                    txtActualTime2Outsole_Leave(txtActualTime2Outsole, EventArgs.Empty);
                    txtActualTime3Upper_Leave(txtActualTime3Upper, EventArgs.Empty);
                    txtActualTime3Outsole_Leave(txtActualTime3Outsole, EventArgs.Empty);
                }
            }

            if (string.IsNullOrEmpty(articleName))
            {
                ShowCenteredMessageBox("Vui lòng nhập Article!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtArticle.Focus();
                dbManager.HighlightTextBoxForShortDuration(txtArticle);
                return;
            }

            string[] emptyActualTextBoxes = GetEmptyActualTextBoxes();
            if (emptyActualTextBoxes.Length > 0)
            {
                string textBoxNames = string.Join("\n", emptyActualTextBoxes);
                DialogResult result = MessageBox.Show($"Giá trị thực tế ở ô:\n{textBoxNames} \nchưa được nhập.\nĐiều này sẽ dẫn đến Kết quả FAIL.\nBạn có chắc chắn bỏ qua không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    return;
                }
            }

            string lineName = cbxLines.SelectedItem?.ToString();
            DateTime selectedDate = dtpDate.Value.Date;
            string username = lblUserPlant.Text;
            int userID = dbManager.GetUserIdByUsername(username);
            int lineId = dbManager.GetLineID(lineName);
            var partIds = dbManager.GetArticlePartIDs(articleName);
            bool timeDataExists = dbManager.HasReachedDataLimitForTimeResults(lineId, selectedDate);
            bool tempDataExists = dbManager.HasReachedDataLimitForTempResults(lineId, selectedDate);


            if (AreAlltxtStandardNullOrEmpty())
            {
                DialogResult result = MessageBox.Show("Article chưa được cập nhật tiêu chuẩn BPFC!\nBạn có muốn thêm nó vào danh sách?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning); txtArticle.Clear();
                if (result == DialogResult.Yes)
                {
                    // Lưu Article vào cơ sở dữ liệu
                    string article = txtArticle.Text.Trim();

                    // Kiểm tra xem Article có tồn tại không trước khi lưu
                    if (!string.IsNullOrEmpty(article))
                    {
                        dbManager.SaveArticleToCreate(article);

                    }
                }
                return;
            }

            if (timeDataExists || tempDataExists)
            {
                DialogResult replaceResult = MessageBox.Show($"Đã có dữ liệu được {lineName} lưu vào ngày {selectedDate.ToString("dd/MM/yyyy")}. Bạn có muốn thay đổi không?", "Thay đổi dữ liệu", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (replaceResult == DialogResult.Yes)
                {
                    if (ContainsInvalidStdFormat(new Dictionary<string, string[]>
            {
                { "Outsole", new string[] { txtStdTime1Outsole.Text, txtStdTemp1Outsole.Text, txtStdTime2Outsole.Text, txtStdTemp2Outsole.Text, txtStdTime3Outsole.Text, txtStdTemp3Outsole.Text } },
                { "Upper", new string[] { txtStdTime1Upper.Text, txtStdTemp1Upper.Text, txtStdTime2Upper.Text, txtStdTemp2Upper.Text, txtStdTime3Upper.Text, txtStdTemp3Upper.Text } }
            }))
                    {
                        DialogResult results = MessageBox.Show("Article chưa được cập nhật tiêu chuẩn BPFC!\nBạn có muốn thêm nó vào danh sách?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning); txtArticle.Clear();
                        if (results == DialogResult.Yes)
                        {
                            if (!string.IsNullOrEmpty(articleName))
                            {
                                dbManager.SaveArticleToCreate(articleName);
                                MessageBox.Show("Đã lưu vào danh sách!", "Thông báo", MessageBoxButtons.OK);
                                txtArticle.Clear();
                            }
                        }
                        txtArticle.Clear();
                        return;
                    }

                    Dictionary<string, string[]> timeResults = new Dictionary<string, string[]>
            {
                { "Outsole", new string[] { GetNonEmptyText(txtActualTime1Outsole), GetNonEmptyText(txtStdTime1Outsole), GetNonEmptyText(txtResultTime1Outsole), GetNonEmptyText(txtActualTime2Outsole), GetNonEmptyText(txtStdTime2Outsole), GetNonEmptyText(txtResultTime2Outsole), GetNonEmptyText(txtActualTime3Outsole), GetNonEmptyText(txtStdTime3Outsole), GetNonEmptyText(txtResultTime3Outsole), null, null, null } },
                { "Upper", new string[] { GetNonEmptyText(txtActualTime1Upper), GetNonEmptyText(txtStdTime1Upper), GetNonEmptyText(txtResultTime1Upper), GetNonEmptyText(txtActualTime2Upper), GetNonEmptyText(txtStdTime2Upper), GetNonEmptyText(txtResultTime2Upper), GetNonEmptyText(txtActualTime3Upper), GetNonEmptyText(txtStdTime3Upper), GetNonEmptyText(txtResultTime3Upper), GetNonEmptyText(txtActualTimeHeat_Upper), GetNonEmptyText(txtStdTimeHeat_Upper), GetNonEmptyText(txtResultTimeHeat_Upper) } }
            };
                    Dictionary<string, string[]> tempResults = new Dictionary<string, string[]>
            {
                { "Outsole", new string[] { GetNonEmptyText(txtActualTemp1Outsole), GetNonEmptyText(txtStdTemp1Outsole), GetNonEmptyText(txtResultTemp1Outsole), GetNonEmptyText(txtActualTemp2Outsole), GetNonEmptyText(txtStdTemp2Outsole), GetNonEmptyText(txtResultTemp2Outsole), GetNonEmptyText(txtActualTemp3Outsole), GetNonEmptyText(txtStdTemp3Outsole), GetNonEmptyText(txtResultTemp3Outsole), null, null, null } },
                { "Upper", new string[] { GetNonEmptyText(txtActualTemp1Upper), GetNonEmptyText(txtStdTemp1Upper), GetNonEmptyText(txtResultTemp1Upper), GetNonEmptyText(txtActualTemp2Upper), GetNonEmptyText(txtStdTemp2Upper), GetNonEmptyText(txtResultTemp2Upper), GetNonEmptyText(txtActualTemp3Upper), GetNonEmptyText(txtStdTemp3Upper), GetNonEmptyText(txtResultTemp3Upper), GetNonEmptyText(txtActualTempHeat_Upper), GetNonEmptyText(txtStdTempHeat_Upper), GetNonEmptyText(txtResultTempHeat_Upper) } }
            };

                    if (timeDataExists)
                    {
                        dbManager.UpdateTimeResults(lineId, partIds, timeResults, selectedDate);
                    }

                    if (tempDataExists)
                    {
                        dbManager.UpdateTemperatureResults(lineId, partIds, tempResults, selectedDate);
                    }
                    string reportDate = selectedDate.ToString("dd-MM-yyyy");
                    string department = lblHeader.Text;
                    string result = DetermineOverallResult(
                        txtResultTime1Outsole, txtResultTime2Outsole, txtResultTime3Outsole,
                        txtResultTime1Upper, txtResultTime2Upper, txtResultTime3Upper,
                        txtResultTemp1Outsole, txtResultTemp2Outsole, txtResultTemp3Outsole,
                        txtResultTemp1Upper, txtResultTemp2Upper, txtResultTemp3Upper,
                        txtResultTempHeat_Upper, txtResultTimeHeat_Upper
                    );

                    dbManager.LogArticleActivity(username, reportDate, lineName, articleName, result, "Insert", department, selectedDate);

                    ShowCenteredMessageBox("Dữ liệu đã được cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    DisplayActivity(department, DateTime.Now);
                }
            }
            else
            {
                if (ContainsInvalidStdFormat(new Dictionary<string, string[]>
                {
                    { "Outsole", new string[] { txtStdTime1Outsole.Text, txtStdTemp1Outsole.Text, txtStdTime2Outsole.Text, txtStdTemp2Outsole.Text, txtStdTime3Outsole.Text, txtStdTemp3Outsole.Text } },
                    { "Upper", new string[] { txtStdTime1Upper.Text, txtStdTemp1Upper.Text, txtStdTime2Upper.Text, txtStdTemp2Upper.Text, txtStdTime3Upper.Text, txtStdTemp3Upper.Text } }
                }))
                {
                    DialogResult result = MessageBox.Show("Article chưa được cập nhật tiêu chuẩn BPFC!\nBạn có muốn thêm nó vào danh sách?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning); txtArticle.Clear();
                    if (result == DialogResult.Yes)
                    {
                        // Kiểm tra xem Article có tồn tại không trước khi lưu
                        if (!string.IsNullOrEmpty(articleName))
                        {
                            dbManager.SaveArticleToCreate(articleName);
                            MessageBox.Show("Đã lưu vào danh sách!", "Thông báo", MessageBoxButtons.OK);
                            txtArticle.Clear();
                        }
                    }
                    txtArticle.Clear();
                    return;
                }


                Dictionary<string, string[]> timeValues = new Dictionary<string, string[]>
            {
                { "Outsole", new string[] { GetNonEmptyText(txtActualTime1Outsole), GetNonEmptyText(txtStdTime1Outsole), GetNonEmptyText(txtResultTime1Outsole), GetNonEmptyText(txtActualTime2Outsole), GetNonEmptyText(txtStdTime2Outsole), GetNonEmptyText(txtResultTime2Outsole), GetNonEmptyText(txtActualTime3Outsole), GetNonEmptyText(txtStdTime3Outsole), GetNonEmptyText(txtResultTime3Outsole), null, null, null } },
                { "Upper", new string[] { GetNonEmptyText(txtActualTime1Upper), GetNonEmptyText(txtStdTime1Upper), GetNonEmptyText(txtResultTime1Upper), GetNonEmptyText(txtActualTime2Upper), GetNonEmptyText(txtStdTime2Upper), GetNonEmptyText(txtResultTime2Upper), GetNonEmptyText(txtActualTime3Upper), GetNonEmptyText(txtStdTime3Upper), GetNonEmptyText(txtResultTime3Upper) , GetNonEmptyText(txtActualTimeHeat_Upper), GetNonEmptyText(txtStdTimeHeat_Upper), GetNonEmptyText(txtResultTimeHeat_Upper) } }
            };

                Dictionary<string, string[]> tempValues = new Dictionary<string, string[]>
            {
                { "Outsole", new string[] { GetNonEmptyText(txtActualTemp1Outsole), GetNonEmptyText(txtStdTemp1Outsole), GetNonEmptyText(txtResultTemp1Outsole), GetNonEmptyText(txtActualTemp2Outsole), GetNonEmptyText(txtStdTemp2Outsole), GetNonEmptyText(txtResultTemp2Outsole), GetNonEmptyText(txtActualTemp3Outsole), GetNonEmptyText(txtStdTemp3Outsole), GetNonEmptyText(txtResultTemp3Outsole), null, null, null } },
                { "Upper", new string[] { GetNonEmptyText(txtActualTemp1Upper), GetNonEmptyText(txtStdTemp1Upper), GetNonEmptyText(txtResultTemp1Upper), GetNonEmptyText(txtActualTemp2Upper), GetNonEmptyText(txtStdTemp2Upper), GetNonEmptyText(txtResultTemp2Upper), GetNonEmptyText(txtActualTemp3Upper), GetNonEmptyText(txtStdTemp3Upper), GetNonEmptyText(txtResultTemp3Upper), GetNonEmptyText(txtActualTempHeat_Upper), GetNonEmptyText(txtStdTempHeat_Upper), GetNonEmptyText(txtResultTempHeat_Upper) } }
            };

                if (timeValues.Any(v => v.Value.Any(value => !string.IsNullOrEmpty(value))) || tempValues.Any(v => v.Value.Any(value => !string.IsNullOrEmpty(value))))
                {
                    dbManager.SaveAllResults(articleName, lineName, timeValues, tempValues, userID, selectedDate);

                    string reportDate = selectedDate.ToString("dd-MM-yyyy");
                    string department = lblHeader.Text;

                    string result = DetermineOverallResult(
                        txtResultTime1Outsole, txtResultTime2Outsole, txtResultTime3Outsole,
                        txtResultTime1Upper, txtResultTime2Upper, txtResultTime3Upper,
                        txtResultTemp1Outsole, txtResultTemp2Outsole, txtResultTemp3Outsole,
                        txtResultTemp1Upper, txtResultTemp2Upper, txtResultTemp3Upper,
                        txtResultTempHeat_Upper, txtResultTimeHeat_Upper
                    );

                    dbManager.LogArticleActivity(username, reportDate, lineName, articleName, result, "Insert", department, selectedDate);
                    DisplayActivity(department, DateTime.Now);

                    ShowCenteredMessageBox("Dữ liệu đã được lưu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else
                {
                    ShowCenteredMessageBox("Vui lòng nhập ít nhất một giá trị thời gian hoặc nhiệt độ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            bool lineExists = dbManager.CheckLineExists(lineName, selectedDate);

            if (lineExists)
            {
                dbManager.UpdateArticleNameForLine(articleName, lineName, selectedDate);
            }

            else
            {
                dbManager.InsertArticleOfLine(articleName, lineName, selectedDate);
            }

            if (!checkboxMultiArticles.Checked)
            {
                ClearTextBoxes();
                txtArticle.Clear();
            }

            int selectedIndex = cbxLines.SelectedIndex;

            if (selectedIndex < cbxLines.Items.Count - 1)
            {
                cbxLines.SelectedIndex = selectedIndex + 1;
            }
            else
            {
                string formattedDate = selectedDate.ToString("dd-MM-yyyy");
                cbxLines.SelectedIndex = 0;

                ClearTextBoxes();
                txtArticle.Clear();
                MessageBox.Show($"Đã nhập thông tin cho tất cả các chuyền trong ngày {formattedDate}!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            txtArticle.Focus();
            dbManager.HighlightTextBoxForShortDuration(txtArticle);
        }

        private void DisplayActivity(string department, DateTime timestamp)
        {
            DatabaseManager dbManager = new DatabaseManager(connectionString);

            // Lấy danh sách mô tả hoạt động từ cơ sở dữ liệu
            List<string> activityDescriptions = dbManager.GetActivity(department, timestamp);

            activityDescriptions.Reverse();
            // Hiển thị mô tả hoạt động trên txtActivityLog
            StringBuilder activityLog = new StringBuilder();

            foreach (string description in activityDescriptions)
            {
                // Tìm vị trí của chuỗi "PASS" hoặc "FAIL"
                int passIndex = description.IndexOf("PASS");
                int failIndex = description.IndexOf("FAIL");

                // Chọn vị trí đầu tiên xuất hiện giữa "PASS" và "FAIL"
                int resultIndex = passIndex != -1 && failIndex != -1 ? Math.Min(passIndex, failIndex) :
                                  passIndex != -1 ? passIndex :
                                  failIndex != -1 ? failIndex :
                                  -1;

                // Kiểm tra xem có chuỗi "PASS" hoặc "FAIL" không và lấy phần từ đầu đến hết chuỗi "PASS" hoặc "FAIL"
                string resultAndBefore = resultIndex != -1 ? description.Substring(0, resultIndex + 4).Trim() : description;

                activityLog.AppendLine(resultAndBefore);
                activityLog.AppendLine();
            }

            // Hiển thị kết quả lên txtActivityLog
            txtActivityLog.Text = activityLog.ToString();

            // Di chuyển con trỏ văn bản đến cuối chuỗi
            txtActivityLog.SelectionStart = txtActivityLog.Text.Length;

            // Đảm bảo dòng cuối cùng được hiển thị bằng cách cuộn đến con trỏ
            txtActivityLog.ScrollToCaret();
        }

        private string DetermineOverallResult(params TextBox[] resultTextBoxes)
        {
            // Check if any of the result TextBoxes contain "FAIL"
            foreach (var textBox in resultTextBoxes)
            {
                if (textBox.Text == "FAIL")
                {
                    return "FAIL";
                }
            }

            // If no "FAIL" is found, check if at least one "PASS" is found or all are null/empty
            foreach (var textBox in resultTextBoxes)
            {
                if (!string.IsNullOrEmpty(textBox.Text) && textBox.Text == "PASS")
                {
                    return "PASS";
                }
            }

            // If no "FAIL" and at least one "PASS" or all are null/empty, return "PASS"
            return "PASS";
        }


        private string GetNonEmptyText(TextBox textBox)
        {
            return string.IsNullOrEmpty(textBox.Text) ? null : textBox.Text;
        }

        private string[] GetEmptyActualTextBoxes()
        {
            (TextBox Std, TextBox Actual, string Name)[] textBoxPairs = new (TextBox, TextBox, string)[]
            {
        (txtStdTime1Outsole, txtActualTime1Outsole, "Thời gian máy 1 Outsole"),
        (txtStdTemp1Outsole, txtActualTemp1Outsole, "Nhiệt độ máy 1 Outsole"),
        (txtStdTime2Outsole, txtActualTime2Outsole, "Thời gian máy 2 Outsole"),
        (txtStdTemp2Outsole, txtActualTemp2Outsole, "Nhiệt độ máy 2 Outsole"),
        (txtStdTime3Outsole, txtActualTime3Outsole, "Thời gian máy 3 Outsole"),
        (txtStdTemp3Upper, txtActualTemp3Outsole, "Nhiệt độ máy 3 Outsole"),

        (txtStdTime1Upper, txtActualTime1Upper, "Thời gian máy 1 Upper"),
        (txtStdTemp1Upper, txtActualTemp1Upper, "Nhiệt độ máy 1 Upper"),
        (txtStdTime2Upper, txtActualTime2Upper, "Thời gian máy 2 Upper"),
        (txtStdTemp2Upper, txtActualTemp2Upper, "Nhiệt độ máy 2 Upper"),
        (txtStdTime3Upper, txtActualTime3Upper, "Thời gian máy 3 Upper"),
        (txtStdTemp3Outsole, txtActualTemp3Upper, "Nhiệt độ máy 3 Upper"),
            };

            List<string> emptyActualTextBoxes = new List<string>();

            foreach (var textBoxPair in textBoxPairs)
            {
                TextBox stdTextBox = textBoxPair.Std;
                TextBox actualTextBox = textBoxPair.Actual;

                if (!string.IsNullOrEmpty(stdTextBox.Text) && string.IsNullOrEmpty(actualTextBox.Text))
                {
                    emptyActualTextBoxes.Add(textBoxPair.Name);
                }
            }

            return emptyActualTextBoxes.ToArray(); 
        }


        private bool AreAlltxtStandardNullOrEmpty()
        {
            List<TextBox> standardTextBoxes = new List<TextBox>
    {
        txtStdTemp1Upper, txtStdTime1Upper, txtStdTemp2Upper, txtStdTime2Upper, txtStdTemp3Outsole, txtStdTime3Upper,
        txtStdTemp1Outsole, txtStdTime1Outsole, txtStdTemp2Outsole, txtStdTime2Outsole, txtStdTemp3Upper, txtStdTime3Outsole
    };


            foreach (var textBox in standardTextBoxes)
            {
                if (!string.IsNullOrWhiteSpace(textBox.Text))
                {
                    return false;
                }
            }

            return true;
        }

        private bool ContainsInvalidStdFormat(Dictionary<string, string[]> results)
        {
            foreach (var result in results)
            {
                if (results.ContainsKey(result.Key))
                {
                    foreach (var txtResult in result.Value)
                    {
                        if (txtResult.Contains("Invalid Format"))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }


        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams handleParam = base.CreateParams;
                handleParam.ExStyle |= 0x02000000;
                return handleParam;
            }
        }
        private void txtStdTempHeat_Upper_Leave(object sender, EventArgs e)
        {
            CompareAndSetResult(txtActualTempHeat_Upper, txtStdTempHeat_Upper, txtResultTempHeat_Upper, txtArticle, txtModel);
        }


        private void txtActualTemp1Upper_Leave(object sender, EventArgs e)
        {
            CompareAndSetResult(txtActualTemp1Upper, txtStdTemp1Upper, txtResultTemp1Upper, txtArticle, txtModel);
        }
        private void txtActualTempHeatUpper_Leave(object sender, EventArgs e)
        {
            CompareAndSetResult(txtActualTempHeat_Upper, txtStdTempHeat_Upper, txtResultTempHeat_Upper, txtArticle, txtModel);
        }

        private void txtActualTemp1Outsole_Leave(object sender, EventArgs e)
        {
            CompareAndSetResult(txtActualTemp1Outsole, txtStdTemp1Outsole, txtResultTemp1Outsole, txtArticle, txtModel);
        }

        private void txtActualTemp2Upper_Leave(object sender, EventArgs e)
        {
            CompareAndSetResult(txtActualTemp2Upper, txtStdTemp2Upper, txtResultTemp2Upper, txtArticle, txtModel);
        }

        private void txtActualTemp2Outsole_Leave(object sender, EventArgs e)
        {
            CompareAndSetResult(txtActualTemp2Outsole, txtStdTemp2Outsole, txtResultTemp2Outsole, txtArticle, txtModel);
        }

        private void txtActualTemp3Upper_Leave(object sender, EventArgs e)
        {
            CompareAndSetResult(txtActualTemp3Upper, txtStdTemp3Upper, txtResultTemp3Upper, txtArticle, txtModel);
        }

        private void txtActualTemp3Outsole_Leave(object sender, EventArgs e)
        {
            CompareAndSetResult(txtActualTemp3Outsole, txtStdTemp3Outsole, txtResultTemp3Outsole, txtArticle, txtModel);
        }


        private string CompareTemperatureValues(float? actualTemp, string stdValue)
        {
            if (actualTemp == null && string.IsNullOrEmpty(stdValue))
            {
                return null; // Cả hai đều null hoặc trống, trả về PASS (hoặc null nếu bạn dùng logic riêng)
            }
            else if (actualTemp == null)
            {
                return "FAIL"; // actualTemp null và stdValue không null, trả về FAIL
            }
            else
            {
                stdValue = stdValue?.Replace("±5", "").Trim();

                // Tiếp tục nếu stdValue còn nội dung
                if (!string.IsNullOrEmpty(stdValue) && float.TryParse(stdValue, out float stdTemp))
                {
                    int tolerance = 5; // Mặc định ±5
                    float minTemp = stdTemp - tolerance;
                    float maxTemp = stdTemp + tolerance;

                    if (actualTemp >= minTemp && actualTemp <= maxTemp)
                    {
                        return "PASS";
                    }
                }

                return "FAIL"; 
            }
        }

        private void CompareAndSetResult(TextBox actualTextBox, TextBox stdTextBox, TextBox resultTextBox, TextBox txtArticle, TextBox txtModel)
        {
            string actualText = actualTextBox.Text.Trim();
            string stdText = stdTextBox.Text.Trim();
            string articleText = txtArticle.Text.Trim();
            string modelText = txtModel.Text.Trim();

            if (string.IsNullOrEmpty(articleText) || string.IsNullOrEmpty(modelText))
            {
                resultTextBox.Text = string.Empty;
                return;
            }

            float? actualTemp = TryParseFloat(actualText);
            float? stdTemp = TryParseFloat(stdText);

            if (actualTemp == null)
            {
                resultTextBox.Text = CompareTemperatureValues(actualTemp, stdText);
                resultTextBox.ForeColor = Color.Silver;
            }
            else
            {
                resultTextBox.Text = CompareTemperatureValues(actualTemp, stdText);

                if (resultTextBox.Text == "FAIL")
                {
                    resultTextBox.ForeColor = Color.Red;
                }
                else
                {
                    resultTextBox.ForeColor = SystemColors.ControlText;
                }
            }
        }

        private float? TryParseFloat(string value)
        {
            if (float.TryParse(value, out float result))
            {
                return result;
            }
            return null;
        }

        private void txtResult_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (textBox.Text == "FAIL")
            {
                textBox.ForeColor = Color.Red;
            }
            else
            {
                textBox.ForeColor = SystemColors.ControlText;
            }
        }

        private void CompareTimeValues(TextBox txtActual, TextBox txtStd, TextBox txtResult)
        {
            string actualValue = txtActual.Text.Trim();
            string stdValue = txtStd.Text.Trim();
            string articleValue = txtArticle?.Text?.Trim();

            // Kiểm tra nếu cả hai giá trị đều là rỗng hoặc null
            if (string.IsNullOrEmpty(actualValue) && string.IsNullOrEmpty(stdValue))
            {
                txtResult.Text = null;
                txtResult.ForeColor = Color.Silver;
            }
            else if (string.IsNullOrEmpty(actualValue) || string.IsNullOrEmpty(stdValue))
            {
                txtResult.Text = "FAIL";
                txtResult.ForeColor = Color.Silver;
            }
            else
            {
                if (articleValue == null)
                {
                    return;
                }

                if (CompareTime(actualValue, stdValue))
                {
                    txtResult.Text = "PASS";
                    txtResult.ForeColor = SystemColors.ControlText; 
                }
                else
                {
                    txtResult.Text = "FAIL"; 
                    txtResult.ForeColor = Color.Red; 
                    txtResult.BackColor = Color.Silver; 
                }
            }
        }

        // Hàm so sánh thời gian
        private bool CompareTime(string actualValue, string stdValue)
        {
            // Kiểm tra định dạng của dữ liệu trong txtStd (m:ss-m:ss hoặc mm:ss-mm:ss)
            if (Regex.IsMatch(stdValue, @"^(\d{1,2}:)?\d{1,2}:\d{2}-\d{1,2}:?\d{2}$"))
            {
                string[] stdParts = stdValue.Split('-');
                string stdStartValue = stdParts[0];
                string stdEndValue = stdParts[1];

                if (!stdStartValue.Contains(":"))
                {
                    if (int.TryParse(stdStartValue, out int startSeconds) && startSeconds >= 0 && startSeconds <= 59)
                    {
                        stdStartValue = "00:" + startSeconds.ToString("D2");
                    }
                    else
                    {
                        DialogResult result = MessageBox.Show("Dữ liệu Tiêu chuẩn không hợp lệ!\nBạn có muốn thêm nó vào danh sách?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning); txtArticle.Clear();
                        if (result == DialogResult.Yes)
                        {
                            DatabaseManager dbManager = new DatabaseManager(connectionString);

                            string articleName = txtArticle.Text;
                            if (!string.IsNullOrEmpty(articleName))
                            {
                                dbManager.SaveArticleToCreate(articleName);
                                MessageBox.Show("Đã lưu vào danh sách!", "Thông báo", MessageBoxButtons.OK);
                                txtArticle.Clear();
                            }
                        }
                        txtArticle.Clear();
                        return false;
                    }
                }

                if (!stdEndValue.Contains(":"))
                {
                    if (int.TryParse(stdEndValue, out int endSeconds) && endSeconds >= 0 && endSeconds <= 59)
                    {
                        stdEndValue = "00:" + endSeconds.ToString("D2");
                    }
                    else
                    {
                        DialogResult result = MessageBox.Show("Dữ liệu Tiêu chuẩn không hợp lệ!\nBạn có muốn thêm nó vào danh sách?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning); txtArticle.Clear();
                        if (result == DialogResult.Yes)
                        {
                            DatabaseManager dbManager = new DatabaseManager(connectionString);

                            string articleName = txtArticle.Text;
                            if (!string.IsNullOrEmpty(articleName))
                            {
                                dbManager.SaveArticleToCreate(articleName);
                                MessageBox.Show("Đã lưu vào danh sách!", "Thông báo", MessageBoxButtons.OK);
                                txtArticle.Clear();
                            }
                        }
                        txtArticle.Clear();

                        return false;
                    }
                }

                // So sánh giá trị thời gian
                TimeSpan actualTime;

                if (Regex.IsMatch(actualValue, @"^(\d{1,2}:)?\d{1,2}:\d{2}$"))
                {
                    if (TimeSpan.TryParseExact(actualValue, @"m\:ss", CultureInfo.InvariantCulture, out actualTime))
                    {
                        if (actualTime.TotalMinutes >= 60)
                        {
                            ShowCenteredMessageBox("Thời gian thực tế không hợp lệ.\nHãy nhập lại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                    }
                    else
                    {
                        ShowCenteredMessageBox("Thời gian thực tế không hợp lệ.\nHãy nhập theo định dạng m:ss!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                else
                {
                    ShowCenteredMessageBox("Dữ liệu Thời gian thực tế không hợp lệ.\nHãy nhập theo định dạng m:ss!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                TimeSpan startTime = TimeSpan.ParseExact(stdStartValue, @"m\:ss", CultureInfo.InvariantCulture);
                TimeSpan endTime = TimeSpan.ParseExact(stdEndValue, @"m\:ss", CultureInfo.InvariantCulture);

                return actualTime >= startTime && actualTime <= endTime;
            }
            else
            {
                DialogResult result = MessageBox.Show("Dữ liệu Tiêu chuẩn không hợp lệ!\nBạn có muốn thêm nó vào danh sách?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning); txtArticle.Clear();
                if (result == DialogResult.Yes)
                {
                    DatabaseManager dbManager = new DatabaseManager(connectionString);

                    string articleName  = txtArticle.Text;
                    if (!string.IsNullOrEmpty(articleName))
                    {
                        dbManager.SaveArticleToCreate(articleName);
                        MessageBox.Show("Đã lưu vào danh sách!", "Thông báo", MessageBoxButtons.OK);
                        txtArticle.Clear();
                    }
                }
                txtArticle.Clear();
                return false;
            }
        }
        private void txtStdTimeHeat_Upper_Leave(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.Text = FormatTextBoxValue(textBox.Text);
            CompareTimeValues(txtActualTimeHeat_Upper, txtStdTimeHeat_Upper, txtResultTimeHeat_Upper);
        }

        private void txtActualTime1Upper_Leave(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.Text = FormatTextBoxValue(textBox.Text);
            CompareTimeValues(txtActualTime1Upper, txtStdTime1Upper, txtResultTime1Upper);
        }

        private void txtActualTime2Upper_Leave(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.Text = FormatTextBoxValue(textBox.Text);
            CompareTimeValues(txtActualTime2Upper, txtStdTime2Upper, txtResultTime2Upper);
        }

        private void txtActualTime3Upper_Leave(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.Text = FormatTextBoxValue(textBox.Text);
            CompareTimeValues(txtActualTime3Upper, txtStdTime3Upper, txtResultTime3Upper);
        }

        private void txtActualTime1Outsole_Leave(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.Text = FormatTextBoxValue(textBox.Text);
            CompareTimeValues(txtActualTime1Outsole, txtStdTime1Outsole, txtResultTime1Outsole);
        } 
        private void txtActualTimeHeatUpper_Leave(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.Text = FormatTextBoxValue(textBox.Text);
            CompareTimeValues(txtActualTimeHeat_Upper, txtStdTimeHeat_Upper, txtResultTimeHeat_Upper);
        }
        
        private void txtActualTime2Outsole_Leave(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.Text = FormatTextBoxValue(textBox.Text);
            CompareTimeValues(txtActualTime2Outsole, txtStdTime2Outsole, txtResultTime2Outsole);
        }

        private void txtActualTime3Outsole_Leave(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.Text = FormatTextBoxValue(textBox.Text);
            CompareTimeValues(txtActualTime3Outsole, txtStdTime3Outsole, txtResultTime3Outsole);
        }

        private void lblLogOut_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Hide();
                frmLogin formLogin = new frmLogin();    
                formLogin.Show();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ClearTextBoxes();
            txtArticle.Clear();
        }

        private void TextBox_Leave(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.Text = FormatTextBoxValue(textBox.Text);
        }

        private string FormatTextBoxValue(string input)
        {
            // Bỏ các khoảng trắng
            string trimmedText = input.Replace(" ", "");

            // Kiểm tra và thêm dấu ':' sau ký tự đầu tiên nếu chưa có
            if (!trimmedText.Contains(":"))
            {
                if (trimmedText.Length > 1)
                {
                    trimmedText = trimmedText.Insert(1, ":");
                }
                else if (trimmedText.Length == 1)
                {
                    trimmedText += ":00";
                }
            }

            // Tách phần trước và sau dấu ':'
            string[] parts = trimmedText.Split(':');

            if (parts.Length == 2)
            {
                string secondsPart = parts[1];

                // Kiểm tra nếu phần giây không hợp lệ hoặc rỗng, hiển thị thông báo và trả về giá trị ban đầu
                if (secondsPart.Length == 0 || !int.TryParse(secondsPart, out int seconds) || seconds < 0 || seconds > 59)
                {
                    return input; 
                }
                else if (secondsPart.Length == 1)
                {
                    secondsPart = "0" + secondsPart; 
                }

                trimmedText = parts[0] + ":" + secondsPart;
            }

            // Trả về giá trị đã được định dạng
            return trimmedText;
        }

        private void txtTemp_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.') && (e.KeyChar != '-') && (e.KeyChar != ':'))
            {
                e.Handled = true;
                errorToolTip.Show("Dữ liệu nhập vào không hợp lệ", textBox, 0, -textBox.Height, 2000);
            }

            if ((e.KeyChar == '.') && (textBox.Text.IndexOf('.') > -1))
            {
                e.Handled = true;
                errorToolTip.Show("Chỉ được nhập một dấu thập phân.", textBox, 0, -textBox.Height, 2000);
            }

            if ((e.KeyChar == '-') && textBox.SelectionStart != 0)
            {
                e.Handled = true;
                errorToolTip.Show("Dấu trừ chỉ được nhập ở vị trí đầu tiên.", textBox, 0, -textBox.Height, 2000);
            }

            if ((e.KeyChar == ':') && textBox.Text.IndexOf(':') > -1)
            {
                e.Handled = true;
                errorToolTip.Show("Chỉ được nhập một dấu hai chấm.", textBox, 0, -textBox.Height, 2000);
            }
        }

        private void ClearTextBoxes()
        {
            List<TextBox> textBoxesToClear = new List<TextBox>
            {
                txtModel,
                txtStdTemp1Upper,
                txtStdTemp2Upper,
                txtStdTemp3Outsole,
                txtActualTempHeat_Upper,
                txtStdTempHeat_Upper,
                txtResultTempHeat_Upper,
                txtStdTemp1Outsole,
                txtStdTemp2Outsole,
                txtStdTemp3Upper,
                txtStdTime1Upper,
                txtStdTime2Upper,
                txtStdTime3Upper,
                txtStdTime1Outsole,
                txtStdTime2Outsole,
                txtStdTime3Outsole,
                txtStdTime1Upper,
                txtStdTimeHeat_Upper,
                txtStdTime2Upper,
                txtStdTime3Upper,
                txtStdTime1Outsole,
                txtStdTime2Outsole,
                txtStdTime3Outsole,
                txtActualTemp1Upper,
                txtActualTemp2Upper,
                txtActualTemp3Upper,
                txtActualTemp1Outsole,
                txtActualTemp2Outsole,
                txtActualTemp3Outsole,
                txtActualTime1Upper,
                txtActualTimeHeat_Upper,
                txtActualTime2Upper,
                txtActualTime3Upper,
                txtActualTime1Outsole,
                txtActualTime2Outsole,
                txtActualTime3Outsole,
                txtResultTime1Upper,
                txtResultTimeHeat_Upper,
                txtResultTime2Upper,
                txtResultTime3Upper,
                txtResultTime1Outsole,
                txtResultTime2Outsole,
                txtResultTime3Outsole,
                txtResultTemp1Upper,
                txtResultTemp2Upper,
                txtResultTemp3Upper,
                txtResultTemp1Outsole,
                txtResultTemp1Outsole,
                txtResultTemp2Outsole,
                txtResultTemp3Outsole
            };
            foreach (var textBox in textBoxesToClear)
            {
                textBox.Clear();
            }

            lblCreatedAt.Text = "";
            lblCreatedBy.Text = "";
        }



        private void btnHome_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmHome formHome = new frmHome();
            formHome.Show();
        }

        private void frmPlant_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void dtpDate_ValueChanged(object sender, EventArgs e)
        {
            ClearTextBoxes();

            if (cbxLines.Items.Count > 0)
            {
                cbxLines.SelectedIndex = -1;
            }
            txtArticle.Clear();
            txtModel.Clear();
        }

        private void txtArticle_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.Text = textBox.Text.ToUpper();
            textBox.SelectionStart = textBox.Text.Length;

            if (!checkboxMultiArticles.Checked)
            {
                ClearTextBoxes() ;
            }
        }

        private void lblLogOut_MouseEnter(object sender, EventArgs e)
        {
            lblLogOut.ForeColor = System.Drawing.Color.NavajoWhite;
        }

        private void lblLogOut_MouseLeave(object sender, EventArgs e)
        {
            lblLogOut.ForeColor = System.Drawing.Color.DarkOrange;
        }

        private void togSwitch_Toggled(object sender, EventArgs e)
        {
            string lineName = cbxLines.Text;
            txtArticle.Text = "";
            ColorizeTextBoxes();

            DateTime selectedDate;

            if (togSwitch.IsOn)
            {
                // Nếu togSwitch được bật, sử dụng ngày được chọn ở dtpDate
                selectedDate = dtpDate.Value;
            }
            else if (dtpDate.Value.DayOfWeek == DayOfWeek.Monday)
            {
                // Nếu hôm nay là thứ 2, lấy 2 ngày trước
                selectedDate = dtpDate.Value.AddDays(-2);
            }
            else
            {
                // Ngược lại, lấy 1 ngày trước
                selectedDate = dtpDate.Value.AddDays(-1);
            }

            ClearTextBoxes();
            LoadDataFromDatabase(lineName, selectedDate);
        }

        private void lvArticleToCreate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvArticleToCreate.SelectedItems.Count > 0)
            {
                btnDeleteArt.Visible = true;
            }
            else
            {
                btnDeleteArt.Visible = false;
            }
        }

        private void btnDeleteArt_Click(object sender, EventArgs e)
        {
            DatabaseManager dbManager = new DatabaseManager(connectionString);

            if (lvArticleToCreate.SelectedItems.Count > 0)
            {
                // Lấy tên article từ thuộc tính Text của ListViewItem
                string articleName = lvArticleToCreate.SelectedItems[0].Text;

                dbManager.DeleteArticleToCreate(articleName);
                txtArticle.Clear();
                DisplayArticleNames();
                btnDeleteArt.Visible = false;
            }
        }

        private void txtArticle_Enter(object sender, EventArgs e)
        {
            string lineName = cbxLines.Text.Trim();
            if (string.IsNullOrEmpty(lineName))
            {
                ShowCenteredMessageBox("Vui lòng chọn chuyền!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);

                cbxLines.DroppedDown = true; 
                return;
            }
        }

        private void txtStdTempHeat_Upper_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            if (txt == null) return;

            // Nếu đã chứa "±5" thì bỏ ra để tránh lặp lại
            string rawText = txt.Text.Replace(" ±5", "").Trim();

            // Nếu rỗng thì không xử lý
            if (string.IsNullOrEmpty(rawText))
                return;

            // Nếu không phải số thì không xử lý
            if (!int.TryParse(rawText, out int tempValue))
                return;

            // Ngắt sự kiện để tránh vòng lặp vô hạn
            txtStdTempHeat_Upper.TextChanged -= txtStdTempHeat_Upper_TextChanged;

            // Gán lại giá trị với ±5
            txt.Text = $"{tempValue} ±5";

            // Đặt con trỏ về cuối chuỗi
            txt.SelectionStart = txt.Text.Length;

            // Gắn lại sự kiện
            txtStdTempHeat_Upper.TextChanged += txtStdTempHeat_Upper_TextChanged;
        }

        private void txtActualTempHeat_Upper_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtStdTempHeat_Upper_TextChanged_1(object sender, EventArgs e)
        {

        }
    }
}
