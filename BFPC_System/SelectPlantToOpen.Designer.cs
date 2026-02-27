namespace BPFC_System
{
    partial class frmSelectPlant
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
            this.label1 = new System.Windows.Forms.Label();
            this.btnOpen = new DevExpress.XtraEditors.SimpleButton();
            this.cbxPlantName = new System.Windows.Forms.ComboBox();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(288, 45);
            this.label1.TabIndex = 0;
            this.label1.Text = "HÃY CHỌN XƯỞNG ĐỂ MỞ";
            this.label1.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // btnOpen
            // 
            this.btnOpen.Appearance.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpen.Appearance.ForeColor = System.Drawing.Color.Black;
            this.btnOpen.Appearance.Options.UseFont = true;
            this.btnOpen.Appearance.Options.UseForeColor = true;
            this.btnOpen.Location = new System.Drawing.Point(153, 103);
            this.btnOpen.LookAndFeel.SkinMaskColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnOpen.LookAndFeel.SkinName = "Glass Oceans";
            this.btnOpen.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(88, 31);
            this.btnOpen.TabIndex = 1;
            this.btnOpen.Text = "Xác nhận";
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // cbxPlantName
            // 
            this.cbxPlantName.BackColor = System.Drawing.Color.WhiteSmoke;
            this.cbxPlantName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxPlantName.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxPlantName.FormattingEnabled = true;
            this.cbxPlantName.Location = new System.Drawing.Point(45, 59);
            this.cbxPlantName.Name = "cbxPlantName";
            this.cbxPlantName.Size = new System.Drawing.Size(196, 31);
            this.cbxPlantName.TabIndex = 2;
            // 
            // btnCancel
            // 
            this.btnCancel.Appearance.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Appearance.ForeColor = System.Drawing.Color.Black;
            this.btnCancel.Appearance.Options.UseFont = true;
            this.btnCancel.Appearance.Options.UseForeColor = true;
            this.btnCancel.Location = new System.Drawing.Point(45, 103);
            this.btnCancel.LookAndFeel.SkinMaskColor = System.Drawing.Color.Cyan;
            this.btnCancel.LookAndFeel.SkinName = "Glass Oceans";
            this.btnCancel.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(88, 31);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Hủy";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmSelectPlant
            // 
            this.Appearance.BackColor = System.Drawing.SystemColors.Window;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(288, 151);
            this.Controls.Add(this.cbxPlantName);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOpen);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmSelectPlant";
            this.Text = "SelectPlantToOpen";
            this.Load += new System.EventHandler(this.frmSelectPlant_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.SimpleButton btnOpen;
        private System.Windows.Forms.ComboBox cbxPlantName;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
    }
}