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
            this.cbDevices = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btConnect = new System.Windows.Forms.Button();
            this.btRefresh = new System.Windows.Forms.Button();
            this.btAutoProbe = new System.Windows.Forms.Button();
            this.gbDeviceSelection.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbDeviceSelection
            // 
            this.gbDeviceSelection.Controls.Add(this.btAutoProbe);
            this.gbDeviceSelection.Controls.Add(this.btRefresh);
            this.gbDeviceSelection.Controls.Add(this.btConnect);
            this.gbDeviceSelection.Controls.Add(this.label1);
            this.gbDeviceSelection.Controls.Add(this.cbDevices);
            this.gbDeviceSelection.Location = new System.Drawing.Point(12, 12);
            this.gbDeviceSelection.Name = "gbDeviceSelection";
            this.gbDeviceSelection.Size = new System.Drawing.Size(294, 107);
            this.gbDeviceSelection.TabIndex = 0;
            this.gbDeviceSelection.TabStop = false;
            this.gbDeviceSelection.Text = "Device connection";
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "COM port of device";
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
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(318, 278);
            this.Controls.Add(this.gbDeviceSelection);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Active Load Tool";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.gbDeviceSelection.ResumeLayout(false);
            this.gbDeviceSelection.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbDeviceSelection;
        private System.Windows.Forms.ComboBox cbDevices;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btAutoProbe;
        private System.Windows.Forms.Button btRefresh;
        private System.Windows.Forms.Button btConnect;
    }
}

