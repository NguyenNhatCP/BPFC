using BPFC_System;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace BPFC_System
{
    public partial class frmAdmin : DevExpress.XtraEditors.XtraForm
    {
        private readonly DataShow dataShow;
        private readonly ManageShow manageShow;
        private bool isDataShowOpen = false;
        private bool isManageShowOpen = false;

        public frmAdmin()
        {
            InitializeComponent();
            dataShow = new DataShow();
            manageShow = new ManageShow();
            ShowDataShow();
            DoubleBuffered = true;

            EnableDoubleBufferingForControls(this);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            // Tạo các màu cho gradient
            Color color1 = Color.Black; // Màu đen
            Color color2 = Color.Navy; // Màu navy
            Color color3 = Color.DarkBlue; // Màu xanh

            // Tạo một LinearGradientBrush
            using (LinearGradientBrush brush = new LinearGradientBrush(this.ClientRectangle, color1, color1, LinearGradientMode.Horizontal))
            {
                ColorBlend colorBlend = new ColorBlend();
                colorBlend.Colors = new Color[] { color1, color2, color3, color2 }; // Thứ tự màu sắc
                colorBlend.Positions = new float[] { 0.0f, 0.4f, 0.6f, 1.0f }; // Tỉ lệ phân bố màu sắc

                brush.InterpolationColors = colorBlend;

                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }
        }

        private void ShowDataShow()
        {
            splAdmin.Panel2.Controls.Clear();

            dataShow.Dock = DockStyle.Fill;

            splAdmin.Panel2.Controls.Add(dataShow);
        }

        private void ShowManageShow()
        {
            splAdmin.Panel2.Controls.Clear();

            manageShow.Dock = DockStyle.Fill;

            splAdmin.Panel2.Controls.Add(manageShow);
        }

        private void btnData_Click(object sender, EventArgs e)
        {
            if (!isDataShowOpen)
            {
                ShowDataShow();
                isDataShowOpen = true;
                isManageShowOpen = false;
            }
        }

        private void btnManage_Click(object sender, EventArgs e)
        {
            if (!isManageShowOpen)
            {
                ShowManageShow();
                isManageShowOpen = true;
                isDataShowOpen = false;
            }
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

        private void btnHome_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmHome formHome = new frmHome();
            formHome.Show();
        }

        public string UserName { get; set; }

        private void frmAdmin_Load(object sender, EventArgs e)
        {
            lblUserME.Text = Globals.Username;
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

        private void frmAdmin_FormClosed(object sender, FormClosedEventArgs e)
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
