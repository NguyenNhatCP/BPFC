namespace BPFC_System
{
    partial class frmBpfc
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBpfc));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.lblCreatedAt = new System.Windows.Forms.Label();
            this.lblCreatedBy = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tblTitle = new System.Windows.Forms.TableLayoutPanel();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.errorToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.timerDeleteLog = new System.Windows.Forms.Timer(this.components);
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.txtTemp1Outsole = new System.Windows.Forms.TextBox();
            this.txtTemp1Upper = new System.Windows.Forms.TextBox();
            this.txtTime1Outsole = new System.Windows.Forms.TextBox();
            this.txtTime1Upper = new System.Windows.Forms.TextBox();
            this.txtChemical1Outsole = new System.Windows.Forms.TextBox();
            this.txtChemical1Upper = new System.Windows.Forms.TextBox();
            this.txtTemp2Outsole = new System.Windows.Forms.TextBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label20 = new System.Windows.Forms.Label();
            this.txtTemp2Upper = new System.Windows.Forms.TextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.lvArticleToCreate = new System.Windows.Forms.ListView();
            this.label13 = new System.Windows.Forms.Label();
            this.checkboxMultiArticles = new DevExpress.XtraEditors.CheckEdit();
            this.btnDeleteArt = new DevExpress.XtraEditors.SimpleButton();
            this.btnCheckArticle = new DevExpress.XtraEditors.SimpleButton();
            this.txtModel = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtArticle = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.btnHome = new DevExpress.XtraEditors.SimpleButton();
            this.lblUserCE = new System.Windows.Forms.Label();
            this.lblLogOut = new System.Windows.Forms.Label();
            this.lblHeader = new System.Windows.Forms.Label();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txtTime2Outsole = new System.Windows.Forms.TextBox();
            this.txtTime2Upper = new System.Windows.Forms.TextBox();
            this.txtChemical2Outsole = new System.Windows.Forms.TextBox();
            this.txtChemical2Upper = new System.Windows.Forms.TextBox();
            this.txtTemp3Outsole = new System.Windows.Forms.TextBox();
            this.txtTemp3Upper = new System.Windows.Forms.TextBox();
            this.txtTime3Outsole = new System.Windows.Forms.TextBox();
            this.txtTime3Upper = new System.Windows.Forms.TextBox();
            this.txtChemical3Outsole = new System.Windows.Forms.TextBox();
            this.txtChemical3Upper = new System.Windows.Forms.TextBox();
            this.highlightTimer = new System.Windows.Forms.Timer(this.components);
            this.sqlCommand1 = new Microsoft.Data.SqlClient.SqlCommand();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tblTitle.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.checkboxMultiArticles.Properties)).BeginInit();
            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Controls.Add(this.btnSave);
            this.panel2.Controls.Add(this.lblCreatedAt);
            this.panel2.Controls.Add(this.lblCreatedBy);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 448);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(969, 74);
            this.panel2.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.Appearance.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Appearance.ForeColor = System.Drawing.Color.Black;
            this.btnCancel.Appearance.Options.UseFont = true;
            this.btnCancel.Appearance.Options.UseForeColor = true;
            this.btnCancel.Location = new System.Drawing.Point(763, 9);
            this.btnCancel.LookAndFeel.SkinMaskColor = System.Drawing.Color.SteelBlue;
            this.btnCancel.LookAndFeel.SkinName = "Glass Oceans";
            this.btnCancel.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(83, 35);
            this.btnCancel.TabIndex = 26;
            this.btnCancel.TabStop = false;
            this.btnCancel.Text = "Hủy";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Appearance.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Appearance.ForeColor = System.Drawing.Color.Black;
            this.btnSave.Appearance.Options.UseFont = true;
            this.btnSave.Appearance.Options.UseForeColor = true;
            this.btnSave.Location = new System.Drawing.Point(868, 9);
            this.btnSave.LookAndFeel.SkinMaskColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnSave.LookAndFeel.SkinName = "Glass Oceans";
            this.btnSave.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(88, 35);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Lưu";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblCreatedAt
            // 
            this.lblCreatedAt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblCreatedAt.AutoSize = true;
            this.lblCreatedAt.BackColor = System.Drawing.Color.Transparent;
            this.lblCreatedAt.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCreatedAt.ForeColor = System.Drawing.SystemColors.Control;
            this.lblCreatedAt.Location = new System.Drawing.Point(20, 40);
            this.lblCreatedAt.Name = "lblCreatedAt";
            this.lblCreatedAt.Size = new System.Drawing.Size(0, 13);
            this.lblCreatedAt.TabIndex = 23;
            this.lblCreatedAt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCreatedBy
            // 
            this.lblCreatedBy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblCreatedBy.AutoSize = true;
            this.lblCreatedBy.BackColor = System.Drawing.Color.Transparent;
            this.lblCreatedBy.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCreatedBy.ForeColor = System.Drawing.SystemColors.Control;
            this.lblCreatedBy.Location = new System.Drawing.Point(20, 20);
            this.lblCreatedBy.Name = "lblCreatedBy";
            this.lblCreatedBy.Size = new System.Drawing.Size(0, 13);
            this.lblCreatedBy.TabIndex = 23;
            this.lblCreatedBy.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.Control;
            this.label4.Location = new System.Drawing.Point(3, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(139, 35);
            this.label4.TabIndex = 10;
            this.label4.Text = "Máy 1 - Outsole";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tblTitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(969, 32);
            this.panel1.TabIndex = 8;
            // 
            // tblTitle
            // 
            this.tblTitle.ColumnCount = 4;
            this.tblTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15.00801F));
            this.tblTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.16069F));
            this.tblTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18.80354F));
            this.tblTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 52.02777F));
            this.tblTitle.Controls.Add(this.label10, 2, 0);
            this.tblTitle.Controls.Add(this.label9, 1, 0);
            this.tblTitle.Controls.Add(this.label11, 3, 0);
            this.tblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblTitle.Location = new System.Drawing.Point(0, 0);
            this.tblTitle.Name = "tblTitle";
            this.tblTitle.RowCount = 1;
            this.tblTitle.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblTitle.Size = new System.Drawing.Size(969, 32);
            this.tblTitle.TabIndex = 7;
            // 
            // label10
            // 
            this.label10.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label10.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.SystemColors.Control;
            this.label10.Location = new System.Drawing.Point(285, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(176, 32);
            this.label10.TabIndex = 5;
            this.label10.Text = "Thời gian sấy";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.SystemColors.Control;
            this.label9.Location = new System.Drawing.Point(148, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(131, 32);
            this.label9.TabIndex = 5;
            this.label9.Text = "Nhiệt độ sấy";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label11
            // 
            this.label11.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label11.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.SystemColors.Control;
            this.label11.Location = new System.Drawing.Point(467, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(499, 32);
            this.label11.TabIndex = 5;
            this.label11.Text = "Hóa chất tiêu chuẩn";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.Control;
            this.label3.Location = new System.Drawing.Point(3, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(139, 35);
            this.label3.TabIndex = 10;
            this.label3.Text = "Máy 1 - Upper";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.Dock = System.Windows.Forms.DockStyle.Top;
            this.label6.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.SystemColors.Control;
            this.label6.Location = new System.Drawing.Point(3, 147);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(139, 35);
            this.label6.TabIndex = 11;
            this.label6.Text = "Máy 2 - Outsole";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.Dock = System.Windows.Forms.DockStyle.Top;
            this.label5.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.Control;
            this.label5.Location = new System.Drawing.Point(3, 205);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(139, 35);
            this.label5.TabIndex = 11;
            this.label5.Text = "Máy 2 - Upper";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.Dock = System.Windows.Forms.DockStyle.Top;
            this.label8.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.SystemColors.Control;
            this.label8.Location = new System.Drawing.Point(3, 296);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(139, 35);
            this.label8.TabIndex = 11;
            this.label8.Text = "Máy 3 - Outsole";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.Dock = System.Windows.Forms.DockStyle.Top;
            this.label7.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.SystemColors.Control;
            this.label7.Location = new System.Drawing.Point(3, 354);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(139, 35);
            this.label7.TabIndex = 11;
            this.label7.Text = "Máy 3 - Upper";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.SystemColors.Control;
            this.label12.Location = new System.Drawing.Point(119, 49);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(193, 15);
            this.label12.TabIndex = 0;
            this.label12.Text = "Developed by the ME  Department";
            // 
            // txtLog
            // 
            this.txtLog.BackColor = System.Drawing.Color.Navy;
            this.txtLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLog.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLog.ForeColor = System.Drawing.SystemColors.Control;
            this.txtLog.Location = new System.Drawing.Point(0, 32);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(323, 416);
            this.txtLog.TabIndex = 1;
            this.txtLog.TabStop = false;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.label12);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel5.Location = new System.Drawing.Point(0, 448);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(323, 74);
            this.panel5.TabIndex = 2;
            // 
            // txtTemp1Outsole
            // 
            this.txtTemp1Outsole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTemp1Outsole.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTemp1Outsole.Location = new System.Drawing.Point(148, 3);
            this.txtTemp1Outsole.Name = "txtTemp1Outsole";
            this.txtTemp1Outsole.Size = new System.Drawing.Size(131, 32);
            this.txtTemp1Outsole.TabIndex = 0;
            this.txtTemp1Outsole.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtTemp1Outsole.Enter += new System.EventHandler(this.TextBox_Enter);
            this.txtTemp1Outsole.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTemp_KeyPress);
            this.txtTemp1Outsole.Leave += new System.EventHandler(this.TextBox_Leave);
            // 
            // txtTemp1Upper
            // 
            this.txtTemp1Upper.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTemp1Upper.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTemp1Upper.Location = new System.Drawing.Point(148, 61);
            this.txtTemp1Upper.Name = "txtTemp1Upper";
            this.txtTemp1Upper.Size = new System.Drawing.Size(131, 32);
            this.txtTemp1Upper.TabIndex = 3;
            this.txtTemp1Upper.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtTemp1Upper.Enter += new System.EventHandler(this.TextBox_Enter);
            this.txtTemp1Upper.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTemp_KeyPress);
            this.txtTemp1Upper.Leave += new System.EventHandler(this.TextBox_Leave);
            // 
            // txtTime1Outsole
            // 
            this.txtTime1Outsole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTime1Outsole.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTime1Outsole.Location = new System.Drawing.Point(285, 3);
            this.txtTime1Outsole.Name = "txtTime1Outsole";
            this.txtTime1Outsole.Size = new System.Drawing.Size(176, 32);
            this.txtTime1Outsole.TabIndex = 1;
            this.txtTime1Outsole.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtTime1Outsole.Leave += new System.EventHandler(this.txtStdTime_Leave);
            // 
            // txtTime1Upper
            // 
            this.txtTime1Upper.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTime1Upper.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTime1Upper.Location = new System.Drawing.Point(285, 61);
            this.txtTime1Upper.Name = "txtTime1Upper";
            this.txtTime1Upper.Size = new System.Drawing.Size(176, 32);
            this.txtTime1Upper.TabIndex = 4;
            this.txtTime1Upper.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtTime1Upper.Leave += new System.EventHandler(this.txtStdTime_Leave);
            // 
            // txtChemical1Outsole
            // 
            this.txtChemical1Outsole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtChemical1Outsole.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtChemical1Outsole.Location = new System.Drawing.Point(467, 3);
            this.txtChemical1Outsole.Name = "txtChemical1Outsole";
            this.txtChemical1Outsole.Size = new System.Drawing.Size(499, 32);
            this.txtChemical1Outsole.TabIndex = 2;
            this.txtChemical1Outsole.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtChemical1Upper
            // 
            this.txtChemical1Upper.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtChemical1Upper.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtChemical1Upper.Location = new System.Drawing.Point(467, 61);
            this.txtChemical1Upper.Name = "txtChemical1Upper";
            this.txtChemical1Upper.Size = new System.Drawing.Size(499, 32);
            this.txtChemical1Upper.TabIndex = 5;
            this.txtChemical1Upper.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtTemp2Outsole
            // 
            this.txtTemp2Outsole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTemp2Outsole.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTemp2Outsole.Location = new System.Drawing.Point(148, 150);
            this.txtTemp2Outsole.Name = "txtTemp2Outsole";
            this.txtTemp2Outsole.Size = new System.Drawing.Size(131, 32);
            this.txtTemp2Outsole.TabIndex = 6;
            this.txtTemp2Outsole.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtTemp2Outsole.Enter += new System.EventHandler(this.TextBox_Enter);
            this.txtTemp2Outsole.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTemp_KeyPress);
            this.txtTemp2Outsole.Leave += new System.EventHandler(this.TextBox_Leave);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.label20);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(323, 32);
            this.panel4.TabIndex = 0;
            // 
            // label20
            // 
            this.label20.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label20.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label20.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.ForeColor = System.Drawing.SystemColors.Control;
            this.label20.Location = new System.Drawing.Point(0, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(323, 32);
            this.label20.TabIndex = 5;
            this.label20.Text = "Lịch sử hoạt động:";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtTemp2Upper
            // 
            this.txtTemp2Upper.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTemp2Upper.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTemp2Upper.Location = new System.Drawing.Point(148, 208);
            this.txtTemp2Upper.Name = "txtTemp2Upper";
            this.txtTemp2Upper.Size = new System.Drawing.Size(131, 32);
            this.txtTemp2Upper.TabIndex = 9;
            this.txtTemp2Upper.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtTemp2Upper.Enter += new System.EventHandler(this.TextBox_Enter);
            this.txtTemp2Upper.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTemp_KeyPress);
            this.txtTemp2Upper.Leave += new System.EventHandler(this.TextBox_Leave);
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel3);
            this.splitContainer1.Panel1.Controls.Add(this.pnlHeader);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1298, 718);
            this.splitContainer1.SplitterDistance = 193;
            this.splitContainer1.SplitterWidth = 1;
            this.splitContainer1.TabIndex = 1;
            this.splitContainer1.TabStop = false;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel6);
            this.panel3.Controls.Add(this.checkboxMultiArticles);
            this.panel3.Controls.Add(this.btnDeleteArt);
            this.panel3.Controls.Add(this.btnCheckArticle);
            this.panel3.Controls.Add(this.txtModel);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.txtArticle);
            this.panel3.Controls.Add(this.label14);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 58);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1296, 133);
            this.panel3.TabIndex = 4;
            // 
            // panel6
            // 
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel6.Controls.Add(this.lvArticleToCreate);
            this.panel6.Controls.Add(this.label13);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel6.Location = new System.Drawing.Point(973, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(323, 133);
            this.panel6.TabIndex = 7;
            // 
            // lvArticleToCreate
            // 
            this.lvArticleToCreate.BackColor = System.Drawing.Color.Navy;
            this.lvArticleToCreate.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvArticleToCreate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvArticleToCreate.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvArticleToCreate.ForeColor = System.Drawing.SystemColors.Control;
            this.lvArticleToCreate.HideSelection = false;
            this.lvArticleToCreate.Location = new System.Drawing.Point(0, 20);
            this.lvArticleToCreate.Name = "lvArticleToCreate";
            this.lvArticleToCreate.Size = new System.Drawing.Size(319, 109);
            this.lvArticleToCreate.TabIndex = 7;
            this.lvArticleToCreate.TabStop = false;
            this.lvArticleToCreate.UseCompatibleStateImageBehavior = false;
            this.lvArticleToCreate.View = System.Windows.Forms.View.List;
            this.lvArticleToCreate.SelectedIndexChanged += new System.EventHandler(this.lvArticleToCreate_SelectedIndexChanged);
            // 
            // label13
            // 
            this.label13.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label13.Dock = System.Windows.Forms.DockStyle.Top;
            this.label13.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.SystemColors.Control;
            this.label13.Location = new System.Drawing.Point(0, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(319, 20);
            this.label13.TabIndex = 0;
            this.label13.Text = "Article cần kiểm tra:";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // checkboxMultiArticles
            // 
            this.checkboxMultiArticles.Location = new System.Drawing.Point(148, 12);
            this.checkboxMultiArticles.Name = "checkboxMultiArticles";
            this.checkboxMultiArticles.Properties.Appearance.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkboxMultiArticles.Properties.Appearance.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkboxMultiArticles.Properties.Appearance.Options.UseFont = true;
            this.checkboxMultiArticles.Properties.Appearance.Options.UseForeColor = true;
            this.checkboxMultiArticles.Properties.Caption = "";
            this.checkboxMultiArticles.Size = new System.Drawing.Size(19, 20);
            this.checkboxMultiArticles.TabIndex = 6;
            this.checkboxMultiArticles.TabStop = false;
            // 
            // btnDeleteArt
            // 
            this.btnDeleteArt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.btnDeleteArt.Appearance.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteArt.Appearance.ForeColor = System.Drawing.Color.Black;
            this.btnDeleteArt.Appearance.Options.UseFont = true;
            this.btnDeleteArt.Appearance.Options.UseForeColor = true;
            this.btnDeleteArt.Location = new System.Drawing.Point(916, 104);
            this.btnDeleteArt.LookAndFeel.SkinMaskColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnDeleteArt.LookAndFeel.SkinName = "Glass Oceans";
            this.btnDeleteArt.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnDeleteArt.Name = "btnDeleteArt";
            this.btnDeleteArt.Size = new System.Drawing.Size(53, 25);
            this.btnDeleteArt.TabIndex = 5;
            this.btnDeleteArt.TabStop = false;
            this.btnDeleteArt.Text = "Xóa";
            this.btnDeleteArt.Visible = false;
            this.btnDeleteArt.Click += new System.EventHandler(this.btnDeleteArt_Click);
            // 
            // btnCheckArticle
            // 
            this.btnCheckArticle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.btnCheckArticle.Appearance.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCheckArticle.Appearance.ForeColor = System.Drawing.Color.Black;
            this.btnCheckArticle.Appearance.Options.UseFont = true;
            this.btnCheckArticle.Appearance.Options.UseForeColor = true;
            this.btnCheckArticle.Location = new System.Drawing.Point(432, 42);
            this.btnCheckArticle.LookAndFeel.SkinMaskColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnCheckArticle.LookAndFeel.SkinName = "Glass Oceans";
            this.btnCheckArticle.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnCheckArticle.Name = "btnCheckArticle";
            this.btnCheckArticle.Size = new System.Drawing.Size(93, 32);
            this.btnCheckArticle.TabIndex = 5;
            this.btnCheckArticle.TabStop = false;
            this.btnCheckArticle.Text = "Kiểm tra";
            this.btnCheckArticle.Click += new System.EventHandler(this.btnCheckArticle_Click);
            // 
            // txtModel
            // 
            this.txtModel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.txtModel.BackColor = System.Drawing.Color.White;
            this.txtModel.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtModel.Location = new System.Drawing.Point(148, 87);
            this.txtModel.Name = "txtModel";
            this.txtModel.Size = new System.Drawing.Size(377, 29);
            this.txtModel.TabIndex = 1;
            this.txtModel.TextChanged += new System.EventHandler(this.txtModel_TextChanged);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.Control;
            this.label2.Location = new System.Drawing.Point(75, 89);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 22);
            this.label2.TabIndex = 3;
            this.label2.Text = "Model:";
            // 
            // txtArticle
            // 
            this.txtArticle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.txtArticle.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtArticle.Location = new System.Drawing.Point(148, 43);
            this.txtArticle.Name = "txtArticle";
            this.txtArticle.Size = new System.Drawing.Size(269, 29);
            this.txtArticle.TabIndex = 0;
            this.txtArticle.TextChanged += new System.EventHandler(this.txtArticle_TextChanged);
            this.txtArticle.Leave += new System.EventHandler(this.txtArticle_Leave);
            // 
            // label14
            // 
            this.label14.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.SystemColors.Control;
            this.label14.Location = new System.Drawing.Point(170, 12);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(183, 19);
            this.label14.TabIndex = 4;
            this.label14.Text = "Lưu dữ liệu cho nhiều Article";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.Control;
            this.label1.Location = new System.Drawing.Point(72, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 22);
            this.label1.TabIndex = 4;
            this.label1.Text = "Article:";
            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.btnHome);
            this.pnlHeader.Controls.Add(this.lblUserCE);
            this.pnlHeader.Controls.Add(this.lblLogOut);
            this.pnlHeader.Controls.Add(this.lblHeader);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1296, 191);
            this.pnlHeader.TabIndex = 3;
            // 
            // btnHome
            // 
            this.btnHome.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.btnHome.Appearance.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHome.Appearance.ForeColor = System.Drawing.SystemColors.Control;
            this.btnHome.Appearance.Options.UseFont = true;
            this.btnHome.Appearance.Options.UseForeColor = true;
            this.btnHome.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnHome.ImageOptions.Image")));
            this.btnHome.Location = new System.Drawing.Point(10, 10);
            this.btnHome.LookAndFeel.SkinMaskColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnHome.LookAndFeel.SkinName = "Glass Oceans";
            this.btnHome.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnHome.Name = "btnHome";
            this.btnHome.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light;
            this.btnHome.Size = new System.Drawing.Size(110, 35);
            this.btnHome.TabIndex = 5;
            this.btnHome.TabStop = false;
            this.btnHome.Text = "Trang chủ";
            this.btnHome.Click += new System.EventHandler(this.btnHome_Click);
            // 
            // lblUserCE
            // 
            this.lblUserCE.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUserCE.BackColor = System.Drawing.Color.Transparent;
            this.lblUserCE.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUserCE.ForeColor = System.Drawing.SystemColors.Control;
            this.lblUserCE.Location = new System.Drawing.Point(1159, 8);
            this.lblUserCE.Name = "lblUserCE";
            this.lblUserCE.Size = new System.Drawing.Size(126, 23);
            this.lblUserCE.TabIndex = 1;
            this.lblUserCE.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblLogOut
            // 
            this.lblLogOut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLogOut.BackColor = System.Drawing.Color.Transparent;
            this.lblLogOut.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLogOut.ForeColor = System.Drawing.Color.DarkOrange;
            this.lblLogOut.Location = new System.Drawing.Point(1205, 30);
            this.lblLogOut.Name = "lblLogOut";
            this.lblLogOut.Size = new System.Drawing.Size(78, 23);
            this.lblLogOut.TabIndex = 1;
            this.lblLogOut.Text = "Đăng xuất";
            this.lblLogOut.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblLogOut.Click += new System.EventHandler(this.lblLogOut_Click);
            this.lblLogOut.MouseEnter += new System.EventHandler(this.lblLogOut_MouseEnter);
            this.lblLogOut.MouseLeave += new System.EventHandler(this.lblLogOut_MouseLeave);
            // 
            // lblHeader
            // 
            this.lblHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblHeader.Font = new System.Drawing.Font("Times New Roman", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.ForeColor = System.Drawing.Color.DarkOrange;
            this.lblHeader.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.lblHeader.Location = new System.Drawing.Point(0, 0);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(1296, 55);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "BPFC - TIÊU CHUẨN";
            this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // splitContainer2
            // 
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.tableLayoutPanel1);
            this.splitContainer2.Panel1.Controls.Add(this.panel2);
            this.splitContainer2.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.txtLog);
            this.splitContainer2.Panel2.Controls.Add(this.panel5);
            this.splitContainer2.Panel2.Controls.Add(this.panel4);
            this.splitContainer2.Size = new System.Drawing.Size(1298, 524);
            this.splitContainer2.SplitterDistance = 971;
            this.splitContainer2.SplitterWidth = 2;
            this.splitContainer2.TabIndex = 9;
            this.splitContainer2.TabStop = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.15313F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18.7935F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 52F));
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label8, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.label7, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.txtTemp1Outsole, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtTemp1Upper, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtTime1Outsole, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtTime1Upper, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtChemical1Outsole, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtChemical1Upper, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtTemp2Outsole, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.txtTemp2Upper, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.txtTime2Outsole, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.txtTime2Upper, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.txtChemical2Outsole, 3, 3);
            this.tableLayoutPanel1.Controls.Add(this.txtChemical2Upper, 3, 4);
            this.tableLayoutPanel1.Controls.Add(this.txtTemp3Outsole, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.txtTemp3Upper, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.txtTime3Outsole, 2, 6);
            this.tableLayoutPanel1.Controls.Add(this.txtTime3Upper, 2, 7);
            this.tableLayoutPanel1.Controls.Add(this.txtChemical3Outsole, 3, 6);
            this.tableLayoutPanel1.Controls.Add(this.txtChemical3Upper, 3, 7);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 32);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 8;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.17174F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.64686F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.020154F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.03527F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.03527F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.020154F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.03527F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.03527F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(969, 416);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // txtTime2Outsole
            // 
            this.txtTime2Outsole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTime2Outsole.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTime2Outsole.Location = new System.Drawing.Point(285, 150);
            this.txtTime2Outsole.Name = "txtTime2Outsole";
            this.txtTime2Outsole.Size = new System.Drawing.Size(176, 32);
            this.txtTime2Outsole.TabIndex = 7;
            this.txtTime2Outsole.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtTime2Outsole.Leave += new System.EventHandler(this.txtStdTime_Leave);
            // 
            // txtTime2Upper
            // 
            this.txtTime2Upper.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTime2Upper.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTime2Upper.Location = new System.Drawing.Point(285, 208);
            this.txtTime2Upper.Name = "txtTime2Upper";
            this.txtTime2Upper.Size = new System.Drawing.Size(176, 32);
            this.txtTime2Upper.TabIndex = 10;
            this.txtTime2Upper.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtTime2Upper.Leave += new System.EventHandler(this.txtStdTime_Leave);
            // 
            // txtChemical2Outsole
            // 
            this.txtChemical2Outsole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtChemical2Outsole.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtChemical2Outsole.Location = new System.Drawing.Point(467, 150);
            this.txtChemical2Outsole.Name = "txtChemical2Outsole";
            this.txtChemical2Outsole.Size = new System.Drawing.Size(499, 32);
            this.txtChemical2Outsole.TabIndex = 8;
            this.txtChemical2Outsole.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtChemical2Upper
            // 
            this.txtChemical2Upper.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtChemical2Upper.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtChemical2Upper.Location = new System.Drawing.Point(467, 208);
            this.txtChemical2Upper.Name = "txtChemical2Upper";
            this.txtChemical2Upper.Size = new System.Drawing.Size(499, 32);
            this.txtChemical2Upper.TabIndex = 11;
            this.txtChemical2Upper.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtTemp3Outsole
            // 
            this.txtTemp3Outsole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTemp3Outsole.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTemp3Outsole.Location = new System.Drawing.Point(148, 299);
            this.txtTemp3Outsole.Name = "txtTemp3Outsole";
            this.txtTemp3Outsole.Size = new System.Drawing.Size(131, 32);
            this.txtTemp3Outsole.TabIndex = 12;
            this.txtTemp3Outsole.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtTemp3Outsole.Enter += new System.EventHandler(this.TextBox_Enter);
            this.txtTemp3Outsole.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTemp_KeyPress);
            this.txtTemp3Outsole.Leave += new System.EventHandler(this.TextBox_Leave);
            // 
            // txtTemp3Upper
            // 
            this.txtTemp3Upper.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTemp3Upper.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTemp3Upper.Location = new System.Drawing.Point(148, 357);
            this.txtTemp3Upper.Name = "txtTemp3Upper";
            this.txtTemp3Upper.Size = new System.Drawing.Size(131, 32);
            this.txtTemp3Upper.TabIndex = 15;
            this.txtTemp3Upper.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtTemp3Upper.Enter += new System.EventHandler(this.TextBox_Enter);
            this.txtTemp3Upper.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTemp_KeyPress);
            this.txtTemp3Upper.Leave += new System.EventHandler(this.TextBox_Leave);
            // 
            // txtTime3Outsole
            // 
            this.txtTime3Outsole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTime3Outsole.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTime3Outsole.Location = new System.Drawing.Point(285, 299);
            this.txtTime3Outsole.Name = "txtTime3Outsole";
            this.txtTime3Outsole.Size = new System.Drawing.Size(176, 32);
            this.txtTime3Outsole.TabIndex = 13;
            this.txtTime3Outsole.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtTime3Outsole.Leave += new System.EventHandler(this.txtStdTime_Leave);
            // 
            // txtTime3Upper
            // 
            this.txtTime3Upper.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTime3Upper.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTime3Upper.Location = new System.Drawing.Point(285, 357);
            this.txtTime3Upper.Name = "txtTime3Upper";
            this.txtTime3Upper.Size = new System.Drawing.Size(176, 32);
            this.txtTime3Upper.TabIndex = 16;
            this.txtTime3Upper.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtTime3Upper.Leave += new System.EventHandler(this.txtStdTime_Leave);
            // 
            // txtChemical3Outsole
            // 
            this.txtChemical3Outsole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtChemical3Outsole.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtChemical3Outsole.Location = new System.Drawing.Point(467, 299);
            this.txtChemical3Outsole.Name = "txtChemical3Outsole";
            this.txtChemical3Outsole.Size = new System.Drawing.Size(499, 32);
            this.txtChemical3Outsole.TabIndex = 14;
            this.txtChemical3Outsole.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtChemical3Upper
            // 
            this.txtChemical3Upper.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtChemical3Upper.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtChemical3Upper.Location = new System.Drawing.Point(467, 357);
            this.txtChemical3Upper.Name = "txtChemical3Upper";
            this.txtChemical3Upper.Size = new System.Drawing.Size(499, 32);
            this.txtChemical3Upper.TabIndex = 17;
            this.txtChemical3Upper.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // highlightTimer
            // 
            this.highlightTimer.Enabled = true;
            this.highlightTimer.Interval = 1000;
            // 
            // sqlCommand1
            // 
            this.sqlCommand1.CommandTimeout = 30;
            this.sqlCommand1.EnableOptimizedParameterBinding = false;
            // 
            // notifyIcon
            // 
            this.notifyIcon.Text = "notifyIcon1";
            this.notifyIcon.Visible = true;
            // 
            // frmBpfc
            // 
            this.AcceptButton = this.btnSave;
            this.Appearance.BackColor = System.Drawing.Color.Navy;
            this.Appearance.Options.UseBackColor = true;
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1298, 718);
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IconOptions.Image = global::BPFC_System.Properties.Resources.LogoAPH;
            this.MaximizeBox = false;
            this.Name = "frmBpfc";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BPFC";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmBpfc_FormClosed);
            this.Load += new System.EventHandler(this.frmBpfc_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.tblTitle.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.checkboxMultiArticles.Properties)).EndInit();
            this.pnlHeader.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblCreatedAt;
        private System.Windows.Forms.Label lblCreatedBy;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolTip errorToolTip;
        private System.Windows.Forms.Timer timerDeleteLog;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.TextBox txtTemp1Outsole;
        private System.Windows.Forms.TextBox txtTemp1Upper;
        private System.Windows.Forms.TextBox txtTime1Outsole;
        private System.Windows.Forms.TextBox txtTime1Upper;
        private System.Windows.Forms.TextBox txtChemical1Outsole;
        private System.Windows.Forms.TextBox txtChemical1Upper;
        private System.Windows.Forms.TextBox txtTemp2Outsole;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.TextBox txtTemp2Upper;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox txtModel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtArticle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlHeader;
        public System.Windows.Forms.Label lblUserCE;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox txtTime2Outsole;
        private System.Windows.Forms.TextBox txtTime2Upper;
        private System.Windows.Forms.TextBox txtChemical2Outsole;
        private System.Windows.Forms.TextBox txtChemical2Upper;
        private System.Windows.Forms.TextBox txtTemp3Outsole;
        private System.Windows.Forms.TextBox txtTemp3Upper;
        private System.Windows.Forms.TextBox txtTime3Outsole;
        private System.Windows.Forms.TextBox txtTime3Upper;
        private System.Windows.Forms.TextBox txtChemical3Outsole;
        private System.Windows.Forms.TextBox txtChemical3Upper;
        private System.Windows.Forms.Timer highlightTimer;
        private System.Windows.Forms.Label label20;
        public System.Windows.Forms.Label lblLogOut;
        private DevExpress.XtraEditors.SimpleButton btnCheckArticle;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private System.Windows.Forms.TableLayoutPanel tblTitle;
        private DevExpress.XtraEditors.SimpleButton btnHome;
        private Microsoft.Data.SqlClient.SqlCommand sqlCommand1;
        private DevExpress.XtraEditors.CheckEdit checkboxMultiArticles;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ListView lvArticleToCreate;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private DevExpress.XtraEditors.SimpleButton btnDeleteArt;
    }
}