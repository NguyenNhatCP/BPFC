using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using static BPFC_System.frmBpfc;
using static BpfcDbContext;

namespace BPFC_System
{
    public partial class frmWarehouse : XtraForm
    {
        private readonly string connectionString;
        private DateTime lastLogTime = DateTime.Now;
        private Dictionary<TextBox, TextBox> actualToStdTextBoxMap = new Dictionary<TextBox, TextBox>();
        BpfcDbContext dbContext = new BpfcDbContext();
        private DatabaseManager dbManager;

        public frmWarehouse()
        {
            InitializeComponent();
            DoubleBuffered = true;
            InitializeWatermarkMappings();
            RegisterWatermarkEvents();
            dtpDate.MaxDate = DateTime.Now;
            dtpDate.Value = DateTime.Now;

            connectionString = ConfigHelper.GetConnectionString("strCon");
            dbManager = new DatabaseManager(connectionString);
            EnableDoubleBufferingForControls(this);

            LookAndFeel.UseWindowsXPTheme = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
        }


        /// <summary>
        /// Ánh xạ Watermark tương ứng vs các Textbox
        /// </summary>
        private void InitializeWatermarkMappings()
        {
            actualToStdTextBoxMap.Add(txtActualChemical1Outsole, txtStdChemical1Outsole);
            actualToStdTextBoxMap.Add(txtActualChemical1Upper, txtStdChemical1Upper);
            actualToStdTextBoxMap.Add(txtActualChemical2Outsole, txtStdChemical2Outsole);
            actualToStdTextBoxMap.Add(txtActualChemical2Upper, txtStdChemical2Upper);
            actualToStdTextBoxMap.Add(txtActualChemical3Outsole, txtStdChemical3Outsole);
            actualToStdTextBoxMap.Add(txtActualChemical3Upper, txtStdChemical3Upper);
        }

        /// <summary>
        /// Giảm flickering
        /// </summary>
        /// <param name="parentControl"></param>
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

