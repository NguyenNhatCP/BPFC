using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Windows.Forms;
using System.Drawing;
using DevExpress.Internal;
using BPFC_System;
namespace BPFC_System
{
    public partial class CreateUsers : DevExpress.XtraEditors.XtraUserControl

    {
        private DataTable dt;

        private string connectionString;

        public CreateUsers()
        {
            InitializeComponent();
            dt = new DataTable();
            connectionString = ConfigHelper.GetConnectionString("strCon");
            this.DoubleBuffered = true;
        }

        private SqlConnection CreateSqlConnection()
        {
            string connectionString = ConfigHelper.GetConnectionString("strCon");
            return new SqlConnection(connectionString);
        }

        private void btnAddDept_Click(object sender, EventArgs e)
        {
            string newDepartment = txtNewDept.Text.Trim();

            if (!string.IsNullOrEmpty(newDepartment))
            {
                DatabaseManager dbManager = new DatabaseManager(connectionString);
                if (!dbManager.IsDepartmentExists(newDepartment))
                {
                    dbManager.AddDepartment(newDepartment);

                    cbxDepartment.DataSource = dbManager.GetDepartments();
                    cbxCurrentDept.DataSource = dbManager.GetDepartments();

                    txtNewDept.Clear();
                    cbxCurrentDept.SelectedIndex = -1;
                    cbxDepartment.SelectedIndex = -1;
                    MessageBox.Show($"Bộ phận {newDepartment} đã được thêm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Bộ phận đã tồn tại trong cơ sở dữ liệu.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng nhập tên bộ phận mới.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDeleteDept_Click(object sender, EventArgs e)
        {
            if (cbxCurrentDept.SelectedItem != null)
            {
                string departmentToDelete = cbxCurrentDept.SelectedItem.ToString();

                if (!string.IsNullOrEmpty(departmentToDelete))
                {
                    DatabaseManager dbManager = new DatabaseManager(connectionString);

                    bool deleted = dbManager.DeleteDepartment(departmentToDelete);

                    if (deleted)
                    {
                        cbxDepartment.DataSource = dbManager.GetDepartments();
                        cbxCurrentDept.DataSource = dbManager.GetDepartments();
                        cbxCurrentDept.SelectedIndex = -1;
                        cbxDepartment.SelectedIndex = -1;

                        MessageBox.Show($"Bộ phận {departmentToDelete} đã được xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Không thể xóa bộ phận. Xảy ra lỗi.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn bộ phận để xóa.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một bộ phận trước khi thực hiện xóa.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ucCreateUsers_Load(object sender, EventArgs e)
        {
            DatabaseManager dbManager = new DatabaseManager(connectionString);

            cbxDepartment.DropDownStyle = ComboBoxStyle.DropDownList;
            cbxCurrentDept.DropDownStyle = ComboBoxStyle.DropDownList;
            cbxCurrentDept.DataSource = dbManager.GetDepartments();
            cbxDepartment.DataSource = dbManager.GetDepartments();
            cbxCurrentDept.SelectedIndex = -1;
            cbxDepartment.SelectedIndex = -1;
        

        // Khởi tạo DataTable và định dạng cột
        dt = new DataTable();
            dt.Columns.Add("UserID");
            dt.Columns.Add("Username");
            dt.Columns.Add("EmployeeName");
            dt.Columns.Add("Department");
            dt.Columns.Add("EmployeeID");
            dt.Columns.Add("CreatedAt");

            // Liên kết DataGridView với DataTable
            dgvUsers.DataSource = dt;

            // Đặt tiêu đề cho các cột
            dgvUsers.Columns["UserID"].HeaderText = "Mã người dùng";
            dgvUsers.Columns["Username"].HeaderText = "Tên tài khoản";
            dgvUsers.Columns["EmployeeName"].HeaderText = "Tên nhân viên";
            dgvUsers.Columns["EmployeeID"].HeaderText = "Mã nhân viên";
            dgvUsers.Columns["Department"].HeaderText = "Bộ phận";
            dgvUsers.Columns["CreatedAt"].HeaderText = "Thời gian tạo";
            dgvUsers.DefaultCellStyle.Font = new Font("Times new roman", 9);
            dgvUsers.Columns["CreatedAt"].Width = 150;
            dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUsers.BorderStyle = BorderStyle.Fixed3D;
            dgvUsers.DefaultCellStyle.Font = new Font("Times New Roman", 9);
            dgvUsers.ColumnHeadersDefaultCellStyle.Font = new Font("Time New Roman", 9);
            dgvUsers.RowTemplate.Height = 30;
            dgvUsers.ColumnHeadersDefaultCellStyle.Padding = new Padding(10, 5, 10, 5);
            foreach (DataGridViewColumn column in dgvUsers.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            LoadUsers();

            // Ẩn cột UserID
            dgvUsers.Columns["UserID"].Visible = false;

            // Thêm cột nút "Vô hiệu hóa"
            var disableButtonColumn = new DataGridViewButtonColumn();
            disableButtonColumn.Name = "Disable";
            disableButtonColumn.HeaderText = "";
            disableButtonColumn.Text = "Vô hiệu hóa";
            disableButtonColumn.UseColumnTextForButtonValue = true;
            dgvUsers.Columns.Add(disableButtonColumn);

            // Đặt con trỏ vào ô nhập liệu txtNewUsername
            txtNewUsername.Focus();
            // Gắn sự kiện CellFormatting để định dạng cột "CreatedAt"
            dgvUsers.CellFormatting += dgvUsers_CellFormatting;

        }
        // Tải danh sách người dùng từ cơ sở dữ liệu
        private void LoadUsers()
        {
            string query = "SELECT UserID, Username, EmployeeName, EmployeeID, Department, CreatedAt FROM dbo.Users WHERE isActive = 1";

            using (SqlConnection connection = CreateSqlConnection())
            {
                connection.Open();
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                {
                    dt.Clear();
                    adapter.Fill(dt);

                    // Chuyển đổi giá trị "CreatedAt" thành kiểu DateTime
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row["CreatedAt"] != DBNull.Value && row["CreatedAt"] is string)
                        {
                            string createdAtStr = row["CreatedAt"].ToString();
                            if (DateTime.TryParse(createdAtStr, out DateTime createdAt))
                            {
                                row["CreatedAt"] = createdAt;
                            }
                        }
                    }
                }
            }
        }

        // Sự kiện CellFormatting để định dạng cột "CreatedAt"
        private void dgvUsers_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == dgvUsers.Columns["CreatedAt"].Index)
            {
                if (e.Value is DateTime dateTimeValue)
                {
                    e.Value = dateTimeValue.ToString("dd/MM/yyyy HH:mm:ss");
                    e.FormattingApplied = true;
                }
            }
        }


        // Sự kiện CellClick để xử lý vô hiệu hóa tài khoản
        private void dgvUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvUsers.Columns["Disable"].Index && e.RowIndex >= 0)
            {
                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn vô hiệu hóa tài khoản này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    string userID = dgvUsers.Rows[e.RowIndex].Cells["UserID"].Value.ToString();

                    if (DisableUserAccount(userID))
                    {
                        MessageBox.Show("Vô hiệu hóa tài khoản thành công!", "Thành công", MessageBoxButtons.OK);
                        LoadUsers();
                    }
                    else
                    {
                        MessageBox.Show("Không thể vô hiệu hóa tài khoản. Xảy ra lỗi.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // Vô hiệu hóa tài khoản người dùng
        private bool DisableUserAccount(string userID)
        {
            try
            {
                using (SqlConnection connection = CreateSqlConnection())
                {
                    connection.Open();

                    string query = "UPDATE Users SET isActive = 0 WHERE UserID = @UserID";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userID);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
                return false;
            }
        }


        public void btnCreate_Click(object sender, EventArgs e)
        {
            string newUsername = txtNewUsername.Text;
            string newPassword = txtPassword.Text;
            string employeeName = txtEmployeeName.Text;
            string department = cbxDepartment.Text;
            int employeeID;

            if (string.IsNullOrEmpty(newUsername) || string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(employeeName) || string.IsNullOrEmpty(department))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin tạo tài khoản.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!int.TryParse(txtEmployeeID.Text, out employeeID) || employeeID == 0)
            {
                MessageBox.Show("Mã nhân viên phải là một số nguyên và không được để trống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string connectionString = ConfigHelper.GetConnectionString("strCon");
            DatabaseManager dbManager = new DatabaseManager(connectionString);


            if (dbManager.UserExists(newUsername))
            {
                MessageBox.Show("Tài khoản đã tồn tại.\nVui lòng chọn tên tài khoản khác.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string hashedPassword = dbManager.ComputeHash(newPassword);
            dbManager.AddUser(newUsername, hashedPassword, employeeName, department, employeeID);
            MessageBox.Show($"User {newUsername} đã được tạo thành công!", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Sau khi tạo thành công, cập nhật bảng dữ liệu và xóa các trường nhập liệu
            LoadUsers();
            txtNewUsername.Clear();
            txtPassword.Clear();
            txtEmployeeName.Clear();
            cbxDepartment.SelectedIndex = -1; 
            txtEmployeeID.Clear();
        }

        private void btnEditUser_Click(object sender, EventArgs e)
        {
            // Tạo một thể hiện của frmEditUser
            frmEditUser editUserForm = new frmEditUser();

            editUserForm.ShowDialog();
        }
    }
}