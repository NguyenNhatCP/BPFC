using BPFC_System;
using DevExpress.Internal;
using System;
using System.Configuration;
using System.Windows.Forms;

namespace BPFC_System
{
    public partial class frmEditUser : DevExpress.XtraEditors.XtraForm
    {

        private readonly string connectionString;

        public frmEditUser()
        {

            InitializeComponent();
            connectionString = ConfigHelper.GetConnectionString("strCon");

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            DatabaseManager dbManager = new DatabaseManager(connectionString);

            string usernameToChangePassword = txtUsername.Text;
            string newPassword = txtPassword.Text;

            // Kiểm tra ô nhập liệu có rỗng không
            if (string.IsNullOrWhiteSpace(usernameToChangePassword))
            {
                MessageBox.Show("Vui lòng nhập tên người dùng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Kiểm tra giá trị cột IsActive
            bool isActive = dbManager.GetUserIsActiveStatus(usernameToChangePassword);

            if (!isActive)
            {
                // Nếu giá trị là False, hiển thị hộp thoại Yes/No
                DialogResult result = MessageBox.Show("Người dùng đã bị vô hiệu hóa. Bạn có muốn kích hoạt lại tài khoản?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Thực hiện cập nhật giá trị thành True
                    dbManager.ActivateUserAccount(usernameToChangePassword);
                    MessageBox.Show("Kích hoạt tài khoản thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            // Kiểm tra có nhập mật khẩu mới hay không
            if (!string.IsNullOrWhiteSpace(newPassword))
            {
                // Kiểm tra tính hợp lệ của mật khẩu mới
                if (!dbManager.IsNewPasswordValid(newPassword))
                {
                    MessageBox.Show("Độ dài mật khẩu tối thiểu 5 kí tự!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Thực hiện cập nhật mật khẩu mới trong cơ sở dữ liệu
                if (dbManager.ChangePassword(usernameToChangePassword, newPassword))
                {
                    MessageBox.Show("Đổi mật khẩu thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Đã xảy ra lỗi trong quá trình đổi mật khẩu.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            // Xóa trường nhập liệu và cập nhật giao diện nếu cần
            txtUsername.Clear();
            txtPassword.Clear();
        }
    }
}