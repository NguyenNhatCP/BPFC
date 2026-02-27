using System;
using System.Windows.Forms;
using System.Configuration;
using DevExpress.XtraEditors;

namespace BPFC_System
{
    public partial class frmLogin : XtraForm
    {
        private readonly DatabaseManager dbManager;

        public static bool isLoggedIn = false;

        private bool isMouseDown = false;

        public frmLogin()
        {
            InitializeComponent();
            EnableDoubleBufferingForForm();
            EnableDoubleBufferingForControls();

            string connectionString = ConfigHelper.GetConnectionString("strCon");
            LookAndFeel.UseWindowsXPTheme = true;
            txtPassword.UseSystemPasswordChar = true;
            dbManager = new DatabaseManager(connectionString);
        }

        private void EnableDoubleBufferingForForm()
        {
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();
        }

        private void EnableDoubleBufferingForControls()
        {
            foreach (Control control in this.Controls)
            {
                control.EnableDoubleBuffering();
            }
        }
            
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtUsername.Focus();
                return;
            }

            // Kiểm tra sự tồn tại của người dùng
            bool doesUserExist = dbManager.UserExists(username);
            if (!doesUserExist)
            {
                // Hiển thị thông báo lỗi nếu tên đăng nhập không tồn tại
                MessageBox.Show("Tên đăng nhập không tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Clear();
                txtPassword.Focus();
                return;
            }

            // Kiểm tra tài khoản có kích hoạt không
            bool isActive = dbManager.IsActive(username);

            if(!isActive)
            {
                // Hiển thị thông báo lỗi nếu tài khoản đã bị vô hiệu hóa
                MessageBox.Show("Tài khoản đã bị vô hiệu hóa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Clear();
                txtPassword.Focus();
                return;
            }

            // Kiểm tra tình trạng tài khoản
            bool isUserValid = dbManager.ValidateUser(username, password);

            if (isUserValid)
            {
                // Xóa mật khẩu và ẩn form đăng nhập
                txtPassword.Clear();
                this.Hide();
                int userId = dbManager.GetUserIdByUsername(username);

                // Mở form tương ứng dựa trên bộ phận
                dbManager.OpenCorrespondingForm(username);
            }
            else
            {
                // Hiển thị thông báo lỗi nếu mật khẩu sai
                MessageBox.Show("Mật khẩu sai!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Clear();
                txtPassword.Focus();
            }
        }

        private void LblShowPassword_MouseEnter(object sender, EventArgs e)
        {
            lblShowPassword.ForeColor = System.Drawing.Color.White; 
        }

        private void LblChangePassword_MouseEnter(object sender, EventArgs e)
        {
            lblChangePassword.ForeColor = System.Drawing.Color.White; 
        }

        private void LblShowPassword_MouseDown(object sender, MouseEventArgs e)
        {
            isMouseDown = true;
            TogglePasswordVisibility();
        }

        private void LblShowPassword_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
            TogglePasswordVisibility();
        }

        private void LblShowPassword_MouseLeave(object sender, MouseEventArgs e)
        {
            lblShowPassword.ForeColor = System.Drawing.Color.Silver;
        }

        private void TogglePasswordVisibility()
        {
            txtPassword.UseSystemPasswordChar = !isMouseDown;
        }

        private void LblShowPassword_MouseLeave(object sender, EventArgs e)
        {
            lblShowPassword.ForeColor = System.Drawing.Color.Silver;
        }


        private void lblChangePassword_Click(object sender, EventArgs e)
        {
            frmChangePassword changePasswordForm = new frmChangePassword();
            changePasswordForm.ShowDialog();
        }

        private void frmLogin_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void lblChangePassword_MouseLeave(object sender, EventArgs e)
        {
            lblChangePassword.ForeColor = System.Drawing.Color.Silver;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}