        /// <summary>
        /// Kiểm tra null cho các textbox
        /// </summary>
        /// <returns></returns>
        private bool IsAnyResultTextBoxNull()
        {
            if (string.IsNullOrEmpty(txtResultChemical1Upper.Text) ||
                string.IsNullOrEmpty(txtResultChemical1Outsole.Text) ||
                string.IsNullOrEmpty(txtResultChemical2Upper.Text) ||
                string.IsNullOrEmpty(txtResultChemical2Outsole.Text) ||
                string.IsNullOrEmpty(txtResultChemical3Upper.Text) ||
                string.IsNullOrEmpty(txtResultChemical3Outsole.Text))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Tô màu cho các textbox
        /// </summary>
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

        /// <summary>
        /// Đọc dữ liệu từ cơ sở dữ liệu
        /// </summary>
        /// <param name="lineName"></param>
        /// <param name="selectedDate"></param>
        private void LoadDataFromDatabase(string lineName, DateTime selectedDate)
        {
            List<ResultViewModel> results = dbContext.GetAtucaChemicalForLineAndDate(lineName, selectedDate);
            DisplayData(results);
        }


        /// <summary>
        /// Hiển thị dữ liệu kết quả lên các textbox tương ứng khi Togle được bật (Trường hợp cần xem dữ liệu của ngày hôm trước)
        /// </summary>
        /// <param name="results"></param>
        private void DisplayData(List<ResultViewModel> results)
        {
            if (results.Count > 0)
            {
                if (togSwitch.IsOn)
                {
                    DisplayResultData(results.Where(r => r.PartName == "Upper").FirstOrDefault(), "Upper");
                    DisplayResultData(results.Where(r => r.PartName == "Outsole").FirstOrDefault(), "Outsole");
                    DisplayActualData(results.Where(r => r.PartName == "Upper").FirstOrDefault(), "Upper");
                    DisplayActualData(results.Where(r => r.PartName == "Outsole").FirstOrDefault(), "Outsole");
                }
                else
                {
                    DisplayActualData(results.Where(r => r.PartName == "Upper").FirstOrDefault(), "Upper");
                    DisplayActualData(results.Where(r => r.PartName == "Outsole").FirstOrDefault(), "Outsole");
                }
            }
        }


        /// <summary>
        /// Hiển thị dữ liệu kết quả lên các text box tương ứng
        /// </summary>
        /// <param name="result"></param>
        /// <param name="partName"></param>
        private void DisplayResultData(ResultViewModel result, string partName)
        {
            if (result != null)
            {
                txtArticle.Text = result.ArticleName?.ToString();
                txtModel.Text = result.Model?.ToString();

                if (partName != null)
                {
                    switch (partName)
                    {
                        case "Upper":
                            Debug.WriteLine("Displaying Upper data");

                            txtResultChemical1Upper.Text = result.ResultChemical_1?.ToString();
                            txtResultChemical2Upper.Text = result.ResultChemical_2?.ToString();
                            txtResultChemical3Upper.Text = result.ResultChemical_3?.ToString();

                            break;

                        case "Outsole":
                            Debug.WriteLine("Displaying Outsole data");

                            txtResultChemical1Outsole.Text = result.ResultChemical_1?.ToString();
                            txtResultChemical2Outsole.Text = result.ResultChemical_2?.ToString();
                            txtResultChemical3Outsole.Text = result.ResultChemical_3?.ToString();

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

        /// <summary>
        /// Hiển thị dữ liệu thực tế lên các textbox tương ứng
        /// </summary>
        /// <param name="result"></param>
        /// <param name="partName"></param>
        private void DisplayActualData(ResultViewModel result, string partName)
        {
            if (result != null)
            {
                txtArticle.Text = result.ArticleName?.ToString();
                txtModel.Text = result.Model?.ToString();

                if (partName != null)
                {
                    switch (partName)
                    {
                        case "Upper":
                            Debug.WriteLine("Displaying Upper data");

                            txtActualChemical1Upper.Text = result.ActualChemical_1 != null ? result.ActualChemical_1.ToString() : string.Empty;
                            txtActualChemical2Upper.Text = result.ActualChemical_2 != null ? result.ActualChemical_2.ToString() : string.Empty;
                            txtActualChemical3Upper.Text = result.ActualChemical_3 != null ? result.ActualChemical_3.ToString() : string.Empty;

                            txtStdChemical1Upper.Text = result.StandardChemical_1 != null ? result.StandardChemical_1.ToString() : string.Empty;
                            txtStdChemical2Upper.Text = result.StandardChemical_2 != null ? result.StandardChemical_2.ToString() : string.Empty;
                            txtStdChemical3Upper.Text = result.StandardChemical_3 != null ? result.StandardChemical_3.ToString() : string.Empty;

                            break;

                        case "Outsole":
                            Debug.WriteLine("Displaying Outsole data");

                            txtActualChemical1Outsole.Text = result.ActualChemical_1 != null ? result.ActualChemical_1.ToString() : string.Empty;
                            txtActualChemical2Outsole.Text = result.ActualChemical_2 != null ? result.ActualChemical_2.ToString() : string.Empty;
                            txtActualChemical3Outsole.Text = result.ActualChemical_3 != null ? result.ActualChemical_3.ToString() : string.Empty;

                            txtStdChemical1Outsole.Text = result.StandardChemical_1 != null ? result.StandardChemical_1.ToString() : string.Empty;
                            txtStdChemical2Outsole.Text = result.StandardChemical_2 != null ? result.StandardChemical_2.ToString() : string.Empty;
                            txtStdChemical3Outsole.Text = result.StandardChemical_3 != null ? result.StandardChemical_3.ToString() : string.Empty;

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            btnSave.Focus();
            string articleName = txtArticle.Text.Trim();
            DatabaseManager dbManager = new DatabaseManager(connectionString);

            if (string.IsNullOrEmpty(articleName))
            {
                MessageBox.Show("Chưa có Article!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (IsAnyResultTextBoxNull() && dbManager.ArticleExists(articleName))
            {
                TriggerLeaveEventsForActualTextBoxes();
            }

            if (CheckAndConfirmEmptyActualTextBoxes())
            {
                return;
            }

            string lineName = cbxLines.SelectedItem.ToString();
            string currentPlantName = cbxPlant.Text.Replace("Xưởng", "").Trim();
            DateTime selectedDate = dtpDate.Value.Date;
            string username = lblUserWH.Text;
            int plantId = dbManager.GetPlantID(currentPlantName);
            int lineId = dbManager.GetLineID(lineName, plantId);
            var partIds = dbManager.GetArticlePartIDs(articleName);
            var chemicalResults = GetChemicalResults();

            if (dbManager.HasReachedDataLimitForChemicalresults(lineId, selectedDate))
            {
                if (MessageBox.Show($"Đã có dữ liệu cho {lineName} vào ngày {selectedDate:dddd, dd/MM/yyyy}. Bạn có muốn thay đổi không?", "Thay đổi dữ liệu", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    dbManager.UpdateChemicalResults(lineId, partIds, chemicalResults, selectedDate);
                    LogActivity("Update", username, selectedDate, lineName, articleName, "Warehouse");
                    MessageBox.Show($"Dữ liệu {lineName} đã được cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                dbManager.SaveChemicalResults(lineId, partIds, chemicalResults, selectedDate);
                LogActivity("Insert", username, selectedDate, lineName, articleName, "Warehouse");
                MessageBox.Show("Dữ liệu đã được lưu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            ClearTextBoxes();
            MoveToNextLineOrPlant(selectedDate);
        }

        private Dictionary<string, string[]> GetChemicalResults()
        {
            return new Dictionary<string, string[]>
    {
        {
            "Outsole", new string[]
            {
                txtActualChemical1Outsole.Text, txtStdChemical1Outsole.Text, txtResultChemical1Outsole.Text,
                txtActualChemical2Outsole.Text, txtStdChemical2Outsole.Text, txtResultChemical2Outsole.Text,
                txtActualChemical3Outsole.Text, txtStdChemical3Outsole.Text, txtResultChemical3Outsole.Text
            }
        },
        {
            "Upper", new string[]
            {
                txtActualChemical1Upper.Text, txtStdChemical1Upper.Text, txtResultChemical1Upper.Text,
                txtActualChemical2Upper.Text, txtStdChemical2Upper.Text, txtResultChemical2Upper.Text,
                txtActualChemical3Upper.Text, txtStdChemical3Upper.Text, txtResultChemical3Upper.Text
            }
        }
    };
        }

        private void LogActivity(string action, string username, DateTime selectedDate, string lineName, string articleName, string department)
        {
            string reportDate = selectedDate.ToString("dd-MM-yyyy");
            string result = DetermineOverallResult(
                txtResultChemical1Outsole, txtResultChemical2Outsole, txtResultChemical3Outsole,
                txtResultChemical1Upper, txtResultChemical2Upper, txtResultChemical3Upper
            );
            dbManager.LogArticleActivity(username, reportDate, lineName, articleName, result, action, department, selectedDate);
            DisplayActivity(department, DateTime.Now);
        }

        private void MoveToNextLineOrPlant(DateTime selectedDate)
        {
            int selectedIndex = cbxLines.SelectedIndex;
            if (selectedIndex < cbxLines.Items.Count - 1)
            {
                cbxLines.SelectedIndex = selectedIndex + 1;
            }
            else
            {
                string plantName = cbxPlant.Text;
                string formattedDate = selectedDate.ToString("dd-MM-yyyy");
                cbxLines.SelectedIndex = -1;
                MessageBox.Show($"Đã nhập thông tin cho tất cả các chuyền của {plantName} trong ngày {formattedDate}!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (cbxPlant.SelectedIndex < cbxPlant.Items.Count - 1)
                {
                    cbxPlant.SelectedIndex += 1;
                }
                cbxLines.Focus();
            }
        }

        private void TriggerLeaveEventsForActualTextBoxes()
        {
            txtActualChemical1Upper_Leave(txtActualChemical1Upper, EventArgs.Empty);
            txtActualChemical1Outsole_Leave(txtActualChemical1Outsole, EventArgs.Empty);
            txtActualChemical2Upper_Leave(txtActualChemical2Upper, EventArgs.Empty);
            txtActualChemical2Outsole_Leave(txtActualChemical2Outsole, EventArgs.Empty);
            txtActualChemical3Upper_Leave(txtActualChemical3Upper, EventArgs.Empty);
            txtActualChemical3Outsole_Leave(txtActualChemical3Outsole, EventArgs.Empty);
        }

        private bool CheckAndConfirmEmptyActualTextBoxes()
        {
            string[] emptyActualTextBoxes = GetEmptyActualTextBoxes();
            if (emptyActualTextBoxes.Length > 0)
            {
                string textBoxNames = string.Join("\n", emptyActualTextBoxes);
                DialogResult result = MessageBox.Show($"Giá trị thực tế ở ô {textBoxNames} chưa được nhập.\nĐiều này sẽ dẫn đến Kết quả FAIL.\nBạn có chắc chắn bỏ qua không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                return result == DialogResult.No;
            }
            return false;
        }

        /// <summary>
        /// Hiển thị các dòng nhật kí lên giao diện người dùng
        /// </summary>
        /// <param name="department"></param>
        /// <param name="timestamp"></param>
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
            foreach (var textBox in resultTextBoxes)
            {
                if (textBox.Text == "FAIL")
                {
                    return "FAIL";
                }
            }

            foreach (var textBox in resultTextBoxes)
            {
                if (!string.IsNullOrEmpty(textBox.Text) && textBox.Text == "PASS")
                {
                    return "PASS";
                }
            }

            return "PASS";
        }
        private void lblLogOut_MouseEnter(object sender, EventArgs e)
        {
            lblLogOut.ForeColor = System.Drawing.Color.NavajoWhite;
        }

        private void lblLogOut_MouseLeave(object sender, EventArgs e)
        {
            lblLogOut.ForeColor = System.Drawing.Color.DarkOrange;
        }


        private string[] GetEmptyActualTextBoxes()
        {
            (TextBox Std, TextBox Actual, string Name)[] textBoxPairs = new (TextBox, TextBox, string)[]
               {
        (txtStdChemical1Outsole , txtActualChemical1Outsole, "Máy 1 Outsole"),
        (txtStdChemical2Outsole , txtActualChemical2Outsole, "Máy 2 Outsole"),
        (txtStdChemical3Outsole , txtActualChemical3Outsole, "Máy 3 Outsole"),
        (txtStdChemical1Upper , txtActualChemical1Upper, "Máy 1 Upper"),
        (txtStdChemical2Upper , txtActualChemical2Upper, "Máy 2 Upper"),
        (txtStdChemical3Upper, txtActualChemical3Upper, "Máy 3 Upper"),
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

        private void RegisterWatermarkEvents()
        {
            foreach (var actualTextBox in actualToStdTextBoxMap.Keys)
            {
                actualTextBox.Enter += ActualTextBox_Enter;
                actualTextBox.KeyPress += ActualTextBox_KeyPress;
            }
        }

        private void txtActualChemical1Outsole_Leave(object sender, EventArgs e)
        {
            TextBox actualTextBox = (TextBox)sender;
            actualTextBox.ForeColor = SystemColors.WindowText;
            CompareAndUpdateResult(actualTextBox, txtStdChemical1Outsole, txtResultChemical1Outsole);
        }

        private void txtActualChemical2Outsole_Leave(object sender, EventArgs e)
        {
            TextBox actualTextBox = (TextBox)sender;
            actualTextBox.ForeColor = SystemColors.WindowText;
            CompareAndUpdateResult(actualTextBox, txtStdChemical2Outsole, txtResultChemical2Outsole);
        }

        private void txtActualChemical3Outsole_Leave(object sender, EventArgs e)
        {
            TextBox actualTextBox = (TextBox)sender;
            actualTextBox.ForeColor = SystemColors.WindowText;
            CompareAndUpdateResult(actualTextBox, txtStdChemical3Outsole, txtResultChemical3Outsole);
        }

        private void txtActualChemical1Upper_Leave(object sender, EventArgs e)
        {
            TextBox actualTextBox = (TextBox)sender;
            actualTextBox.ForeColor = SystemColors.WindowText;
            CompareAndUpdateResult(actualTextBox, txtStdChemical1Upper, txtResultChemical1Upper);
        }

        private void txtActualChemical2Upper_Leave(object sender, EventArgs e)
        {
            TextBox actualTextBox = (TextBox)sender;
            actualTextBox.ForeColor = SystemColors.WindowText;
            CompareAndUpdateResult(actualTextBox, txtStdChemical2Upper, txtResultChemical2Upper);
        }

        private void txtActualChemical3Upper_Leave(object sender, EventArgs e)
        {
            TextBox actualTextBox = (TextBox)sender;
            actualTextBox.ForeColor = SystemColors.WindowText;
            CompareAndUpdateResult(actualTextBox, txtStdChemical3Upper, txtResultChemical3Upper);
        }

        private void CompareAndUpdateResult(TextBox actualTextBox, TextBox stdTextBox, TextBox resultTextBox)
        {
            string actualValue = actualTextBox?.Text.Replace(" ", "").Replace("\"", "") ?? "";
            string stdValue = stdTextBox?.Text.Replace(" ", "").Replace("\"", "") ?? "";

            // Kiểm tra nếu TextBox Tiêu chuẩn không null và không rỗng
            if (!string.IsNullOrEmpty(stdValue))
            {
                // Kiểm tra nếu actualTextBox là txtArticle và txtModel đang là null và actualValue rỗng
                if (actualTextBox == txtArticle && txtModel == null && string.IsNullOrEmpty(actualValue))
                {
                    resultTextBox.Text = "";
                    return;
                }

                string result = CompareTextboxes(actualValue, stdValue);

                // Kiểm tra nếu cả 2 đều null
                if (actualTextBox == null && stdTextBox == null)
                {
                    resultTextBox.Text = "PASS";
                    return;
                }

                resultTextBox.Text = result;

                if (result == "FAIL")
                {
                    resultTextBox.ForeColor = Color.Red;
                }
                else if (result == "PASS")
                {
                    resultTextBox.ForeColor = SystemColors.WindowText;
                }

                else
                {
                    resultTextBox.ForeColor = SystemColors.WindowText;
                    resultTextBox.BackColor = Color.Silver;
                }
            }
        }

        private string CompareTextboxes(string actualValue, string stdValue)
        {
            if (actualValue == null && stdValue != null)
            {
                return "FAIL";
            }
            else if (actualValue != null && stdValue == null)
            {
                return "FAIL";
            }
            else if (actualValue == null && stdValue == null)
            {
                return "PASS";
            }
            else
            {
                return actualValue.Equals(stdValue, StringComparison.OrdinalIgnoreCase) ? "PASS" : "FAIL";
            }
        }

        private void ActualTextBox_Enter(object sender, EventArgs e)
        {
            TextBox actualTextBox = sender as TextBox;
            if (actualTextBox != null)
            {
                TextBox stdTextBox;
                if (actualToStdTextBoxMap.TryGetValue(actualTextBox, out stdTextBox))
                {
                    string watermarkText = stdTextBox.Text;
                    SetWatermark(actualTextBox, watermarkText);
                }
            }
        }

        private void ActualTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox actualTextBox = sender as TextBox;
            if (actualTextBox != null)
            {
                TextBox stdTextBox;
                if (actualToStdTextBoxMap.TryGetValue(actualTextBox, out stdTextBox))
                {
                    string watermarkText = stdTextBox.Text;

                    if (actualTextBox.Text == watermarkText)
                    {
                        actualTextBox.Text = ""; 
                        actualTextBox.ForeColor = SystemColors.WindowText;
                    }
                }
            }
        }

        private void SetWatermark(TextBox textBox, string watermarkText)
        {
            if (string.IsNullOrEmpty(textBox.Text))
            {
                textBox.ForeColor = SystemColors.GrayText;
                textBox.Text = watermarkText;
            }
        }

        public string UserName { get; set; }
        private void frmWarehouse_Load(object sender, EventArgs e)
        {
            string department = "Warehouse";
            DisplayActivity(department, DateTime.Now);

            cbxPlant.Focus();

            List<string> plantNames = dbManager.GetPlantNames();
            foreach (var plantName in plantNames)
            {
                cbxPlant.Items.Add("Xưởng " + plantName);
            }

            lblUserWH.Text = Globals.Username;
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
                        cbxLines.DataSource = lines;
                     //   dbManager.SortProductionLines(cbxLines);
                    }
                    else
                    {
                        MessageBox.Show("Không có Chuyền tương ứng với xưởng này.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                ClearTextBoxes();
            }
        }
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams handleParam = base.CreateParams;
                handleParam.ExStyle |= 0x02000000; // WS_EX_COMPOSITED
                return handleParam;
            }
        }
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            // Tạo các màu cho gradient
            Color color1 = Color.Black; // Màu đen
            Color color2 = Color.Navy; // Màu navy
            Color color3 = Color.DarkBlue; // Màu xanh đen

            // Tạo một LinearGradientBrush
            using (LinearGradientBrush brush = new LinearGradientBrush(this.ClientRectangle, color1, color1, LinearGradientMode.Horizontal))
            {
                ColorBlend colorBlend = new ColorBlend();
                colorBlend.Colors = new Color[] { color1, color2, color3, color2 }; // Thứ tự màu sắc
                colorBlend.Positions = new float[] { 0.0f, 0.5f, 0.6f, 1.0f }; // Tỉ lệ phân bố màu sắc

                brush.InterpolationColors = colorBlend;

                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }
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

        private void ClearTextBoxes()
        {
            // Tạo danh sách chứa tất cả các TextBox cần xóa
            List<TextBox> textBoxesToClear = new List<TextBox>
            {
                txtArticle, txtModel,
                txtActualChemical1Outsole, txtActualChemical2Outsole, txtActualChemical3Outsole,
                txtActualChemical1Upper, txtActualChemical2Upper, txtActualChemical3Upper,
                txtResultChemical1Outsole, txtResultChemical2Outsole, txtResultChemical3Outsole,
                txtResultChemical1Upper, txtResultChemical2Upper, txtResultChemical3Upper,
                txtStdChemical1Outsole, txtStdChemical2Outsole, txtStdChemical3Outsole,
                txtStdChemical1Upper, txtStdChemical2Upper, txtStdChemical3Upper
            };
            // Duyệt qua danh sách và xóa nội dung của từng TextBox
            foreach (var textBox in textBoxesToClear)
            {
                textBox.Clear();
            }

            lblCreatedAt.Text = "";
            lblCreatedBy.Text = "";
        }


        private void txtArtcicle_TextChanged(object sender, EventArgs e)
        {
            string article = txtArticle.Text;

            if (!string.IsNullOrEmpty(article))
            {
                DatabaseManager dbManager = new DatabaseManager(connectionString);

                bool articleExists = dbManager.ArticleExists(article);

                if (articleExists)
                {
                    // Kiểm tra và hiển thị thông tin tiêu chuẩn
                    CheckAndDisplayStandardData(article);
                }
                else
                {
                    // Hiển thị thông báo khi Article không tồn tại trong cơ sở dữ liệu
                    DialogResult dialogResult = MessageBox.Show("Article không tồn tại trong cơ sở dữ liệu.\nHãy thông báo CE để cập nhật thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    if (dialogResult == DialogResult.OK)
                    {
                        txtArticle.Clear();
                    }
                }
            }
        }

        private bool AreAllStandardValuesNullOrEmpty(ArticleData articleData)
        {
            bool Chemical1UpperNullOrEmpty = string.IsNullOrWhiteSpace(articleData.Chemical1Upper);
            bool Chemical2UpperNullOrEmpty = string.IsNullOrWhiteSpace(articleData.Chemical2Upper);
            bool Chemical3UpperNullOrEmpty = string.IsNullOrWhiteSpace(articleData.Chemical3Upper);
            bool Chemical1OutsoleNullOrEmpty = string.IsNullOrWhiteSpace(articleData.Chemical1Outsole);
            bool Chemical2OutsoleNullOrEmpty = string.IsNullOrWhiteSpace(articleData.Chemical2Outsole);
            bool Chemical3OutsoleNullOrEmpty = string.IsNullOrWhiteSpace(articleData.Chemical3Outsole);

            return Chemical1UpperNullOrEmpty && Chemical2OutsoleNullOrEmpty &&
                   Chemical3UpperNullOrEmpty && Chemical1OutsoleNullOrEmpty &&
                   Chemical2UpperNullOrEmpty && Chemical3OutsoleNullOrEmpty;
        }
        private void CheckAndDisplayStandardData(string article)
        {
            string articleName = txtArticle.Text;
            DatabaseManager dbManager = new DatabaseManager(connectionString);
            ArticleData articleData = dbManager.GetArticleData(article);

            if (articleData != null)
            {
                if (AreAllStandardValuesNullOrEmpty(articleData))
                {
                    DialogResult dialogResult = MessageBox.Show ($"Article '{articleName}' chưa được cập nhật tiêu chuẩn BPFC!\nHãy thông báo CE để cập nhật thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    if (dialogResult == DialogResult.OK)
                    {
                    }
                }
                else
                {
                    DisplayArticleData(articleData);
                }
            }
            else
            {
                // Hiển thị thông báo khi không có Article trong cơ sở dữ liệu
                MessageBox.Show("Không có dữ liệu cho Article này.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void DisplayArticleData(ArticleData articleData)
        {
            // Tùy chỉnh để hiển thị thông tin lên các TextBox tương ứng trên form frmPlant
            lblCreatedAt.Text = $"Thời gian tạo: {articleData.CreatedAt.ToString()}";
            lblCreatedBy.Text = $"BPFC được nhập bởi: {articleData.CreatedBy}";
            txtModel.Text = articleData.Model;

            // Hiển thị thông tin lên các TextBox Upper
            txtStdChemical1Upper.Text = articleData.Chemical1Upper;
            txtStdChemical2Upper.Text = articleData.Chemical2Upper;
            txtStdChemical3Upper.Text = articleData.Chemical3Upper;

            // Hiển thị thông tin lên các TextBox Outsole
            txtStdChemical1Outsole.Text = articleData.Chemical1Outsole;
            txtStdChemical2Outsole.Text = articleData.Chemical2Outsole;
            txtStdChemical3Outsole.Text = articleData.Chemical3Outsole;
        }

        private void cbxLines_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Lấy LineName được chọn trong cbxLines
            string LineName = cbxLines.SelectedItem?.ToString();

            DateTime selectedDate = dtpDate.Value.Date;

            if (!string.IsNullOrEmpty(LineName))
            {
                if (togSwitch.IsOn)
                {
                    LoadDataFromDatabase(LineName, selectedDate);
                    ColorizeTextBoxes();
                }
                else
                {
                    ColorizeTextBoxes();
                    string articleName = GetArticleNameForLineAndDate(LineName, selectedDate);

                    if (string.IsNullOrEmpty(articleName))
                    {
                        string formattedDate = selectedDate.ToString("dd/MM/yyyy");

                        MessageBox.Show($"Ngày {formattedDate} - Chuyền \"{LineName}\" chưa cập nhật Article!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        txtArticle.Text = articleName;
                    }
                    txtActualChemical1Outsole.Focus();
                }

            }
        }

        private string GetArticleNameForLineAndDate(string lineName, DateTime date)
        {
            string articleName = "";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT TOP 1 ArticleName FROM ArticleOfLines WHERE LineName = @LineName AND CAST(ReportDate AS DATE) = @SelectedDate ORDER BY ReportDate DESC";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LineName", lineName);
                    command.Parameters.AddWithValue("@SelectedDate", date);

                    try
                    {
                        // Thực hiện truy vấn và lấy giá trị ArticleName
                        object result = command.ExecuteScalar();
                        if (result != null)
                        {
                            articleName = result.ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Lỗi khi truy vấn dữ liệu từ bảng ArticleOfLines: " + ex.Message);
                    }
                }
            }

            return articleName;
        }

        private void SetTextBoxesBackColor(params TextBox[] textBoxes)
        {
            foreach (TextBox textBox in textBoxes)
            {
                if (textBox != null)
                {
                    textBox.BackColor = Color.Silver;
                }
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            ClearTextBoxes();
            cbxPlant.SelectedIndex = 0;
            cbxLines.SelectedIndex = -1;
            SetTextBoxesBackColor(
            txtResultChemical1Outsole,
            txtResultChemical1Upper,
            txtResultChemical2Outsole,
            txtResultChemical2Upper,
            txtResultChemical3Outsole,
            txtResultChemical3Upper
            );
        }

        private void cbxLines_MouseClick(object sender, MouseEventArgs e)
        {
            ClearTextBoxes();
            txtArticle.Clear();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmHome formHome = new frmHome();
            formHome.Show();
        }

        private void frmWarehouse_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void dtpDate_ValueChanged(object sender, EventArgs e)
        {
            if(cbxPlant.Items.Count > 0)
            {
                cbxPlant.SelectedIndex = -1;
            }
            ClearTextBoxes() ;
            cbxLines.SelectedIndex = -1;
        }

        private void togSwitch_Toggled(object sender, EventArgs e)
        {
            ColorizeTextBoxes();
            ClearTextBoxes();

        }
    }
}
