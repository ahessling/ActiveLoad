namespace ActiveLoadTool
{
    partial class CalibrationForm
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
            this.btOK = new System.Windows.Forms.Button();
            this.gbCurrent = new System.Windows.Forms.GroupBox();
            this.tlpCurrent = new System.Windows.Forms.TableLayoutPanel();
            this.btReadCurrent1 = new System.Windows.Forms.Button();
            this.nuDeviceCurrent1 = new System.Windows.Forms.NumericUpDown();
            this.nuRealCurrent2 = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btApplyCurrent1 = new System.Windows.Forms.Button();
            this.btApplyCurrent2 = new System.Windows.Forms.Button();
            this.btRealCurrent1 = new System.Windows.Forms.Button();
            this.btRealCurrent2 = new System.Windows.Forms.Button();
            this.nuSetpointCurrent1 = new System.Windows.Forms.NumericUpDown();
            this.nuSetpointCurrent2 = new System.Windows.Forms.NumericUpDown();
            this.nuRealCurrent1 = new System.Windows.Forms.NumericUpDown();
            this.nuDeviceCurrent2 = new System.Windows.Forms.NumericUpDown();
            this.btReadCurrent2 = new System.Windows.Forms.Button();
            this.gbVoltage = new System.Windows.Forms.GroupBox();
            this.tlpVoltage = new System.Windows.Forms.TableLayoutPanel();
            this.btCalibrateCurrent = new System.Windows.Forms.Button();
            this.btCalibrateVoltage = new System.Windows.Forms.Button();
            this.btStartCurrentCalibration = new System.Windows.Forms.Button();
            this.btStartVoltageCalibration = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.nuRealVoltage1 = new System.Windows.Forms.NumericUpDown();
            this.nuDeviceVoltage1 = new System.Windows.Forms.NumericUpDown();
            this.nuRealVoltage2 = new System.Windows.Forms.NumericUpDown();
            this.nuDeviceVoltage2 = new System.Windows.Forms.NumericUpDown();
            this.btReadVoltage1 = new System.Windows.Forms.Button();
            this.btReadVoltage2 = new System.Windows.Forms.Button();
            this.gbCurrent.SuspendLayout();
            this.tlpCurrent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nuDeviceCurrent1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nuRealCurrent2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nuSetpointCurrent1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nuSetpointCurrent2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nuRealCurrent1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nuDeviceCurrent2)).BeginInit();
            this.gbVoltage.SuspendLayout();
            this.tlpVoltage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nuRealVoltage1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nuDeviceVoltage1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nuRealVoltage2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nuDeviceVoltage2)).BeginInit();
            this.SuspendLayout();
            // 
            // btOK
            // 
            this.btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOK.Location = new System.Drawing.Point(12, 273);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(600, 23);
            this.btOK.TabIndex = 0;
            this.btOK.Text = "Close";
            this.btOK.UseVisualStyleBackColor = true;
            // 
            // gbCurrent
            // 
            this.gbCurrent.Controls.Add(this.tlpCurrent);
            this.gbCurrent.Location = new System.Drawing.Point(12, 41);
            this.gbCurrent.Name = "gbCurrent";
            this.gbCurrent.Size = new System.Drawing.Size(294, 185);
            this.gbCurrent.TabIndex = 2;
            this.gbCurrent.TabStop = false;
            this.gbCurrent.Text = "Current";
            // 
            // tlpCurrent
            // 
            this.tlpCurrent.ColumnCount = 3;
            this.tlpCurrent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 42.70833F));
            this.tlpCurrent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 31.59722F));
            this.tlpCurrent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.34722F));
            this.tlpCurrent.Controls.Add(this.btReadCurrent1, 2, 1);
            this.tlpCurrent.Controls.Add(this.nuDeviceCurrent1, 1, 1);
            this.tlpCurrent.Controls.Add(this.nuRealCurrent2, 1, 5);
            this.tlpCurrent.Controls.Add(this.label1, 0, 0);
            this.tlpCurrent.Controls.Add(this.label2, 0, 1);
            this.tlpCurrent.Controls.Add(this.label3, 0, 2);
            this.tlpCurrent.Controls.Add(this.label4, 0, 3);
            this.tlpCurrent.Controls.Add(this.label5, 0, 4);
            this.tlpCurrent.Controls.Add(this.label6, 0, 5);
            this.tlpCurrent.Controls.Add(this.btApplyCurrent1, 2, 0);
            this.tlpCurrent.Controls.Add(this.btApplyCurrent2, 2, 3);
            this.tlpCurrent.Controls.Add(this.btRealCurrent1, 2, 2);
            this.tlpCurrent.Controls.Add(this.btRealCurrent2, 2, 5);
            this.tlpCurrent.Controls.Add(this.nuSetpointCurrent1, 1, 0);
            this.tlpCurrent.Controls.Add(this.nuSetpointCurrent2, 1, 3);
            this.tlpCurrent.Controls.Add(this.nuRealCurrent1, 1, 2);
            this.tlpCurrent.Controls.Add(this.nuDeviceCurrent2, 1, 4);
            this.tlpCurrent.Controls.Add(this.btReadCurrent2, 2, 4);
            this.tlpCurrent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpCurrent.Location = new System.Drawing.Point(3, 16);
            this.tlpCurrent.Name = "tlpCurrent";
            this.tlpCurrent.RowCount = 6;
            this.tlpCurrent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpCurrent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpCurrent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpCurrent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpCurrent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpCurrent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpCurrent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpCurrent.Size = new System.Drawing.Size(288, 166);
            this.tlpCurrent.TabIndex = 0;
            // 
            // btReadCurrent1
            // 
            this.btReadCurrent1.Location = new System.Drawing.Point(217, 30);
            this.btReadCurrent1.Name = "btReadCurrent1";
            this.btReadCurrent1.Size = new System.Drawing.Size(68, 21);
            this.btReadCurrent1.TabIndex = 15;
            this.btReadCurrent1.Tag = "1";
            this.btReadCurrent1.Text = "Read";
            this.btReadCurrent1.UseVisualStyleBackColor = true;
            this.btReadCurrent1.Click += new System.EventHandler(this.btReadCurrent_Click);
            // 
            // nuDeviceCurrent1
            // 
            this.nuDeviceCurrent1.DecimalPlaces = 3;
            this.nuDeviceCurrent1.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nuDeviceCurrent1.Location = new System.Drawing.Point(126, 30);
            this.nuDeviceCurrent1.Maximum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.nuDeviceCurrent1.Name = "nuDeviceCurrent1";
            this.nuDeviceCurrent1.Size = new System.Drawing.Size(85, 20);
            this.nuDeviceCurrent1.TabIndex = 13;
            // 
            // nuRealCurrent2
            // 
            this.nuRealCurrent2.DecimalPlaces = 3;
            this.nuRealCurrent2.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nuRealCurrent2.Location = new System.Drawing.Point(126, 138);
            this.nuRealCurrent2.Maximum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.nuRealCurrent2.Name = "nuRealCurrent2";
            this.nuRealCurrent2.Size = new System.Drawing.Size(85, 20);
            this.nuRealCurrent2.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Setpoint current 1 [A]";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Device current 1 [A]";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 54);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Real current 1 [A]";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 81);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(107, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Setpoint current 2 [A]";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 108);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(102, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Device current 2 [A]";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 135);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(90, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Real current 2 [A]";
            // 
            // btApplyCurrent1
            // 
            this.btApplyCurrent1.Location = new System.Drawing.Point(217, 3);
            this.btApplyCurrent1.Name = "btApplyCurrent1";
            this.btApplyCurrent1.Size = new System.Drawing.Size(68, 21);
            this.btApplyCurrent1.TabIndex = 6;
            this.btApplyCurrent1.Tag = "1";
            this.btApplyCurrent1.Text = "Set";
            this.btApplyCurrent1.UseVisualStyleBackColor = true;
            this.btApplyCurrent1.Click += new System.EventHandler(this.btApplyCurrent_Click);
            // 
            // btApplyCurrent2
            // 
            this.btApplyCurrent2.Enabled = false;
            this.btApplyCurrent2.Location = new System.Drawing.Point(217, 84);
            this.btApplyCurrent2.Name = "btApplyCurrent2";
            this.btApplyCurrent2.Size = new System.Drawing.Size(68, 21);
            this.btApplyCurrent2.TabIndex = 7;
            this.btApplyCurrent2.Tag = "2";
            this.btApplyCurrent2.Text = "Set";
            this.btApplyCurrent2.UseVisualStyleBackColor = true;
            this.btApplyCurrent2.Click += new System.EventHandler(this.btApplyCurrent_Click);
            // 
            // btRealCurrent1
            // 
            this.btRealCurrent1.Enabled = false;
            this.btRealCurrent1.Location = new System.Drawing.Point(217, 57);
            this.btRealCurrent1.Name = "btRealCurrent1";
            this.btRealCurrent1.Size = new System.Drawing.Size(68, 21);
            this.btRealCurrent1.TabIndex = 8;
            this.btRealCurrent1.Tag = "1";
            this.btRealCurrent1.Text = "Apply";
            this.btRealCurrent1.UseVisualStyleBackColor = true;
            this.btRealCurrent1.Click += new System.EventHandler(this.btRealCurrent_Click);
            // 
            // btRealCurrent2
            // 
            this.btRealCurrent2.Enabled = false;
            this.btRealCurrent2.Location = new System.Drawing.Point(217, 138);
            this.btRealCurrent2.Name = "btRealCurrent2";
            this.btRealCurrent2.Size = new System.Drawing.Size(68, 21);
            this.btRealCurrent2.TabIndex = 9;
            this.btRealCurrent2.Tag = "2";
            this.btRealCurrent2.Text = "Apply";
            this.btRealCurrent2.UseVisualStyleBackColor = true;
            this.btRealCurrent2.Click += new System.EventHandler(this.btRealCurrent_Click);
            // 
            // nuSetpointCurrent1
            // 
            this.nuSetpointCurrent1.DecimalPlaces = 3;
            this.nuSetpointCurrent1.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nuSetpointCurrent1.Location = new System.Drawing.Point(126, 3);
            this.nuSetpointCurrent1.Maximum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.nuSetpointCurrent1.Name = "nuSetpointCurrent1";
            this.nuSetpointCurrent1.Size = new System.Drawing.Size(85, 20);
            this.nuSetpointCurrent1.TabIndex = 10;
            // 
            // nuSetpointCurrent2
            // 
            this.nuSetpointCurrent2.DecimalPlaces = 3;
            this.nuSetpointCurrent2.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nuSetpointCurrent2.Location = new System.Drawing.Point(126, 84);
            this.nuSetpointCurrent2.Maximum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.nuSetpointCurrent2.Name = "nuSetpointCurrent2";
            this.nuSetpointCurrent2.Size = new System.Drawing.Size(85, 20);
            this.nuSetpointCurrent2.TabIndex = 11;
            // 
            // nuRealCurrent1
            // 
            this.nuRealCurrent1.DecimalPlaces = 3;
            this.nuRealCurrent1.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nuRealCurrent1.Location = new System.Drawing.Point(126, 57);
            this.nuRealCurrent1.Maximum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.nuRealCurrent1.Name = "nuRealCurrent1";
            this.nuRealCurrent1.Size = new System.Drawing.Size(85, 20);
            this.nuRealCurrent1.TabIndex = 12;
            // 
            // nuDeviceCurrent2
            // 
            this.nuDeviceCurrent2.DecimalPlaces = 3;
            this.nuDeviceCurrent2.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nuDeviceCurrent2.Location = new System.Drawing.Point(126, 111);
            this.nuDeviceCurrent2.Maximum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.nuDeviceCurrent2.Name = "nuDeviceCurrent2";
            this.nuDeviceCurrent2.Size = new System.Drawing.Size(85, 20);
            this.nuDeviceCurrent2.TabIndex = 14;
            // 
            // btReadCurrent2
            // 
            this.btReadCurrent2.Location = new System.Drawing.Point(217, 111);
            this.btReadCurrent2.Name = "btReadCurrent2";
            this.btReadCurrent2.Size = new System.Drawing.Size(68, 21);
            this.btReadCurrent2.TabIndex = 16;
            this.btReadCurrent2.Tag = "2";
            this.btReadCurrent2.Text = "Read";
            this.btReadCurrent2.UseVisualStyleBackColor = true;
            this.btReadCurrent2.Click += new System.EventHandler(this.btReadCurrent_Click);
            // 
            // gbVoltage
            // 
            this.gbVoltage.Controls.Add(this.tlpVoltage);
            this.gbVoltage.Location = new System.Drawing.Point(318, 41);
            this.gbVoltage.Name = "gbVoltage";
            this.gbVoltage.Size = new System.Drawing.Size(294, 135);
            this.gbVoltage.TabIndex = 3;
            this.gbVoltage.TabStop = false;
            this.gbVoltage.Text = "Voltage";
            // 
            // tlpVoltage
            // 
            this.tlpVoltage.ColumnCount = 3;
            this.tlpVoltage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 41.31944F));
            this.tlpVoltage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30.55556F));
            this.tlpVoltage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27.77778F));
            this.tlpVoltage.Controls.Add(this.nuRealVoltage1, 1, 0);
            this.tlpVoltage.Controls.Add(this.label7, 0, 0);
            this.tlpVoltage.Controls.Add(this.label8, 0, 1);
            this.tlpVoltage.Controls.Add(this.label9, 0, 2);
            this.tlpVoltage.Controls.Add(this.label10, 0, 3);
            this.tlpVoltage.Controls.Add(this.nuDeviceVoltage1, 1, 1);
            this.tlpVoltage.Controls.Add(this.nuRealVoltage2, 1, 2);
            this.tlpVoltage.Controls.Add(this.nuDeviceVoltage2, 1, 3);
            this.tlpVoltage.Controls.Add(this.btReadVoltage1, 2, 1);
            this.tlpVoltage.Controls.Add(this.btReadVoltage2, 2, 3);
            this.tlpVoltage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpVoltage.Location = new System.Drawing.Point(3, 16);
            this.tlpVoltage.Name = "tlpVoltage";
            this.tlpVoltage.RowCount = 4;
            this.tlpVoltage.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpVoltage.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpVoltage.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpVoltage.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpVoltage.Size = new System.Drawing.Size(288, 116);
            this.tlpVoltage.TabIndex = 0;
            // 
            // btCalibrateCurrent
            // 
            this.btCalibrateCurrent.Location = new System.Drawing.Point(12, 232);
            this.btCalibrateCurrent.Name = "btCalibrateCurrent";
            this.btCalibrateCurrent.Size = new System.Drawing.Size(294, 23);
            this.btCalibrateCurrent.TabIndex = 4;
            this.btCalibrateCurrent.Text = "Calibrate current";
            this.btCalibrateCurrent.UseVisualStyleBackColor = true;
            this.btCalibrateCurrent.Click += new System.EventHandler(this.btCalibrateCurrent_Click);
            // 
            // btCalibrateVoltage
            // 
            this.btCalibrateVoltage.Location = new System.Drawing.Point(318, 232);
            this.btCalibrateVoltage.Name = "btCalibrateVoltage";
            this.btCalibrateVoltage.Size = new System.Drawing.Size(294, 23);
            this.btCalibrateVoltage.TabIndex = 5;
            this.btCalibrateVoltage.Text = "Calibrate voltage";
            this.btCalibrateVoltage.UseVisualStyleBackColor = true;
            this.btCalibrateVoltage.Click += new System.EventHandler(this.btCalibrateVoltage_Click);
            // 
            // btStartCurrentCalibration
            // 
            this.btStartCurrentCalibration.Location = new System.Drawing.Point(12, 12);
            this.btStartCurrentCalibration.Name = "btStartCurrentCalibration";
            this.btStartCurrentCalibration.Size = new System.Drawing.Size(294, 23);
            this.btStartCurrentCalibration.TabIndex = 6;
            this.btStartCurrentCalibration.Text = "Reset current calibration";
            this.btStartCurrentCalibration.UseVisualStyleBackColor = true;
            this.btStartCurrentCalibration.Click += new System.EventHandler(this.btStartCurrentCalibration_Click);
            // 
            // btStartVoltageCalibration
            // 
            this.btStartVoltageCalibration.Location = new System.Drawing.Point(318, 12);
            this.btStartVoltageCalibration.Name = "btStartVoltageCalibration";
            this.btStartVoltageCalibration.Size = new System.Drawing.Size(294, 23);
            this.btStartVoltageCalibration.TabIndex = 7;
            this.btStartVoltageCalibration.Text = "Reset voltage calibration";
            this.btStartVoltageCalibration.UseVisualStyleBackColor = true;
            this.btStartVoltageCalibration.Click += new System.EventHandler(this.btStartVoltageCalibration_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(92, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Real voltage 1 [V]";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 29);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(104, 13);
            this.label8.TabIndex = 1;
            this.label8.Text = "Device voltage 1 [V]";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 58);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(92, 13);
            this.label9.TabIndex = 2;
            this.label9.Text = "Real voltage 2 [V]";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 87);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(104, 13);
            this.label10.TabIndex = 3;
            this.label10.Text = "Device voltage 2 [V]";
            // 
            // nuRealVoltage1
            // 
            this.nuRealVoltage1.DecimalPlaces = 3;
            this.nuRealVoltage1.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nuRealVoltage1.Location = new System.Drawing.Point(122, 3);
            this.nuRealVoltage1.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nuRealVoltage1.Name = "nuRealVoltage1";
            this.nuRealVoltage1.Size = new System.Drawing.Size(82, 20);
            this.nuRealVoltage1.TabIndex = 11;
            // 
            // nuDeviceVoltage1
            // 
            this.nuDeviceVoltage1.DecimalPlaces = 3;
            this.nuDeviceVoltage1.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nuDeviceVoltage1.Location = new System.Drawing.Point(122, 32);
            this.nuDeviceVoltage1.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nuDeviceVoltage1.Name = "nuDeviceVoltage1";
            this.nuDeviceVoltage1.Size = new System.Drawing.Size(82, 20);
            this.nuDeviceVoltage1.TabIndex = 12;
            // 
            // nuRealVoltage2
            // 
            this.nuRealVoltage2.DecimalPlaces = 3;
            this.nuRealVoltage2.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nuRealVoltage2.Location = new System.Drawing.Point(122, 61);
            this.nuRealVoltage2.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nuRealVoltage2.Name = "nuRealVoltage2";
            this.nuRealVoltage2.Size = new System.Drawing.Size(82, 20);
            this.nuRealVoltage2.TabIndex = 13;
            // 
            // nuDeviceVoltage2
            // 
            this.nuDeviceVoltage2.DecimalPlaces = 3;
            this.nuDeviceVoltage2.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nuDeviceVoltage2.Location = new System.Drawing.Point(122, 90);
            this.nuDeviceVoltage2.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nuDeviceVoltage2.Name = "nuDeviceVoltage2";
            this.nuDeviceVoltage2.Size = new System.Drawing.Size(82, 20);
            this.nuDeviceVoltage2.TabIndex = 14;
            // 
            // btReadVoltage1
            // 
            this.btReadVoltage1.Location = new System.Drawing.Point(210, 32);
            this.btReadVoltage1.Name = "btReadVoltage1";
            this.btReadVoltage1.Size = new System.Drawing.Size(75, 23);
            this.btReadVoltage1.TabIndex = 15;
            this.btReadVoltage1.Tag = "1";
            this.btReadVoltage1.Text = "Read";
            this.btReadVoltage1.UseVisualStyleBackColor = true;
            this.btReadVoltage1.Click += new System.EventHandler(this.btReadVoltage_Click);
            // 
            // btReadVoltage2
            // 
            this.btReadVoltage2.Location = new System.Drawing.Point(210, 90);
            this.btReadVoltage2.Name = "btReadVoltage2";
            this.btReadVoltage2.Size = new System.Drawing.Size(75, 23);
            this.btReadVoltage2.TabIndex = 16;
            this.btReadVoltage2.Tag = "2";
            this.btReadVoltage2.Text = "Read";
            this.btReadVoltage2.UseVisualStyleBackColor = true;
            this.btReadVoltage2.Click += new System.EventHandler(this.btReadVoltage_Click);
            // 
            // CalibrationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 309);
            this.Controls.Add(this.btStartVoltageCalibration);
            this.Controls.Add(this.btStartCurrentCalibration);
            this.Controls.Add(this.btCalibrateVoltage);
            this.Controls.Add(this.btCalibrateCurrent);
            this.Controls.Add(this.gbVoltage);
            this.Controls.Add(this.gbCurrent);
            this.Controls.Add(this.btOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CalibrationForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Device calibration";
            this.Load += new System.EventHandler(this.CalibrationForm_Load);
            this.gbCurrent.ResumeLayout(false);
            this.tlpCurrent.ResumeLayout(false);
            this.tlpCurrent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nuDeviceCurrent1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nuRealCurrent2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nuSetpointCurrent1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nuSetpointCurrent2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nuRealCurrent1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nuDeviceCurrent2)).EndInit();
            this.gbVoltage.ResumeLayout(false);
            this.tlpVoltage.ResumeLayout(false);
            this.tlpVoltage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nuRealVoltage1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nuDeviceVoltage1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nuRealVoltage2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nuDeviceVoltage2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.GroupBox gbCurrent;
        private System.Windows.Forms.GroupBox gbVoltage;
        private System.Windows.Forms.Button btCalibrateCurrent;
        private System.Windows.Forms.Button btCalibrateVoltage;
        private System.Windows.Forms.Button btStartCurrentCalibration;
        private System.Windows.Forms.Button btStartVoltageCalibration;
        private System.Windows.Forms.TableLayoutPanel tlpCurrent;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btApplyCurrent1;
        private System.Windows.Forms.Button btApplyCurrent2;
        private System.Windows.Forms.Button btRealCurrent1;
        private System.Windows.Forms.Button btRealCurrent2;
        private System.Windows.Forms.NumericUpDown nuSetpointCurrent1;
        private System.Windows.Forms.NumericUpDown nuSetpointCurrent2;
        private System.Windows.Forms.NumericUpDown nuRealCurrent2;
        private System.Windows.Forms.NumericUpDown nuRealCurrent1;
        private System.Windows.Forms.Button btReadCurrent1;
        private System.Windows.Forms.NumericUpDown nuDeviceCurrent1;
        private System.Windows.Forms.NumericUpDown nuDeviceCurrent2;
        private System.Windows.Forms.Button btReadCurrent2;
        private System.Windows.Forms.TableLayoutPanel tlpVoltage;
        private System.Windows.Forms.NumericUpDown nuRealVoltage1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown nuDeviceVoltage1;
        private System.Windows.Forms.NumericUpDown nuRealVoltage2;
        private System.Windows.Forms.NumericUpDown nuDeviceVoltage2;
        private System.Windows.Forms.Button btReadVoltage1;
        private System.Windows.Forms.Button btReadVoltage2;
    }
}