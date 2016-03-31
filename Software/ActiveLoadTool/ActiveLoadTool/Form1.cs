using ActiveLoadProtocol;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ActiveLoadTool
{
    public partial class Form1 : Form
    {
        ActiveLoadDevice activeLoadDevice;
        CancellationTokenSource ctsRefreshTask;

        bool connected = false;

        bool setpointChangedFromDevice = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void btRefresh_Click(object sender, EventArgs e)
        {
            string[] comPorts = SerialPort.GetPortNames();

            cbDevices.Items.Clear();

            foreach (string comPort in comPorts)
            {
                cbDevices.Items.Add(comPort);
            }

            if (cbDevices.Items.Count > 0)
            {
                cbDevices.SelectedIndex = 0;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btRefresh_Click(this, null);

            // try to load last COM port with valid connection
            if (Properties.Settings.Default.LastComPort != null)
            {
                int indexLastComPort = cbDevices.Items.IndexOf(Properties.Settings.Default.LastComPort);

                if (indexLastComPort >= 0)
                {
                    cbDevices.SelectedIndex = indexLastComPort;
                }
            }
        }

        private async void btAutoProbe_Click(object sender, EventArgs e)
        {
            // cancel running refresh task
            if (ctsRefreshTask != null)
            {
                ctsRefreshTask.Cancel();
            }

            // close device
            if (activeLoadDevice != null)
            {
                activeLoadDevice.Close();
                connected = false;
                btConnect.Text = "Connect";
            }

            // try to find all Active Load devices
            activeLoadDevice = new ActiveLoadDevice();

            btAutoProbe.Enabled = false;
            string[] comPorts = await activeLoadDevice.FindDevicesAsync();
            btAutoProbe.Enabled = true;

            cbDevices.Items.Clear();

            foreach (string comPort in comPorts)
            {
                cbDevices.Items.Add(comPort);
            }

            // select first device
            if (cbDevices.Items.Count > 0)
            {
                cbDevices.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Asynchronous polling loop which refreshes the process image.
        /// </summary>
        /// <returns></returns>
        private async Task RefreshProcessImageAsync()
        {
            gbProcessImage.Enabled = true;

            try
            {
                while (true)
                {
                    ctsRefreshTask.Token.ThrowIfCancellationRequested();

                    // get current process image
                    double actualCurrent = await activeLoadDevice.GetActualCurrentAsync();
                    double setpointCurrent = await activeLoadDevice.GetSetpointCurrentAsync();
                    double actualVoltage = await activeLoadDevice.GetActualVoltageAsync();
                    double? temperature = await activeLoadDevice.GetTemperatureAsync();
                    double dissipatedPower = actualCurrent * actualVoltage;

                    // show process image
                    lActualCurrent.Text = string.Format("{0:0.00} A", actualCurrent);
                    lActualVoltage.Text = string.Format("{0:0.00} V", actualVoltage);
                    lDissipatedPower.Text = string.Format("{0:0.0} W", dissipatedPower);

                    if (temperature != null)
                    {
                        lTemperature.Text = string.Format("{0:0.0} °C", temperature);

                        // visualize overtemperature condition
                        if (activeLoadDevice.Overtemperature)
                        {
                            lTemperature.ForeColor = Color.Red;
                            lTemperature.Font = new Font(lTemperature.Font, FontStyle.Bold);
                        }
                        else
                        {
                            lTemperature.ForeColor = Color.Black;
                            lTemperature.Font = new Font(lTemperature.Font, FontStyle.Regular);
                        }
                    }
                    else
                    {
                        lTemperature.Text = "? °C";
                    }

                    // refresh numeric up/down control, but ignore OnValueChanged event
                    setpointChangedFromDevice = true;
                    nuSetpointCurrent.Value = (decimal)setpointCurrent;
                    setpointChangedFromDevice = false;

                    // slow down refresh loop
                    await Task.Delay(100, ctsRefreshTask.Token);
                }
            }
            catch (OperationCanceledException)
            {
                // ignore, cancel requested
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString() + e.Message);

                // produce error if exception occured unexpected
                if (!ctsRefreshTask.IsCancellationRequested)
                {
                    if (e is InvalidOperationException || e is System.IO.IOException)
                    {
                        MessageBox.Show("Connection to device lost. Lost power or cable disconnected?", "Connection lost", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("Connection to device lost:\n" + e.Message, "Connection lost", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            finally
            {
                gbProcessImage.Enabled = false;
            }
        }

        private async void btConnect_Click(object sender, EventArgs e)
        {
            if (!connected)
            {
                if (cbDevices.SelectedIndex < 0)
                {
                    MessageBox.Show("Connect a device and select it in the dropdown list.", "Device selection", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // close device as a precaution (if it is open for some reason)
                if (activeLoadDevice != null)
                {
                    activeLoadDevice.Close();
                }

                // try to open specified device
                activeLoadDevice = new ActiveLoadDevice(cbDevices.Text);
                ctsRefreshTask = new CancellationTokenSource();

                try
                {
                    await activeLoadDevice.OpenAsync();
                    connected = true;
                }
                catch (UnexpectedResponseException)
                {
                    MessageBox.Show("Device is probably not an Active Load device", "Unexpected response", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (TimeoutException)
                {
                    MessageBox.Show("Device is probably not an Active Load device", "Timeout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                // close device again if not successful above
                if (!connected)
                {
                    activeLoadDevice.Close();
                }
            }
            else
            {
                // cancel running refresh task
                if (ctsRefreshTask != null)
                {
                    ctsRefreshTask.Cancel();
                }

                connected = false;
            }

            // update button text
            if (connected)
            {
                btConnect.Text = "Disconnect";

                gbProcessImage.Text = "Active load device (version: " + activeLoadDevice.Identity.Version + ")";

                if (!activeLoadDevice.IsCalibrated)
                {
                    MessageBox.Show("Device is not fully calibrated. Measurements and setpoints may be inaccurate. Consider calibration of device!", "Device calibration", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                // save COM port
                Properties.Settings.Default.LastComPort = activeLoadDevice.PortName;
                Properties.Settings.Default.Save();

                // start asynchronous polling loop task
                try
                {
                    await RefreshProcessImageAsync();
                }
                catch
                {

                }

                // polling task cancelled or error
                btConnect.Text = "Connect";

                // connection lost, disconnect and close device
                if (activeLoadDevice != null)
                {
                    activeLoadDevice.Close();
                }

                connected = false;
            }
            else
            {
                btConnect.Text = "Connect";
            }
        }

        private async void nuSetpointCurrent_ValueChanged(object sender, EventArgs e)
        {
            if (!setpointChangedFromDevice)
            {
                // value changed from GUI
                try
                {
                    await activeLoadDevice.SetSetpointCurrentAsync((double)nuSetpointCurrent.Value);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not change setpoint current (" + ex.Message + ").", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
