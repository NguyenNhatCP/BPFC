namespace BPFC_System
{
    partial class frmAdmin
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAdmin));
            this.splAdmin = new System.Windows.Forms.SplitContainer();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnHome = new DevExpress.XtraEditors.SimpleButton();
            this.btnData = new DevExpress.XtraEditors.SimpleButton();
            this.btnManage = new DevExpress.XtraEditors.SimpleButton();
            this.lblUserME = new System.Windows.Forms.Label();
            this.lblLogOut = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splAdmin)).BeginInit();
            this.splAdmin.Panel1.SuspendLayout();
            this.splAdmin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splAdmin
            // 
            this.splAdmin.BackColor = System.Drawing.Color.Transparent;
            this.splAdmin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splAdmin.Location = new System.Drawing.Point(0, 0);
            this.splAdmin.Name = "splAdmin";
            this.splAdmin.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splAdmin.Panel1
            // 
            this.splAdmin.Panel1.Controls.Add(this.splitContainer1);
            this.splAdmin.Size = new System.Drawing.Size(1298, 718);
            this.splAdmin.SplitterDistance = 82;
            this.splAdmin.SplitterWidth = 2;
            this.splAdmin.TabIndex = 1;
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.flowLayoutPanel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lblUserME);
            this.splitContainer1.Panel2.Controls.Add(this.lblLogOut);
            this.splitContainer1.Size = new System.Drawing.Size(1298, 82);
            this.splitContainer1.SplitterDistance = 503;
            this.splitContainer1.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.btnHome);
            this.flowLayoutPanel1.Controls.Add(this.btnData);
            this.flowLayoutPanel1.Controls.Add(this.btnManage);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(438, 52);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // btnHome
            // 
            this.btnHome.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.btnHome.Appearance.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHome.Appearance.Options.UseFont = true;
            this.btnHome.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnHome.ImageOptions.Image")));
            this.btnHome.Location = new System.Drawing.Point(3, 3);
            this.btnHome.LookAndFeel.SkinMaskColor = System.Drawing.Color.Orange;
            this.btnHome.LookAndFeel.SkinName = "Dark Side";
            this.btnHome.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnHome.Name = "btnHome";
            this.btnHome.Size = new System.Drawing.Size(140, 45);
            this.btnHome.TabIndex = 1;
            this.btnHome.Text = "Trang chủ";
            this.btnHome.Click += new System.EventHandler(this.btnHome_Click);
            // 
            // btnData
            // 
            this.btnData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.btnData.Appearance.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnData.Appearance.Options.UseFont = true;
            this.btnData.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnData.ImageOptions.Image")));
            this.btnData.Location = new System.Drawing.Point(149, 3);
            this.btnData.LookAndFeel.SkinMaskColor = System.Drawing.Color.Orange;
            this.btnData.LookAndFeel.SkinName = "Dark Side";
            this.btnData.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnData.MinimumSize = new System.Drawing.Size(100, 45);
            this.btnData.Name = "btnData";
            this.btnData.Size = new System.Drawing.Size(140, 45);
            this.btnData.TabIndex = 0;
            this.btnData.Text = "Dữ liệu";
            this.btnData.Click += new System.EventHandler(this.btnData_Click);
            // 
            // btnManage
            // 
            this.btnManage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.btnManage.Appearance.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnManage.Appearance.Options.UseFont = true;
            this.btnManage.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnManage.ImageOptions.Image")));
            this.btnManage.Location = new System.Drawing.Point(295, 3);
            this.btnManage.LookAndFeel.SkinMaskColor = System.Drawing.Color.Orange;
            this.btnManage.LookAndFeel.SkinName = "Dark Side";
            this.btnManage.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnManage.Name = "btnManage";
            this.btnManage.Size = new System.Drawing.Size(140, 45);
            this.btnManage.TabIndex = 1;
            this.btnManage.Text = "Quản lý";
            this.btnManage.Click += new System.EventHandler(this.btnManage_Click);
            // 
            // lblUserME
            // 
            this.lblUserME.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUserME.BackColor = System.Drawing.Color.Transparent;
            this.lblUserME.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUserME.ForeColor = System.Drawing.SystemColors.Control;
            this.lblUserME.Location = new System.Drawing.Point(651, 7);
            this.lblUserME.Name = "lblUserME";
            this.lblUserME.Size = new System.Drawing.Size(126, 23);
            this.lblUserME.TabIndex = 2;
            this.lblUserME.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblLogOut
            // 
            this.lblLogOut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLogOut.BackColor = System.Drawing.Color.Transparent;
            this.lblLogOut.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLogOut.ForeColor = System.Drawing.Color.DarkOrange;
            this.lblLogOut.Location = new System.Drawing.Point(699, 30);
            this.lblLogOut.Name = "lblLogOut";
            this.lblLogOut.Size = new System.Drawing.Size(78, 23);
            this.lblLogOut.TabIndex = 3;
            this.lblLogOut.Text = "Đăng xuất";
            this.lblLogOut.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblLogOut.Click += new System.EventHandler(this.lblLogOut_Click);
            this.lblLogOut.MouseEnter += new System.EventHandler(this.lblLogOut_MouseEnter);
            this.lblLogOut.MouseLeave += new System.EventHandler(this.lblLogOut_MouseLeave);
            // 
            // frmAdmin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1298, 718);
            this.Controls.Add(this.splAdmin);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.IconOptions.Image = global::BPFC_System.Properties.Resources.LogoAPH;
            this.MaximizeBox = false;
            this.Name = "frmAdmin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Admin";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmAdmin_FormClosed);
            this.Load += new System.EventHandler(this.frmAdmin_Load);
            this.splAdmin.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splAdmin)).EndInit();
            this.splAdmin.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.SplitContainer splAdmin;
        private DevExpress.XtraEditors.SimpleButton btnData;
        private DevExpress.XtraEditors.SimpleButton btnManage;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        public System.Windows.Forms.Label lblUserME;
        public System.Windows.Forms.Label lblLogOut;
        private DevExpress.XtraEditors.SimpleButton btnHome;
    }
}