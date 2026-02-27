namespace BPFC_System
{
    partial class frmHome
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btnAdmin = new DevExpress.XtraEditors.SimpleButton();
            this.btnMe = new DevExpress.XtraEditors.SimpleButton();
            this.btnWh = new DevExpress.XtraEditors.SimpleButton();
            this.btnQip = new DevExpress.XtraEditors.SimpleButton();
            this.btnCe = new DevExpress.XtraEditors.SimpleButton();
            this.lblUser = new System.Windows.Forms.Label();
            this.lblLogOut = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.btnAdmin);
            this.splitContainer1.Panel1.Controls.Add(this.btnMe);
            this.splitContainer1.Panel1.Controls.Add(this.btnWh);
            this.splitContainer1.Panel1.Controls.Add(this.btnQip);
            this.splitContainer1.Panel1.Controls.Add(this.btnCe);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackgroundImage = global::BPFC_System.Properties.Resources.hq720;
            this.splitContainer1.Panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.splitContainer1.Panel2.Controls.Add(this.lblUser);
            this.splitContainer1.Panel2.Controls.Add(this.lblLogOut);
            this.splitContainer1.Size = new System.Drawing.Size(837, 463);
            this.splitContainer1.SplitterDistance = 213;
            this.splitContainer1.TabIndex = 0;
            // 
            // btnAdmin
            // 
            this.btnAdmin.Appearance.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdmin.Appearance.Options.UseFont = true;
            this.btnAdmin.Location = new System.Drawing.Point(16, 200);
            this.btnAdmin.LookAndFeel.SkinMaskColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnAdmin.LookAndFeel.SkinName = "Dark Side";
            this.btnAdmin.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnAdmin.Name = "btnAdmin";
            this.btnAdmin.Size = new System.Drawing.Size(171, 37);
            this.btnAdmin.TabIndex = 3;
            this.btnAdmin.Text = "QUẢN TRỊ";
            this.btnAdmin.Click += new System.EventHandler(this.btnAdmin_Click);
            // 
            // btnMe
            // 
            this.btnMe.Appearance.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMe.Appearance.Options.UseFont = true;
            this.btnMe.Location = new System.Drawing.Point(16, 153);
            this.btnMe.LookAndFeel.SkinMaskColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnMe.LookAndFeel.SkinName = "Dark Side";
            this.btnMe.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnMe.Name = "btnMe";
            this.btnMe.Size = new System.Drawing.Size(171, 37);
            this.btnMe.TabIndex = 3;
            this.btnMe.Text = "BÁO CÁO";
            this.btnMe.Click += new System.EventHandler(this.btnMe_Click);
            // 
            // btnWh
            // 
            this.btnWh.Appearance.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnWh.Appearance.Options.UseFont = true;
            this.btnWh.Location = new System.Drawing.Point(16, 106);
            this.btnWh.LookAndFeel.SkinMaskColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnWh.LookAndFeel.SkinName = "Dark Side";
            this.btnWh.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnWh.Name = "btnWh";
            this.btnWh.Size = new System.Drawing.Size(171, 37);
            this.btnWh.TabIndex = 2;
            this.btnWh.Text = "KHO HÓA CHẤT";
            this.btnWh.Click += new System.EventHandler(this.btnWh_Click);
            // 
            // btnQip
            // 
            this.btnQip.Appearance.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnQip.Appearance.Options.UseFont = true;
            this.btnQip.Location = new System.Drawing.Point(16, 59);
            this.btnQip.LookAndFeel.SkinMaskColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnQip.LookAndFeel.SkinName = "Dark Side";
            this.btnQip.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnQip.Name = "btnQip";
            this.btnQip.Size = new System.Drawing.Size(171, 37);
            this.btnQip.TabIndex = 1;
            this.btnQip.Text = "XƯỞNG - QIP";
            this.btnQip.Click += new System.EventHandler(this.btnQip_Click);
            // 
            // btnCe
            // 
            this.btnCe.Appearance.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCe.Appearance.Options.UseFont = true;
            this.btnCe.Location = new System.Drawing.Point(16, 12);
            this.btnCe.LookAndFeel.SkinMaskColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnCe.LookAndFeel.SkinName = "Dark Side";
            this.btnCe.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnCe.Name = "btnCe";
            this.btnCe.Size = new System.Drawing.Size(171, 37);
            this.btnCe.TabIndex = 0;
            this.btnCe.Text = "BPFC - CE";
            this.btnCe.Click += new System.EventHandler(this.btnCe_Click);
            // 
            // lblUser
            // 
            this.lblUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUser.BackColor = System.Drawing.Color.Transparent;
            this.lblUser.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUser.ForeColor = System.Drawing.SystemColors.Control;
            this.lblUser.Location = new System.Drawing.Point(487, 7);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(126, 23);
            this.lblUser.TabIndex = 2;
            this.lblUser.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblLogOut
            // 
            this.lblLogOut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLogOut.BackColor = System.Drawing.Color.Transparent;
            this.lblLogOut.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLogOut.ForeColor = System.Drawing.Color.DarkOrange;
            this.lblLogOut.Location = new System.Drawing.Point(528, 30);
            this.lblLogOut.Name = "lblLogOut";
            this.lblLogOut.Size = new System.Drawing.Size(78, 23);
            this.lblLogOut.TabIndex = 3;
            this.lblLogOut.Text = "Đăng xuất";
            this.lblLogOut.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblLogOut.Click += new System.EventHandler(this.lblLogOut_Click);
            this.lblLogOut.MouseEnter += new System.EventHandler(this.lblLogOut_MouseEnter);
            this.lblLogOut.MouseLeave += new System.EventHandler(this.lblLogOut_MouseLeave);
            // 
            // frmHome
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(837, 463);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.IconOptions.Image = global::BPFC_System.Properties.Resources.LogoAPH;
            this.MaximizeBox = false;
            this.Name = "frmHome";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Trang chủ";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmHome_FormClosed);
            this.Load += new System.EventHandler(this.frmHome_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private DevExpress.XtraEditors.SimpleButton btnCe;
        private DevExpress.XtraEditors.SimpleButton btnWh;
        private DevExpress.XtraEditors.SimpleButton btnQip;
        private DevExpress.XtraEditors.SimpleButton btnMe;
        public System.Windows.Forms.Label lblUser;
        public System.Windows.Forms.Label lblLogOut;
        private DevExpress.XtraEditors.SimpleButton btnAdmin;
    }
}