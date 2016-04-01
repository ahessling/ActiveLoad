using ActiveLoadProtocol;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ActiveLoadTool
{
    public partial class CalibrationForm : Form
    {
        public ActiveLoadDevice.CalibrationData Calibration;
        ActiveLoadDevice activeLoadDevice;

        public CalibrationForm(ActiveLoadDevice activeLoadDevice)
        {
            InitializeComponent();

            this.activeLoadDevice = activeLoadDevice;
            Calibration = activeLoadDevice.Calibration;

            // current calibration data
            if (Calibration.Current != null)
            {
                // copy current values to form
                nuSetpointCurrent1.Value = (decimal)Calibration.Current.DeviceCurrentSetpoint[0];
                nuSetpointCurrent2.Value = (decimal)Calibration.Current.DeviceCurrentSetpoint[1];
                nuRealCurrent1.Value = (decimal)Calibration.Current.ActualCurrent[0];
                nuRealCurrent2.Value = (decimal)Calibration.Current.ActualCurrent[1];
                nuDeviceCurrent1.Value = (decimal)Calibration.Current.DeviceMeasuredCurrent[0];
                nuDeviceCurrent2.Value = (decimal)Calibration.Current.DeviceMeasuredCurrent[1];
            }
            else
            {
                // default values
                nuSetpointCurrent1.Value = (decimal)0.1;
                nuSetpointCurrent2.Value = (decimal)1.0;
                nuRealCurrent1.Value = nuSetpointCurrent1.Value;
                nuRealCurrent2.Value = nuSetpointCurrent2.Value;
                nuDeviceCurrent1.Value = nuSetpointCurrent1.Value;
                nuDeviceCurrent2.Value = nuSetpointCurrent2.Value;
            }

            btApplyCurrent1.Enabled = true;
            btApplyCurrent2.Enabled = false;
            btReadCurrent1.Enabled = false;
            btReadCurrent2.Enabled = false;
            btRealCurrent1.Enabled = false;
            btRealCurrent2.Enabled = false;

            // voltage calibration data
            if (Calibration.Voltage != null)
            {
                // copy current values to form
                nuDeviceVoltage1.Value = (decimal)Calibration.Voltage.DeviceMeasuredVoltage[0];
                nuDeviceVoltage2.Value = (decimal)Calibration.Voltage.DeviceMeasuredVoltage[1];
                nuRealVoltage1.Value = (decimal)Calibration.Voltage.ActualVoltage[0];
                nuRealVoltage2.Value = (decimal)Calibration.Voltage.ActualVoltage[1];
            }
            else
            {
                // default values
                nuDeviceVoltage1.Value = 10;
                nuDeviceVoltage2.Value = 30;
                nuRealVoltage1.Value = nuDeviceVoltage1.Value;
                nuRealVoltage2.Value = nuDeviceVoltage2.Value;
            }
        }

        private async void btStartCurrentCalibration_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Current calibration data will be lost. Continue?", "Calibration", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    // clear current calibration
                    await activeLoadDevice.ClearCalibrationCurrentAsync();

                    // reset current to 0
                    await activeLoadDevice.SetSetpointCurrentAsync(0);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not reset calibration data (" + ex.Message.ToString() + ").", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void btStartVoltageCalibration_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Voltage calibration data will be lost. Continue?", "Calibration", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    // clear current calibration
                    await activeLoadDevice.ClearCalibrationVoltageAsync();

                    // reset current to 0
                    await activeLoadDevice.SetSetpointCurrentAsync(0);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not reset calibration data (" + ex.Message.ToString() + ").", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void btApplyCurrent_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;

            try
            {
                if ((string)button.Tag == "1")
                {
                    await activeLoadDevice.SetSetpointCurrentAsync((double)nuSetpointCurrent1.Value);

                    btReadCurrent1.Enabled = true;
                    btRealCurrent1.Enabled = true;
                }
                else
                {
                    await activeLoadDevice.SetSetpointCurrentAsync((double)nuSetpointCurrent2.Value);

                    btReadCurrent1.Enabled = false;
                    btRealCurrent1.Enabled = false;
                    btReadCurrent2.Enabled = true;
                    btRealCurrent2.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not set current (" + ex.Message.ToString() + ").", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btRealCurrent_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;

            if ((string)button.Tag == "1")
            {
                btApplyCurrent1.Enabled = false;
                btApplyCurrent2.Enabled = true;
            }
            else
            {
                btApplyCurrent2.Enabled = false;
                btCalibrateCurrent.Enabled = true;
            }
        }

        private async void btCalibrateCurrent_Click(object sender, EventArgs e)
        {
            btRealCurrent2.Enabled = false;
            btReadCurrent2.Enabled = false;

            try
            {
                double[] deviceCurrentSetpoint = new double[2];
                double[] realCurrent = new double[2];
                double[] displayedCurrent = new double[2];

                deviceCurrentSetpoint[0] = (double)nuSetpointCurrent1.Value;
                deviceCurrentSetpoint[1] = (double)nuSetpointCurrent2.Value;
                realCurrent[0] = (double)nuRealCurrent1.Value;
                realCurrent[1] = (double)nuRealCurrent2.Value;
                displayedCurrent[0] = (double)nuDeviceCurrent1.Value;
                displayedCurrent[1] = (double)nuDeviceCurrent2.Value;

                // apply new current calibration
                await activeLoadDevice.CalibrateCurrentAsync(deviceCurrentSetpoint, realCurrent, displayedCurrent);

                MessageBox.Show("Current calibrated successfully.", "Calibration done", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not calibrate current (" + ex.Message.ToString() + ").", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btReadCurrent_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;

            try
            {
                double current = await activeLoadDevice.GetActualCurrentAsync();

                if ((string)button.Tag == "1")
                {
                    nuDeviceCurrent1.Value = (decimal)current;
                }
                else
                {
                    nuDeviceCurrent2.Value = (decimal)current;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not read current (" + ex.Message.ToString() + ").", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btReadVoltage_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;

            try
            {
                double voltage = await activeLoadDevice.GetActualVoltageAsync();

                if ((string)button.Tag == "1")
                {
                    nuDeviceVoltage1.Value = (decimal)voltage;
                }
                else
                {
                    nuDeviceVoltage2.Value = (decimal)voltage;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not read voltage (" + ex.Message.ToString() + ").", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CalibrationForm_Load(object sender, EventArgs e)
        {

        }

        private async void btCalibrateVoltage_Click(object sender, EventArgs e)
        {
            try
            {
                double[] realVoltage = new double[2];
                double[] displayedVoltage = new double[2];

                realVoltage[0] = (double)nuRealVoltage1.Value;
                realVoltage[1] = (double)nuRealVoltage2.Value;
                displayedVoltage[0] = (double)nuDeviceVoltage1.Value;
                displayedVoltage[1] = (double)nuDeviceVoltage2.Value;

                // apply new voltage calibration
                await activeLoadDevice.CalibrateVoltageAsync(realVoltage, displayedVoltage);

                MessageBox.Show("Voltage calibrated successfully.", "Calibration done", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not calibrate voltage (" + ex.Message.ToString() + ").", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
