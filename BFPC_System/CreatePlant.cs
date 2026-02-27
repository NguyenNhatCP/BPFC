using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using static BPFC_System.DatabaseManager;

namespace BPFC_System
{
    public partial class CreatePlant : DevExpress.XtraEditors.XtraUserControl
    {
        private readonly string connectionString;
        private readonly DatabaseManager dbManager;

        public CreatePlant()
        {
            InitializeComponent();
            string connectionString = ConfigHelper.GetConnectionString("strCon");
            dbManager = new DatabaseManager(connectionString);
            LoadPlantNames();
            this.DoubleBuffered = true;
            txtEditLine.Visible = false;
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
            numNumberOfLines.Value = 10;
        }

        private void lvProductionLines_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Hiển thị hoặc ẩn btnEditLines và btnDeleteLine dựa trên số lượng item được chọn
            if (lvProductionLines.SelectedItems.Count > 0)
            {
                btnEditLines.Visible = true;
                btnDeleteLine.Visible = true;
                btnDeletePlant.Visible = false;
            }
            else
            {   
                btnDeletePlant.Visible = true;
                btnEditLines.Visible = false;
                btnDeleteLine.Visible = false;
                btnCancel.Visible = false;
            }
        }

        private void btnDeleteLine_Click(object sender, EventArgs e)
        {
            if (lvProductionLines.SelectedItems.Count > 0)
            {
                foreach (ListViewItem item in lvProductionLines.SelectedItems)
                {
                    string selectedLineName = item.Text;
                    try
                    {
                        dbManager.DeleteProductionLine(selectedLineName);
                        lvProductionLines.Items.Remove(item);
                        MessageBox.Show($"Line {selectedLineName} has been deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting line {selectedLineName}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a line to delete.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void cbxPlantNames_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnEditLines.Visible = false;
            btnDeleteLine.Visible = false ;
            btnDeletePlant.Visible = true ;

            if (cbxPlantNames.SelectedItem != null)
            {
                string selectedPlantName = cbxPlantNames.SelectedItem.ToString().Substring(6);
                if (!string.IsNullOrEmpty(selectedPlantName))
                {
                    LoadProductionLines(selectedPlantName);
                }
                else
                {
                    lvProductionLines.Items.Clear();
                }
            }
            else
            {
                lvProductionLines.Items.Clear();
            }
        }

        private bool isFirstCharEntered = false;
        private bool isToolTipShown = false; 

        private void txtPlantName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Back && isFirstCharEntered)
            {
                // Kiểm tra độ dài của văn bản, ví dụ cho phép chỉ nhập 1 ký tự
                if (txtPlantName.Text.Length >= 1)
                {
                    e.SuppressKeyPress = true;

                    if (!isToolTipShown)
                    {
                        ToolTip toolTip = new ToolTip();
                        toolTip.ToolTipTitle = "Mẹo";
                        toolTip.ToolTipIcon = ToolTipIcon.Info;
                        toolTip.SetToolTip(txtPlantName, "Tên xưởng chỉ bao gồm một ký tự.");
                        isToolTipShown = true; 
                    }
                }
            }
            else
            {
                isFirstCharEntered = true;
            }
        }

        private void btnEditLines_Click(object sender, EventArgs e)
        {
            if (lvProductionLines.SelectedItems.Count > 0)
            {
                // Lấy giá trị của mục đang chọn
                string selectedLine = lvProductionLines.SelectedItems[0].Text;

                // Hiển thị giá trị cũ trong TextBox để chỉnh sửa
                txtEditLine.Text = selectedLine;

                // Hiển thị TextBox để người dùng nhập giá trị mới
                txtEditLine.Visible = true;

                // Ẩn nút Edit, hiển thị nút Save và Cancel
                btnEditLines.Visible = false;
                btnSaveLines.Visible = true;
                btnCancel.Visible = true;
                btnDeleteLine.Visible = false;

                // Tắt khả năng chọn mục trong ListView
                lvProductionLines.Enabled = false;
            }
        }
        private void UpdateLineName(string oldLineName, string newLineName)
        {
            try
            {
                // Cập nhật dữ liệu trong cơ sở dữ liệu
                dbManager.UpdateLineName(oldLineName, newLineName);

                // Cập nhật dữ liệu trong ListView
                lvProductionLines.SelectedItems[0].Text = newLineName;

                MessageBox.Show("Cập nhật thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Trong sự kiện LoadProductionLines
        private void LoadProductionLines(string selectedPlantName)
        {
            lvProductionLines.Items.Clear();
            List<string> productionLines = dbManager.LoadProductionLines(selectedPlantName);
 
            // Hiển thị danh sách trước khi sắp xếp
            lvProductionLines.Items.AddRange(productionLines.Select(line => new ListViewItem(line)).ToArray());

            // Sắp xếp danh sách trong lvProductionLines
            SortProductionLines(lvProductionLines);
        }

        public void SortProductionLines(ListView listview)
        {
            listview.Sorting = System.Windows.Forms.SortOrder.Ascending;
            listview.ListViewItemSorter = new ListViewItemComparer();
            listview.Sort();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (lvProductionLines.SelectedItems.Count > 0)
            {
                // Lấy giá trị của mục đang chọn
                string selectedLine = lvProductionLines.SelectedItems[0].Text;

                // Lấy giá trị mới từ TextBox
                string editedLine = txtEditLine.Text.Trim();

                // Kiểm tra giá trị mới có rỗng không
                if (string.IsNullOrEmpty(editedLine))
                {
                    MessageBox.Show("Tên chuyền mới không được để trống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Kiểm tra xem tên mới có khác với tên cũ hay không
                if (selectedLine != editedLine)
                {
                    // Kiểm tra xem giá trị mới đã tồn tại trong danh sách chưa
                    if (IsLineNameExists(editedLine))
                    {
                        MessageBox.Show($"Tên chuyền '{editedLine}' đã tồn tại trong danh sách.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    lvProductionLines.Enabled = true;
                    // Ẩn textbox
                    txtEditLine.Visible = false;

                    // Ẩn nút Save, hiển thị nút Edit
                    btnSaveLines.Visible = false;
                    btnEditLines.Visible = true;

                    // Ẩn nút Cancel
                    btnCancel.Visible = false;

                    btnDeleteLine.Visible = true;

                    // Cập nhật lineName mới lên cơ sở dữ liệu
                    UpdateLineName(selectedLine, editedLine);

                    // Làm mới dữ liệu trong lvProductionLines
                    if (cbxPlantNames.SelectedItem != null)
                    {
                        string selectedPlantName = cbxPlantNames.SelectedItem.ToString().Substring(6);
                        if (!string.IsNullOrEmpty(selectedPlantName))
                        {
                            LoadProductionLines(selectedPlantName);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Tên chuyền không có thay đổi!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private bool IsLineNameExists(string lineName)
        {
            // Kiểm tra xem giá trị mới đã tồn tại trong danh sách chưa
            foreach (ListViewItem item in lvProductionLines.Items)
            {
                if (item.Text.Trim().Equals(lineName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        private void btnAddPlant_Click(object sender, EventArgs e)
        {
            string plantName = txtPlantName.Text.Trim();

            if (string.IsNullOrEmpty(plantName))
            {
                MessageBox.Show("Tên xưởng không được để trống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPlantName.Focus();
                return;
            }

            if (IsPlantNameExists(plantName))
            {
                MessageBox.Show($"Xưởng {plantName} tồn tại trong hệ thống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPlantName.Clear();
                txtPlantName.Focus();
                return;
            }

            int numberOfLines = (int)numNumberOfLines.Value;

            if (dbManager.AddPlant(plantName, numberOfLines))
            {
                string newPlantName = "Xưởng " + plantName;
                cbxPlantNames.Items.Add(newPlantName);
                cbxPlantNames.SelectedItem = newPlantName;

                LoadProductionLines(plantName);

                MessageBox.Show($"Xưởng {plantName} đã được thêm thành công.");
                txtPlantName.Clear();
                numNumberOfLines.Value = 10;
            }
            else
            {
                MessageBox.Show("Lỗi khi thêm xưởng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public List<string> SortProductionLines(List<string> productionLines)
        {
            // Sắp xếp danh sách các dòng sản xuất theo phần số từ bé đến lớn
            productionLines.Sort((x, y) =>
            {
                string[] partsX = x.Split('L');
                string[] partsY = y.Split('L');

                // Chuyển đổi phần số thành số nguyên để so sánh
                int numberX = int.Parse(partsX[1]);
                int numberY = int.Parse(partsY[1]);

                return numberX.CompareTo(numberY);
            });

            return productionLines;
        }

        private bool IsPlantNameExists(string plantName)
        {
            return dbManager.IsPlantNameExists(plantName);
        }


        private void DeletePlantAndLines(string plantName)
        {
            if (dbManager.DeletePlantAndLines(plantName))
            {
                lvProductionLines.Items.Clear();
                if (cbxPlantNames.Items.Contains("Xưởng " + plantName))
                {
                    cbxPlantNames.Items.Remove("Xưởng " + plantName);
                }
                if (cbxPlantNames.Items.Count > 0)
                {
                    cbxPlantNames.SelectedIndex = 0;
                }
                else
                {
                    lvProductionLines.Items.Clear();
                    LoadPlantNames();
                }

                MessageBox.Show($"Xóa xưởng {plantName} và các line tương ứng thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show($"Lỗi khi xóa xưởng {plantName}.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDeletePlant_Click(object sender, EventArgs e)
        {
            if (cbxPlantNames.SelectedItem != null)
            {
                string selectedPlant = cbxPlantNames.SelectedItem.ToString().Substring(6);
                DialogResult result = MessageBox.Show($"Bạn có chắc chắn muốn xóa xưởng {selectedPlant} và các line tương ứng?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    DeletePlantAndLines(selectedPlant);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một xưởng để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    

        private void txtPlantName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLower(e.KeyChar))
            {
                e.KeyChar = char.ToUpper(e.KeyChar);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // Ẩn TextBox
            txtEditLine.Visible = false;

            // Hiển thị lại nút Edit, ẩn nút Save
            btnEditLines.Visible = true;
            btnSaveLines.Visible = false;

            // Bật khả năng chọn mục trong ListView
            lvProductionLines.Enabled = true;
            btnDeleteLine.Visible = true;
        }
    }
}
