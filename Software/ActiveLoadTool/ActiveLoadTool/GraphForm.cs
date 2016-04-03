using ActiveLoadProtocol;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace ActiveLoadTool
{
    public partial class GraphForm : Form
    {
        public class ComboBoxItem<T>
        {
            public string Name { get; set; }
            public T Value { get; set; }

            public ComboBoxItem(T value)
            {
                Value = value;
                Name = value.ToString();
            }
        };

        public ActiveLoadDevice ActiveLoadDevice;
        int maxSamples = 10;

        private static GraphForm inst;
        public static GraphForm GetForm(ActiveLoadDevice activeLoadDevice)
        {
            if (inst == null || inst.IsDisposed)
            {
                inst = new GraphForm(activeLoadDevice);
            }

            // set (new) active load device instance
            inst.ActiveLoadDevice = activeLoadDevice;

            return inst;
        }

        private GraphForm(ActiveLoadDevice activeLoadDevice)
        {
            InitializeComponent();

            this.ActiveLoadDevice = activeLoadDevice;

            // fill interval combo box
            cbInterval.DisplayMember = "Name";
            cbInterval.ValueMember = "Value";

            List<ComboBoxItem<int>> listIntervals = new List<ComboBoxItem<int>>();
            listIntervals.Add(new ComboBoxItem<int>(100));
            listIntervals.Add(new ComboBoxItem<int>(250));
            listIntervals.Add(new ComboBoxItem<int>(500));
            listIntervals.Add(new ComboBoxItem<int>(1000));
            listIntervals.Add(new ComboBoxItem<int>(5000));
            listIntervals.Add(new ComboBoxItem<int>(10000));

            foreach (var item in listIntervals)
            {
                item.Name = item.Value + " ms";
            }

            cbInterval.DataSource = listIntervals;

            // fill samples combo box
            cbSamples.DisplayMember = "Name";
            cbSamples.ValueMember = "Value";

            List<ComboBoxItem<int>> listSamples = new List<ComboBoxItem<int>>();
            listSamples.Add(new ComboBoxItem<int>(10));
            listSamples.Add(new ComboBoxItem<int>(25));
            listSamples.Add(new ComboBoxItem<int>(50));
            listSamples.Add(new ComboBoxItem<int>(500));
            listSamples.Add(new ComboBoxItem<int>(1000));

            foreach (var item in listSamples)
            {
                item.Name = item.Value + " samples";
            }

            cbSamples.DataSource = listSamples;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int sampleIndex = 0;
            int time = 0;

            foreach (var series in chartActiveLoad.Series)
            {
                sampleIndex = 0;

                if (series.Points.Count > maxSamples)
                {
                    // remove oldest point
                    series.Points.RemoveAt(0);
                }

                // "patch" every X value so that the X axis labels never change and match the actual time they represent
                foreach (var point in series.Points)
                {
                    time = (-series.Points.Count + sampleIndex++) * tmrRefresh.Interval;
                    point.XValue = time;
                }
            }

            // the newly drawn point is at position t = 0
            time = 0;

            chartActiveLoad.Series["Actual current"].Points.AddXY(time, ActiveLoadDevice.ActualCurrent);
            chartActiveLoad.Series["Setpoint current"].Points.AddXY(time, ActiveLoadDevice.SetpointCurrent);

            chartActiveLoad.Series["Actual voltage"].Points.AddXY(time, ActiveLoadDevice.ActualVoltage);
            chartActiveLoad.Series["Dissipated power"].Points.AddXY(time, ActiveLoadDevice.ActualCurrent * ActiveLoadDevice.ActualVoltage);
            chartActiveLoad.Series["Temperature"].Points.AddXY(time, ActiveLoadDevice.Temperature);

            // force recalculation of bounds
            chartActiveLoad.ResetAutoValues();
        }

        private void chkPowerVisible_CheckedChanged(object sender, EventArgs e)
        {
            chartActiveLoad.Series["Dissipated power"].Enabled = chkPowerVisible.Checked;
            chartActiveLoad.Series["Temperature"].Enabled = chkTemperatureVisible.Checked;

            chartActiveLoad.ChartAreas["ChartAreaPowerTemperature"].Visible = chkPowerVisible.Checked || chkTemperatureVisible.Checked;
        }

        private void chkVoltageVisible_CheckedChanged(object sender, EventArgs e)
        {
            chartActiveLoad.ChartAreas["ChartAreaVoltage"].Visible = chkVoltageVisible.Checked;
        }

        private void chkRun_CheckedChanged(object sender, EventArgs e)
        {
            tmrRefresh.Enabled = chkRun.Checked;
        }

        private void cbInterval_SelectedIndexChanged(object sender, EventArgs e)
        {
            tmrRefresh.Interval = (int)cbInterval.SelectedValue;

            // clear all samples
            foreach (var series in chartActiveLoad.Series)
            {
                series.Points.Clear();
            }

            setChartBounds();
        }

        private void cbInterval_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                try
                {
                    // get only the number
                    int interval = int.Parse(Regex.Match(cbInterval.Text, @"\d+").Value);

                    if (interval < 100)
                    {
                        interval = 100;
                    }

                    tmrRefresh.Interval = interval;
                    cbInterval.Text = interval + " ms";

                    // clear all samples
                    foreach (var series in chartActiveLoad.Series)
                    {
                        series.Points.Clear();
                    }

                    setChartBounds();
                }
                catch (Exception)
                {
                }

                e.Handled = true;
            }
        }

        private void cbSamples_SelectedIndexChanged(object sender, EventArgs e)
        {
            maxSamples = (int)cbSamples.SelectedValue;

            // clear all samples
            foreach (var series in chartActiveLoad.Series)
            {
                series.Points.Clear();
            }

            setChartBounds();
        }

        private void cbSamples_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                try
                {
                    // get only the number
                    maxSamples = int.Parse(Regex.Match(cbSamples.Text, @"\d+").Value);

                    if (maxSamples < 1)
                    {
                        maxSamples = 1;
                    }

                    cbSamples.Text = maxSamples + " samples";

                    // clear all samples
                    foreach (var series in chartActiveLoad.Series)
                    {
                        series.Points.Clear();
                    }

                    setChartBounds();
                }
                catch (Exception)
                {
                }

                e.Handled = true;
            }
        }

        private void setChartBounds()
        {
            foreach (var chartArea in chartActiveLoad.ChartAreas)
            {
                chartArea.AxisX.Minimum = -maxSamples * tmrRefresh.Interval;
                chartArea.AxisX.Maximum = 0;
            }
        }
    }
}
