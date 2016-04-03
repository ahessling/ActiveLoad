namespace ActiveLoadTool
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.gbDeviceSelection = new System.Windows.Forms.GroupBox();
            this.btAutoProbe = new System.Windows.Forms.Button();
            this.btRefresh = new System.Windows.Forms.Button();
            this.btConnect = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cbDevices = new System.Windows.Forms.ComboBox();
            this.tlProcessImage = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lActualCurrent = new System.Windows.Forms.Label();
            this.lActualVoltage = new System.Windows.Forms.Label();
            this.lDissipatedPower = new System.Windows.Forms.Label();
            this.lTemperature = new System.Windows.Forms.Label();
            this.nuSetpointCurrent = new System.Windows.Forms.NumericUpDown();
            this.gbProcessImage = new System.Windows.Forms.GroupBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.deviceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.calibrateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showgraphToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gbDeviceSelection.SuspendLayout();
            this.tlProcessImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nuSetpointCurrent)).BeginInit();
            this.gbProcessImage.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbDeviceSelection
            // 
            this.gbDeviceSelection.Controls.Add(this.btAutoProbe);
            this.gbDeviceSelection.Controls.Add(this.btRefresh);
            this.gbDeviceSelection.Controls.Add(this.btConnect);
            this.gbDeviceSelection.Controls.Add(this.label1);
            this.gbDeviceSelection.Controls.Add(this.cbDevices);
            this.gbDeviceSelection.Location = new System.Drawing.Point(12, 27);
            this.gbDeviceSelection.Name = "gbDeviceSelection";
            this.gbDeviceSelection.Size = new System.Drawing.Size(294, 107);
            this.gbDeviceSelection.TabIndex = 0;
            this.gbDeviceSelection.TabStop = false;
            this.gbDeviceSelection.Text = "Device connection";
            // 
            // btAutoProbe
            // 
            this.btAutoProbe.Location = new System.Drawing.Point(218, 69);
            this.btAutoProbe.Name = "btAutoProbe";
            this.btAutoProbe.Size = new System.Drawing.Size(70, 23);
            this.btAutoProbe.TabIndex = 4;
            this.btAutoProbe.Text = "Auto probe";
            this.btAutoProbe.UseVisualStyleBackColor = true;
            this.btAutoProbe.Click += new System.EventHandler(this.btAutoProbe_Click);
            // 
            // btRefresh
            // 
            this.btRefresh.Location = new System.Drawing.Point(218, 40);
            this.btRefresh.Name = "btRefresh";
            this.btRefresh.Size = new System.Drawing.Size(70, 23);
            this.btRefresh.TabIndex = 3;
            this.btRefresh.Text = "Refresh";
            this.btRefresh.UseVisualStyleBackColor = true;
            this.btRefresh.Click += new System.EventHandler(this.btRefresh_Click);
            // 
            // btConnect
            // 
            this.btConnect.Location = new System.Drawing.Point(6, 69);
            this.btConnect.Name = "btConnect";
            this.btConnect.Size = new System.Drawing.Size(206, 23);
            this.btConnect.TabIndex = 2;
            this.btConnect.Text = "Connect";
            this.btConnect.UseVisualStyleBackColor = true;
            this.btConnect.Click += new System.EventHandler(this.btConnect_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "COM port of device";
            // 
            // cbDevices
            // 
            this.cbDevices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDevices.FormattingEnabled = true;
            this.cbDevices.Location = new System.Drawing.Point(6, 41);
            this.cbDevices.Name = "cbDevices";
            this.cbDevices.Size = new System.Drawing.Size(206, 21);
            this.cbDevices.TabIndex = 0;
            // 
            // tlProcessImage
            // 
            this.tlProcessImage.ColumnCount = 2;
            this.tlProcessImage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlProcessImage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlProcessImage.Controls.Add(this.label2, 0, 0);
            this.tlProcessImage.Controls.Add(this.label3, 0, 1);
            this.tlProcessImage.Controls.Add(this.label4, 0, 2);
            this.tlProcessImage.Controls.Add(this.label5, 0, 3);
            this.tlProcessImage.Controls.Add(this.label6, 0, 4);
            this.tlProcessImage.Controls.Add(this.lActualCurrent, 1, 0);
            this.tlProcessImage.Controls.Add(this.lActualVoltage, 1, 2);
            this.tlProcessImage.Controls.Add(this.lDissipatedPower, 1, 3);
            this.tlProcessImage.Controls.Add(this.lTemperature, 1, 4);
            this.tlProcessImage.Controls.Add(this.nuSetpointCurrent, 1, 1);
            this.tlProcessImage.Location = new System.Drawing.Point(6, 19);
            this.tlProcessImage.Name = "tlProcessImage";
            this.tlProcessImage.RowCount = 5;
            this.tlProcessImage.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlProcessImage.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlProcessImage.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlProcessImage.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlProcessImage.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlProcessImage.Size = new System.Drawing.Size(282, 131);
            this.tlProcessImage.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Actual current";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Setpoint current";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 52);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Actual voltage";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 78);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(88, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Dissipated power";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 104);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(102, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "Temperature at load";
            // 
            // lActualCurrent
            // 
            this.lActualCurrent.AutoSize = true;
            this.lActualCurrent.Location = new System.Drawing.Point(144, 0);
            this.lActualCurrent.Name = "lActualCurrent";
            this.lActualCurrent.Size = new System.Drawing.Size(16, 13);
            this.lActualCurrent.TabIndex = 5;
            this.lActualCurrent.Text = "   ";
            // 
            // lActualVoltage
            // 
            this.lActualVoltage.AutoSize = true;
            this.lActualVoltage.Location = new System.Drawing.Point(144, 52);
            this.lActualVoltage.Name = "lActualVoltage";
            this.lActualVoltage.Size = new System.Drawing.Size(16, 13);
            this.lActualVoltage.TabIndex = 6;
            this.lActualVoltage.Text = "   ";
            // 
            // lDissipatedPower
            // 
            this.lDissipatedPower.AutoSize = true;
            this.lDissipatedPower.Location = new System.Drawing.Point(144, 78);
            this.lDissipatedPower.Name = "lDissipatedPower";
            this.lDissipatedPower.Size = new System.Drawing.Size(16, 13);
            this.lDissipatedPower.TabIndex = 7;
            this.lDissipatedPower.Text = "   ";
            // 
            // lTemperature
            // 
            this.lTemperature.AutoSize = true;
            this.lTemperature.BackColor = System.Drawing.SystemColors.Control;
            this.lTemperature.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lTemperature.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lTemperature.Location = new System.Drawing.Point(144, 104);
            this.lTemperature.Name = "lTemperature";
            this.lTemperature.Size = new System.Drawing.Size(16, 13);
            this.lTemperature.TabIndex = 8;
            this.lTemperature.Text = "   ";
            // 
            // nuSetpointCurrent
            // 
            this.nuSetpointCurrent.DecimalPlaces = 3;
            this.nuSetpointCurrent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nuSetpointCurrent.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nuSetpointCurrent.Location = new System.Drawing.Point(144, 29);
            this.nuSetpointCurrent.Maximum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.nuSetpointCurrent.Name = "nuSetpointCurrent";
            this.nuSetpointCurrent.Size = new System.Drawing.Size(135, 20);
            this.nuSetpointCurrent.TabIndex = 9;
            this.nuSetpointCurrent.ValueChanged += new System.EventHandler(this.nuSetpointCurrent_ValueChanged);
            // 
            // gbProcessImage
            // 
            this.gbProcessImage.Controls.Add(this.tlProcessImage);
            this.gbProcessImage.Enabled = false;
            this.gbProcessImage.Location = new System.Drawing.Point(12, 140);
            this.gbProcessImage.Name = "gbProcessImage";
            this.gbProcessImage.Size = new System.Drawing.Size(294, 156);
            this.gbProcessImage.TabIndex = 2;
            this.gbProcessImage.TabStop = false;
            this.gbProcessImage.Text = "Active Load device";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deviceToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(318, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // deviceToolStripMenuItem
            // 
            this.deviceToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.calibrateToolStripMenuItem,
            this.showgraphToolStripMenuItem});
            this.deviceToolStripMenuItem.Name = "deviceToolStripMenuItem";
            this.deviceToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.deviceToolStripMenuItem.Text = "&Device";
            // 
            // calibrateToolStripMenuItem
            // 
            this.calibrateToolStripMenuItem.Enabled = false;
            this.calibrateToolStripMenuItem.Name = "calibrateToolStripMenuItem";
            this.calibrateToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.calibrateToolStripMenuItem.Text = "&Calibrate";
            this.calibrateToolStripMenuItem.Click += new System.EventHandler(this.calibrateToolStripMenuItem_Click);
            // 
            // showgraphToolStripMenuItem
            // 
            this.showgraphToolStripMenuItem.Enabled = false;
            this.showgraphToolStripMenuItem.Name = "showgraphToolStripMenuItem";
            this.showgraphToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.showgraphToolStripMenuItem.Text = "Show &graph";
            this.showgraphToolStripMenuItem.Click += new System.EventHandler(this.showgraphToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(318, 312);
            this.Controls.Add(this.gbProcessImage);
            this.Controls.Add(this.gbDeviceSelection);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Active Load Tool";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.gbDeviceSelection.ResumeLayout(false);
            this.gbDeviceSelection.PerformLayout();
            this.tlProcessImage.ResumeLayout(false);
            this.tlProcessImage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nuSetpointCurrent)).EndInit();
            this.gbProcessImage.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbDeviceSelection;
        private System.Windows.Forms.ComboBox cbDevices;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btAutoProbe;
        private System.Windows.Forms.Button btRefresh;
        private System.Windows.Forms.Button btConnect;
        private System.Windows.Forms.TableLayoutPanel tlProcessImage;
        private System.Windows.Forms.GroupBox gbProcessImage;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lActualCurrent;
        private System.Windows.Forms.Label lActualVoltage;
        private System.Windows.Forms.Label lDissipatedPower;
        private System.Windows.Forms.Label lTemperature;
        private System.Windows.Forms.NumericUpDown nuSetpointCurrent;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem deviceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem calibrateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showgraphToolStripMenuItem;
    }
}

