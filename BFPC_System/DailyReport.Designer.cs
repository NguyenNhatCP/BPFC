namespace BPFC_System
{
    partial class DailyReport
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.dgvLineDetail = new System.Windows.Forms.DataGridView();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.dtpReportDate = new System.Windows.Forms.DateTimePicker();
            this.cbxPlantNames = new System.Windows.Forms.ComboBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.dgvDailyReport = new System.Windows.Forms.DataGridView();
            this.pnlChart = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLineDetail)).BeginInit();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDailyReport)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.dgvLineDetail);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 423);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1105, 203);
            this.panel1.TabIndex = 0;
            // 
            // dgvLineDetail
            // 
            this.dgvLineDetail.BackgroundColor = System.Drawing.Color.LightSteelBlue;
            this.dgvLineDetail.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvLineDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLineDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvLineDetail.Location = new System.Drawing.Point(0, 47);
            this.dgvLineDetail.Name = "dgvLineDetail";
            this.dgvLineDetail.ReadOnly = true;
            this.dgvLineDetail.Size = new System.Drawing.Size(1101, 152);
            this.dgvLineDetail.TabIndex = 2;
            this.dgvLineDetail.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvLineDetail_CellFormatting);
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.LightSteelBlue;
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel4.Controls.Add(this.label2);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1101, 47);
            this.panel4.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(14, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(141, 35);
            this.label2.TabIndex = 25;
            this.label2.Text = "Dữ liệu chi tiết:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dtpReportDate
            // 
            this.dtpReportDate.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpReportDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpReportDate.Location = new System.Drawing.Point(14, 21);
            this.dtpReportDate.Name = "dtpReportDate";
            this.dtpReportDate.Size = new System.Drawing.Size(246, 26);
            this.dtpReportDate.TabIndex = 22;
            this.dtpReportDate.ValueChanged += new System.EventHandler(this.dtpReportDate_ValueChanged);
            // 
            // cbxPlantNames
            // 
            this.cbxPlantNames.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.cbxPlantNames.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbxPlantNames.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxPlantNames.FormattingEnabled = true;
            this.cbxPlantNames.Location = new System.Drawing.Point(709, 20);
            this.cbxPlantNames.Name = "cbxPlantNames";
            this.cbxPlantNames.Size = new System.Drawing.Size(125, 27);
            this.cbxPlantNames.TabIndex = 23;
            this.cbxPlantNames.SelectedIndexChanged += new System.EventHandler(this.cbxPlantNames_SelectedIndexChanged);
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel3.Controls.Add(this.dgvDailyReport);
            this.panel3.Controls.Add(this.pnlChart);
            this.panel3.Controls.Add(this.panel2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1105, 423);
            this.panel3.TabIndex = 1;
            // 
            // dgvDailyReport
            // 
            this.dgvDailyReport.BackgroundColor = System.Drawing.Color.LightSteelBlue;
            this.dgvDailyReport.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvDailyReport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDailyReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDailyReport.Location = new System.Drawing.Point(599, 60);
            this.dgvDailyReport.Name = "dgvDailyReport";
            this.dgvDailyReport.ReadOnly = true;
            this.dgvDailyReport.Size = new System.Drawing.Size(502, 359);
            this.dgvDailyReport.TabIndex = 3;
            this.dgvDailyReport.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDailyReport_CellDoubleClick);
            this.dgvDailyReport.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvDailyReport_CellFormatting);
            // 
            // pnlChart
            // 
            this.pnlChart.BackColor = System.Drawing.Color.AliceBlue;
            this.pnlChart.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlChart.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlChart.Location = new System.Drawing.Point(0, 60);
            this.pnlChart.Name = "pnlChart";
            this.pnlChart.Size = new System.Drawing.Size(599, 359);
            this.pnlChart.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.LightSteelBlue;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.cbxPlantNames);
            this.panel2.Controls.Add(this.dtpReportDate);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1101, 60);
            this.panel2.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(597, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 27);
            this.label1.TabIndex = 24;
            this.label1.Text = "Chọn xưởng:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DailyReport
            // 
            this.Appearance.BackColor = System.Drawing.Color.LightSteelBlue;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Name = "DailyReport";
            this.Size = new System.Drawing.Size(1105, 626);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLineDetail)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDailyReport)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.DateTimePicker dtpReportDate;
        private System.Windows.Forms.ComboBox cbxPlantNames;
        private System.Windows.Forms.Panel pnlChart;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dgvDailyReport;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.DataGridView dgvLineDetail;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}
