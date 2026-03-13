namespace BPFC_System
{
    partial class frmCheckDiffLine
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCheckDiffLine));
            this.tablePanel1 = new DevExpress.Utils.Layout.TablePanel();
            this.tablePanel2 = new DevExpress.Utils.Layout.TablePanel();
            this.btnCheck = new DevExpress.XtraEditors.SimpleButton();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.btnHome = new DevExpress.XtraEditors.SimpleButton();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).BeginInit();
            this.tablePanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel2)).BeginInit();
            this.tablePanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // tablePanel1
            // 
            this.tablePanel1.Columns.AddRange(new DevExpress.Utils.Layout.TablePanelColumn[] {
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 16.97F)});
            this.tablePanel1.Controls.Add(this.dataGridView);
            this.tablePanel1.Controls.Add(this.tablePanel2);
            this.tablePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tablePanel1.Location = new System.Drawing.Point(0, 0);
            this.tablePanel1.Name = "tablePanel1";
            this.tablePanel1.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] {
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 65F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F)});
            this.tablePanel1.Size = new System.Drawing.Size(839, 527);
            this.tablePanel1.TabIndex = 0;
            this.tablePanel1.UseSkinIndents = true;
            // 
            // tablePanel2
            // 
            this.tablePanel1.SetColumn(this.tablePanel2, 0);
            this.tablePanel2.Columns.AddRange(new DevExpress.Utils.Layout.TablePanelColumn[] {
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 18.99F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 38.71F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 19.72F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 53.74F)});
            this.tablePanel2.Controls.Add(this.dtpDate);
            this.tablePanel2.Controls.Add(this.btnHome);
            this.tablePanel2.Controls.Add(this.btnCheck);
            this.tablePanel2.Location = new System.Drawing.Point(13, 12);
            this.tablePanel2.Name = "tablePanel2";
            this.tablePanel1.SetRow(this.tablePanel2, 0);
            this.tablePanel2.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] {
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 32F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F)});
            this.tablePanel2.Size = new System.Drawing.Size(813, 61);
            this.tablePanel2.TabIndex = 0;
            this.tablePanel2.UseSkinIndents = true;
            // 
            // btnCheck
            // 
            this.tablePanel2.SetColumn(this.btnCheck, 2);
            this.btnCheck.Location = new System.Drawing.Point(361, 14);
            this.btnCheck.Name = "btnCheck";
            this.tablePanel2.SetRow(this.btnCheck, 0);
            this.btnCheck.Size = new System.Drawing.Size(115, 23);
            this.btnCheck.TabIndex = 1;
            this.btnCheck.Text = "Kiểm tra";
            this.btnCheck.Click += new System.EventHandler(this.btnCompare_Click);
            // 
            // dataGridView
            // 
            this.tablePanel1.SetColumn(this.dataGridView, 0);
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.Location = new System.Drawing.Point(13, 77);
            this.dataGridView.Name = "dataGridView";
            this.tablePanel1.SetRow(this.dataGridView, 1);
            this.dataGridView.Size = new System.Drawing.Size(813, 437);
            this.dataGridView.TabIndex = 1;
            // 
            // btnHome
            // 
            this.btnHome.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.btnHome.Appearance.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHome.Appearance.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnHome.Appearance.Options.UseFont = true;
            this.btnHome.Appearance.Options.UseForeColor = true;
            this.tablePanel2.SetColumn(this.btnHome, 0);
            this.btnHome.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnHome.ImageOptions.Image")));
            this.btnHome.Location = new System.Drawing.Point(13, 12);
            this.btnHome.LookAndFeel.SkinMaskColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnHome.LookAndFeel.SkinName = "Pumpkin";
            this.btnHome.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnHome.MaximumSize = new System.Drawing.Size(100, 29);
            this.btnHome.MinimumSize = new System.Drawing.Size(100, 29);
            this.btnHome.Name = "btnHome";
            this.tablePanel2.SetRow(this.btnHome, 0);
            this.btnHome.Size = new System.Drawing.Size(100, 29);
            this.btnHome.TabIndex = 11;
            this.btnHome.TabStop = false;
            this.btnHome.Text = "Trang chủ";
            this.btnHome.Click += new System.EventHandler(this.btnHome_Click);
            // 
            // dtpDate
            // 
            this.dtpDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dtpDate.CalendarFont = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tablePanel2.SetColumn(this.dtpDate, 1);
            this.dtpDate.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDate.Location = new System.Drawing.Point(128, 12);
            this.dtpDate.MaxDate = new System.DateTime(2027, 1, 1, 0, 0, 0, 0);
            this.dtpDate.Name = "dtpDate";
            this.tablePanel2.SetRow(this.dtpDate, 0);
            this.dtpDate.Size = new System.Drawing.Size(229, 29);
            this.dtpDate.TabIndex = 12;
            this.dtpDate.Value = new System.DateTime(2023, 12, 26, 0, 0, 0, 0);
            // 
            // frmCheckDiffLine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(839, 527);
            this.Controls.Add(this.tablePanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmCheckDiffLine";
            this.Text = "Check Different";
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).EndInit();
            this.tablePanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel2)).EndInit();
            this.tablePanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.Utils.Layout.TablePanel tablePanel1;
        private DevExpress.Utils.Layout.TablePanel tablePanel2;
        private DevExpress.XtraEditors.SimpleButton btnCheck;
        private System.Windows.Forms.DataGridView dataGridView;
        private DevExpress.XtraEditors.SimpleButton btnHome;
        private System.Windows.Forms.DateTimePicker dtpDate;
    }
}