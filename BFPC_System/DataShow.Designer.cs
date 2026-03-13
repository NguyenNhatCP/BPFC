namespace BPFC_System
{
    partial class DataShow
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataShow));
            this.panel1 = new System.Windows.Forms.Panel();
            this.splManage = new System.Windows.Forms.SplitContainer();
            this.sqlCommand1 = new Microsoft.Data.SqlClient.SqlCommand();
            this.sqlCommand2 = new Microsoft.Data.SqlClient.SqlCommand();
            this.btnRandomCheck = new DevExpress.XtraEditors.SimpleButton();
            this.btnComposeEmail = new DevExpress.XtraEditors.SimpleButton();
            this.btnExport = new DevExpress.XtraEditors.SimpleButton();
            this.btnUserManage = new DevExpress.XtraEditors.SimpleButton();
            this.btnCheck = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.splManage)).BeginInit();
            this.splManage.Panel1.SuspendLayout();
            this.splManage.Panel2.SuspendLayout();
            this.splManage.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1109, 630);
            this.panel1.TabIndex = 0;
            // 
            // splManage
            // 
            this.splManage.BackColor = System.Drawing.Color.Transparent;
            this.splManage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splManage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splManage.IsSplitterFixed = true;
            this.splManage.Location = new System.Drawing.Point(0, 0);
            this.splManage.Name = "splManage";
            // 
            // splManage.Panel1
            // 
            this.splManage.Panel1.BackColor = System.Drawing.Color.Transparent;
            this.splManage.Panel1.Controls.Add(this.btnCheck);
            this.splManage.Panel1.Controls.Add(this.btnRandomCheck);
            this.splManage.Panel1.Controls.Add(this.btnComposeEmail);
            this.splManage.Panel1.Controls.Add(this.btnExport);
            this.splManage.Panel1.Controls.Add(this.btnUserManage);
            // 
            // splManage.Panel2
            // 
            this.splManage.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.splManage.Panel2.Controls.Add(this.panel1);
            this.splManage.Size = new System.Drawing.Size(1298, 634);
            this.splManage.SplitterDistance = 184;
            this.splManage.SplitterWidth = 1;
            this.splManage.TabIndex = 2;
            // 
            // sqlCommand1
            // 
            this.sqlCommand1.CommandTimeout = 30;
            this.sqlCommand1.EnableOptimizedParameterBinding = false;
            // 
            // sqlCommand2
            // 
            this.sqlCommand2.CommandTimeout = 30;
            this.sqlCommand2.EnableOptimizedParameterBinding = false;
            // 
            // btnRandomCheck
            // 
            this.btnRandomCheck.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRandomCheck.Appearance.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRandomCheck.Appearance.Options.UseFont = true;
            this.btnRandomCheck.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnRandomCheck.ImageOptions.Image")));
            this.btnRandomCheck.Location = new System.Drawing.Point(15, 175);
            this.btnRandomCheck.LookAndFeel.SkinMaskColor = System.Drawing.Color.SkyBlue;
            this.btnRandomCheck.LookAndFeel.SkinName = "Coffee";
            this.btnRandomCheck.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnRandomCheck.Name = "btnRandomCheck";
            this.btnRandomCheck.Size = new System.Drawing.Size(154, 40);
            this.btnRandomCheck.TabIndex = 1;
            this.btnRandomCheck.Text = "Kiểm tra ngẫu nhiên";
            this.btnRandomCheck.Click += new System.EventHandler(this.btnRandomCheck_Click);
            // 
            // btnComposeEmail
            // 
            this.btnComposeEmail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnComposeEmail.Appearance.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnComposeEmail.Appearance.Options.UseFont = true;
            this.btnComposeEmail.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnComposeEmail.ImageOptions.Image")));
            this.btnComposeEmail.Location = new System.Drawing.Point(15, 120);
            this.btnComposeEmail.LookAndFeel.SkinMaskColor = System.Drawing.Color.SkyBlue;
            this.btnComposeEmail.LookAndFeel.SkinName = "Coffee";
            this.btnComposeEmail.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnComposeEmail.Name = "btnComposeEmail";
            this.btnComposeEmail.Size = new System.Drawing.Size(154, 40);
            this.btnComposeEmail.TabIndex = 0;
            this.btnComposeEmail.Text = "Gửi mail";
            this.btnComposeEmail.Click += new System.EventHandler(this.btnComposeEmail_Click);
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.Appearance.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExport.Appearance.Options.UseFont = true;
            this.btnExport.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnExport.ImageOptions.Image")));
            this.btnExport.Location = new System.Drawing.Point(15, 65);
            this.btnExport.LookAndFeel.SkinMaskColor = System.Drawing.Color.SkyBlue;
            this.btnExport.LookAndFeel.SkinName = "Coffee";
            this.btnExport.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(154, 40);
            this.btnExport.TabIndex = 0;
            this.btnExport.Text = "Xem báo cáo";
            this.btnExport.Click += new System.EventHandler(this.btnViewExport_Click);
            // 
            // btnUserManage
            // 
            this.btnUserManage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUserManage.Appearance.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUserManage.Appearance.Options.UseFont = true;
            this.btnUserManage.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnUserManage.ImageOptions.Image")));
            this.btnUserManage.Location = new System.Drawing.Point(15, 10);
            this.btnUserManage.LookAndFeel.SkinMaskColor = System.Drawing.Color.SkyBlue;
            this.btnUserManage.LookAndFeel.SkinName = "Coffee";
            this.btnUserManage.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnUserManage.Name = "btnUserManage";
            this.btnUserManage.Size = new System.Drawing.Size(154, 40);
            this.btnUserManage.TabIndex = 0;
            this.btnUserManage.Text = "Hằng ngày";
            this.btnUserManage.Click += new System.EventHandler(this.btnDaily_Click);
            // 
            // btnCheck
            // 
            this.btnCheck.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCheck.Appearance.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCheck.Appearance.Options.UseFont = true;
            this.btnCheck.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.ImageOptions.Image")));
            this.btnCheck.Location = new System.Drawing.Point(15, 236);
            this.btnCheck.LookAndFeel.SkinMaskColor = System.Drawing.Color.SkyBlue;
            this.btnCheck.LookAndFeel.SkinName = "Coffee";
            this.btnCheck.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnCheck.Name = "btnCheck";
            this.btnCheck.Size = new System.Drawing.Size(154, 40);
            this.btnCheck.TabIndex = 2;
            this.btnCheck.Text = "Kiểm tra ART";
            this.btnCheck.Click += new System.EventHandler(this.btnCheck_Click);
            // 
            // DataShow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.Controls.Add(this.splManage);
            this.Name = "DataShow";
            this.Size = new System.Drawing.Size(1298, 634);
            this.Load += new System.EventHandler(this.DataShow_Load);
            this.splManage.Panel1.ResumeLayout(false);
            this.splManage.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splManage)).EndInit();
            this.splManage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.SplitContainer splManage;
        private DevExpress.XtraEditors.SimpleButton btnUserManage;
        private Microsoft.Data.SqlClient.SqlCommand sqlCommand1;
        private DevExpress.XtraEditors.SimpleButton btnExport;
        private Microsoft.Data.SqlClient.SqlCommand sqlCommand2;
        private DevExpress.XtraEditors.SimpleButton btnComposeEmail;
        private DevExpress.XtraEditors.SimpleButton btnRandomCheck;
        private DevExpress.XtraEditors.SimpleButton btnCheck;
    }
}
