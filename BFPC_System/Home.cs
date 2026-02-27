using BPFC_System;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Configuration;

namespace BPFC_System
{
    public partial class frmHome : DevExpress.XtraEditors.XtraForm
    {
        private string connectionString;

        public frmHome()
        {
            InitializeComponent();
            connectionString = ConfigHelper.GetConnectionString("strCon");
        }

        private void frmHome_Load(object sender, EventArgs e)
        {
            lblUser.Text = Globals.Username;
            DatabaseManager dbManager = new DatabaseManager(connectionString);

            this.EnableDoubleBuffering();

            if (!string.IsNullOrEmpty(Globals.Username))
            {
                string department = dbManager.GetDepartmentFromDatabase(Globals.Username);

                if (IsUserInMEDepartment(department))
                {
                    btnAdmin.Visible = true;
                }
                else
                {
                    btnAdmin.Visible = false;
                }
            }
        }
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            // Tạo các màu cho gradient
            Color color1 = Color.Black;
            Color color2 = Color.Black; 
            Color color3 = Color.FromArgb(0, 51, 51); 

            // Tạo một LinearGradientBrush
            using (LinearGradientBrush brush = new LinearGradientBrush(this.ClientRectangle, color1, color1, LinearGradientMode.Horizontal))
            {
                ColorBlend colorBlend = new ColorBlend();
                colorBlend.Colors = new Color[] { color1, color2, color3, color2 };
                colorBlend.Positions = new float[] { 0.0f, 0.4f, 0.6f, 1.0f };

                brush.InterpolationColors = colorBlend;

                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }
        }

        private void btnCe_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmBpfc formBpfc = new frmBpfc();
            formBpfc.Show();
        }

        private void btnQip_Click(object sender, EventArgs e)
        {
            using (frmSelectPlant selectPlantForm = new frmSelectPlant())
            {
                selectPlantForm.StartPosition = FormStartPosition.CenterScreen;

                DialogResult result = selectPlantForm.ShowDialog(this);

                if (selectPlantForm.PlantFormOpened)
                {
                    this.Hide();
                }
            }
        }

        private void btnWh_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmWarehouse formWarehouse = new frmWarehouse();
            formWarehouse.Show();
        }

        private void btnMe_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmReport report = new frmReport();
            report.Show();
        }

        private void lblLogOut_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Hide();
                frmLogin formLogin = new frmLogin();
                formLogin.ShowDialog();
            }
        }

        private void btnAdmin_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmAdmin formAdmin = new frmAdmin();
            formAdmin.Show();
        }

        private bool IsUserInMEDepartment(string department)
        {
            return string.Equals(department, "ME", StringComparison.OrdinalIgnoreCase);
        }

        private void frmHome_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void lblLogOut_MouseEnter(object sender, EventArgs e)
        {
            lblLogOut.ForeColor = System.Drawing.Color.NavajoWhite;
        }

        private void lblLogOut_MouseLeave(object sender, EventArgs e)
        {
            lblLogOut.ForeColor = System.Drawing.Color.DarkOrange;
        }
    }
}