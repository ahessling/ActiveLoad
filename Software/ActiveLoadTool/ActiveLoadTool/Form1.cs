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
        CancellationTokenSource activeLoadCancellationTokenSource;

        bool connected = false;

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
        }

        private async void btAutoProbe_Click(object sender, EventArgs e)
        {
            // cancel running refresh task
            if (activeLoadCancellationTokenSource != null)
            {
                activeLoadCancellationTokenSource.Cancel();
            }

            // await cancellation
            await RefreshProcessImageAsync();

            // close device
            if (activeLoadDevice != null)
            {
                activeLoadDevice.Close();
                connected = false;
                btConnect.Text = "Connect";
            }

            // try to find all Active Load devices
            activeLoadDevice = new ActiveLoadDevice();

            string[] comPorts = await activeLoadDevice.FindDevicesAsync();

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
            try
            {
                while (true)
                {
                    activeLoadCancellationTokenSource.Token.ThrowIfCancellationRequested();

                    double actualCurrent = await activeLoadDevice.GetActualCurrentAsync();

                    await Task.Delay(100, activeLoadCancellationTokenSource.Token);
                    Debug.WriteLine("Actual current: " + actualCurrent);
                }
            }
            catch (OperationCanceledException)
            {
                // ignore, cancel requested
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
                activeLoadCancellationTokenSource = new CancellationTokenSource();

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
                if (activeLoadCancellationTokenSource != null)
                {
                    activeLoadCancellationTokenSource.Cancel();
                }

                // await cancellation
                await RefreshProcessImageAsync();

                // disconnect and close device
                if (activeLoadDevice != null)
                {
                    activeLoadDevice.Close();
                }

                connected = false;
            }

            // update button text
            if (connected)
            {
                btConnect.Text = "Disconnect";

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
    }
}
