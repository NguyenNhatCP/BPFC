using System;
using System.Windows.Forms;

namespace BPFC_System
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // Lấy chuỗi kết nối từ cấu hình appconfig
            string connectionString = ConfigHelper.GetConnectionString("strCon");

            // Khởi tạo tài khoản admin nếu chưa có
            InitializeAdminAccount(connectionString);

            // Khởi động ứng dụng
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form formToOpen = new frmLogin();

            Application.Run(formToOpen);
        }

        private static void InitializeAdminAccount(string connectionString)
        {
            using (AdminInitializer adminInitializer = new AdminInitializer(connectionString))
            {
                adminInitializer.InitializeAdminAccount();
            }
        }
    }
}
