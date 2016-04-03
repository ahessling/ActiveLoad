namespace ActiveLoadTool
{
    partial class GraphForm
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.tmrRefresh = new System.Windows.Forms.Timer(this.components);
            this.tlpGraphs = new System.Windows.Forms.TableLayoutPanel();
            this.chartActiveLoad = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.chkVoltageVisible = new System.Windows.Forms.CheckBox();
            this.chkPowerVisible = new System.Windows.Forms.CheckBox();
            this.chkTemperatureVisible = new System.Windows.Forms.CheckBox();
            this.chkRun = new System.Windows.Forms.CheckBox();
            this.cbInterval = new System.Windows.Forms.ComboBox();
            this.cbSamples = new System.Windows.Forms.ComboBox();
            this.tlpGraphs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartActiveLoad)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tmrRefresh
            // 
            this.tmrRefresh.Enabled = true;
            this.tmrRefresh.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // tlpGraphs
            // 
            this.tlpGraphs.ColumnCount = 1;
            this.tlpGraphs.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpGraphs.Controls.Add(this.chartActiveLoad, 0, 0);
            this.tlpGraphs.Controls.Add(this.flowLayoutPanel1, 0, 1);
            this.tlpGraphs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpGraphs.Location = new System.Drawing.Point(0, 0);
            this.tlpGraphs.Name = "tlpGraphs";
            this.tlpGraphs.RowCount = 2;
            this.tlpGraphs.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpGraphs.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpGraphs.Size = new System.Drawing.Size(1001, 561);
            this.tlpGraphs.TabIndex = 0;
            // 
            // chartActiveLoad
            // 
            chartArea1.AxisX.Title = "Time [ms]";
            chartArea1.AxisY.Title = "Current [A]";
            chartArea1.Name = "ChartAreaCurrent";
            chartArea2.AlignWithChartArea = "ChartAreaCurrent";
            chartArea2.AxisX.Title = "Time [ms]";
            chartArea2.AxisY.Minimum = 0D;
            chartArea2.AxisY.Title = "Voltage [V]";
            chartArea2.AxisY2.Title = "Power diss. [W]";
            chartArea2.Name = "ChartAreaVoltage";
            chartArea3.AlignWithChartArea = "ChartAreaCurrent";
            chartArea3.AxisX.Title = "Time [ms]";
            chartArea3.AxisY.Title = "Temp. [°C]";
            chartArea3.AxisY2.Interval = 5D;
            chartArea3.AxisY2.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            chartArea3.AxisY2.Title = "Power diss. [W]";
            chartArea3.Name = "ChartAreaPowerTemperature";
            this.chartActiveLoad.ChartAreas.Add(chartArea1);
            this.chartActiveLoad.ChartAreas.Add(chartArea2);
            this.chartActiveLoad.ChartAreas.Add(chartArea3);
            this.chartActiveLoad.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.DockedToChartArea = "ChartAreaCurrent";
            legend1.IsDockedInsideChartArea = false;
            legend1.Name = "Current";
            legend2.DockedToChartArea = "ChartAreaVoltage";
            legend2.IsDockedInsideChartArea = false;
            legend2.Name = "Voltage";
            legend3.DockedToChartArea = "ChartAreaPowerTemperature";
            legend3.IsDockedInsideChartArea = false;
            legend3.Name = "TemperaturePower";
            this.chartActiveLoad.Legends.Add(legend1);
            this.chartActiveLoad.Legends.Add(legend2);
            this.chartActiveLoad.Legends.Add(legend3);
            this.chartActiveLoad.Location = new System.Drawing.Point(3, 3);
            this.chartActiveLoad.Name = "chartActiveLoad";
            series1.BorderWidth = 2;
            series1.ChartArea = "ChartAreaCurrent";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Color = System.Drawing.Color.Red;
            series1.Legend = "Current";
            series1.Name = "Actual current";
            series2.BorderWidth = 2;
            series2.ChartArea = "ChartAreaCurrent";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            series2.Legend = "Current";
            series2.Name = "Setpoint current";
            series3.BorderWidth = 2;
            series3.ChartArea = "ChartAreaVoltage";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series3.Color = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            series3.Legend = "Voltage";
            series3.Name = "Actual voltage";
            series4.BorderWidth = 2;
            series4.ChartArea = "ChartAreaPowerTemperature";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series4.Color = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            series4.Legend = "TemperaturePower";
            series4.Name = "Dissipated power";
            series4.YAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Secondary;
            series5.BorderWidth = 2;
            series5.ChartArea = "ChartAreaPowerTemperature";
            series5.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series5.Color = System.Drawing.Color.Blue;
            series5.Legend = "TemperaturePower";
            series5.Name = "Temperature";
            this.chartActiveLoad.Series.Add(series1);
            this.chartActiveLoad.Series.Add(series2);
            this.chartActiveLoad.Series.Add(series3);
            this.chartActiveLoad.Series.Add(series4);
            this.chartActiveLoad.Series.Add(series5);
            this.chartActiveLoad.Size = new System.Drawing.Size(995, 520);
            this.chartActiveLoad.TabIndex = 3;
            this.chartActiveLoad.Text = "Current";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.chkVoltageVisible);
            this.flowLayoutPanel1.Controls.Add(this.chkPowerVisible);
            this.flowLayoutPanel1.Controls.Add(this.chkTemperatureVisible);
            this.flowLayoutPanel1.Controls.Add(this.chkRun);
            this.flowLayoutPanel1.Controls.Add(this.cbInterval);
            this.flowLayoutPanel1.Controls.Add(this.cbSamples);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 529);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(995, 29);
            this.flowLayoutPanel1.TabIndex = 4;
            // 
            // chkVoltageVisible
            // 
            this.chkVoltageVisible.AutoSize = true;
            this.chkVoltageVisible.Checked = true;
            this.chkVoltageVisible.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkVoltageVisible.Location = new System.Drawing.Point(3, 3);
            this.chkVoltageVisible.Name = "chkVoltageVisible";
            this.chkVoltageVisible.Size = new System.Drawing.Size(94, 17);
            this.chkVoltageVisible.TabIndex = 2;
            this.chkVoltageVisible.Text = "Voltage visible";
            this.chkVoltageVisible.UseVisualStyleBackColor = true;
            this.chkVoltageVisible.CheckedChanged += new System.EventHandler(this.chkVoltageVisible_CheckedChanged);
            // 
            // chkPowerVisible
            // 
            this.chkPowerVisible.AutoSize = true;
            this.chkPowerVisible.Checked = true;
            this.chkPowerVisible.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPowerVisible.Location = new System.Drawing.Point(103, 3);
            this.chkPowerVisible.Name = "chkPowerVisible";
            this.chkPowerVisible.Size = new System.Drawing.Size(88, 17);
            this.chkPowerVisible.TabIndex = 0;
            this.chkPowerVisible.Text = "Power visible";
            this.chkPowerVisible.UseVisualStyleBackColor = true;
            this.chkPowerVisible.CheckedChanged += new System.EventHandler(this.chkPowerVisible_CheckedChanged);
            // 
            // chkTemperatureVisible
            // 
            this.chkTemperatureVisible.AutoSize = true;
            this.chkTemperatureVisible.Checked = true;
            this.chkTemperatureVisible.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTemperatureVisible.Location = new System.Drawing.Point(197, 3);
            this.chkTemperatureVisible.Name = "chkTemperatureVisible";
            this.chkTemperatureVisible.Size = new System.Drawing.Size(118, 17);
            this.chkTemperatureVisible.TabIndex = 1;
            this.chkTemperatureVisible.Text = "Temperature visible";
            this.chkTemperatureVisible.UseVisualStyleBackColor = true;
            this.chkTemperatureVisible.CheckedChanged += new System.EventHandler(this.chkPowerVisible_CheckedChanged);
            // 
            // chkRun
            // 
            this.chkRun.AutoSize = true;
            this.chkRun.Checked = true;
            this.chkRun.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRun.Location = new System.Drawing.Point(321, 3);
            this.chkRun.Name = "chkRun";
            this.chkRun.Size = new System.Drawing.Size(46, 17);
            this.chkRun.TabIndex = 3;
            this.chkRun.Text = "Run";
            this.chkRun.UseVisualStyleBackColor = true;
            this.chkRun.CheckedChanged += new System.EventHandler(this.chkRun_CheckedChanged);
            // 
            // cbInterval
            // 
            this.cbInterval.FormattingEnabled = true;
            this.cbInterval.Location = new System.Drawing.Point(373, 3);
            this.cbInterval.Name = "cbInterval";
            this.cbInterval.Size = new System.Drawing.Size(121, 21);
            this.cbInterval.TabIndex = 4;
            this.cbInterval.SelectedIndexChanged += new System.EventHandler(this.cbInterval_SelectedIndexChanged);
            this.cbInterval.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cbInterval_KeyPress);
            // 
            // cbSamples
            // 
            this.cbSamples.FormattingEnabled = true;
            this.cbSamples.Location = new System.Drawing.Point(500, 3);
            this.cbSamples.Name = "cbSamples";
            this.cbSamples.Size = new System.Drawing.Size(121, 21);
            this.cbSamples.TabIndex = 5;
            this.cbSamples.SelectedIndexChanged += new System.EventHandler(this.cbSamples_SelectedIndexChanged);
            this.cbSamples.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cbSamples_KeyPress);
            // 
            // GraphForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1001, 561);
            this.Controls.Add(this.tlpGraphs);
            this.MinimumSize = new System.Drawing.Size(640, 480);
            this.Name = "GraphForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Active Load Graph";
            this.tlpGraphs.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartActiveLoad)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer tmrRefresh;
        private System.Windows.Forms.TableLayoutPanel tlpGraphs;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartActiveLoad;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.CheckBox chkPowerVisible;
        private System.Windows.Forms.CheckBox chkTemperatureVisible;
        private System.Windows.Forms.CheckBox chkVoltageVisible;
        private System.Windows.Forms.CheckBox chkRun;
        private System.Windows.Forms.ComboBox cbInterval;
        private System.Windows.Forms.ComboBox cbSamples;
    }
}