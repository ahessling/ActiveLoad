using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveLoadProtocol
{
    public class ActiveLoadDevice
    {
        #region Exceptions
        [Serializable]
        public class DeviceNotFoundException : Exception
        {
            public DeviceNotFoundException() { }
            public DeviceNotFoundException(string message) : base(message) { }
            public DeviceNotFoundException(string message, Exception inner) : base(message, inner) { }
            protected DeviceNotFoundException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context) : base(info, context)
            { }
        }
        #endregion

        #region Private members
        SerialPortBuffered serialPort;
        SimpleSCPIProtocol scpiProtocol;
        #endregion

        #region Properties
        /// <summary>
        /// COM port of device.
        /// </summary>
        public string PortName
        {
            get; private set;
        }

        /// <summary>
        /// Last received SCPI identity object.
        /// </summary>
        public SCPIIdentity Identity
        {
            get; private set;
        }

        /// <summary>
        /// Device open state.
        /// </summary>
        public bool IsOpen
        {
            get
            {
                return serialPort != null && serialPort.IsOpen;
            }
        }

        /// <summary>
        /// Calibration data of the device.
        /// </summary>
        public CalibrationData Calibration
        {
            get; private set;
        }

        /// <summary>
        /// Overtemperature indicator (temperature greater than 70 °C).
        /// </summary>
        public bool Overtemperature
        {
            get; private set;
        }

        /// <summary>
        /// Indicates if device is fully calibrated.
        /// </summary>
        public bool IsCalibrated
        {
            get; private set;
        }

        /// <summary>
        /// Last seen setpoint current [A].
        /// </summary>
        public double SetpointCurrent
        {
            get; private set;
        }

        /// <summary>
        /// Last seen actual curent [A].
        /// </summary>
        public double ActualCurrent
        {
            get; private set;
        }

        /// <summary>
        /// Last seen actual voltage [V].
        /// </summary>
        public double ActualVoltage
        {
            get; private set;
        }

        /// <summary>
        /// Last seen temperature [°C].
        /// </summary>
        public double Temperature
        {
            get; private set;
        }
        #endregion

        #region Constructors
        public ActiveLoadDevice()
        {
            Calibration = new CalibrationData();
        }

        public ActiveLoadDevice(string portName) : this()
        {
            PortName = portName;
        }

        public ActiveLoadDevice(SerialPortBuffered serialPort) : this()
        {
            this.serialPort = serialPort;
        }
        #endregion

        #region Device open/close handling
        /// <summary>
        /// Find all connected Active Load devices.
        /// </summary>
        /// <returns>Array of COM ports</returns>
        public async Task<string[]> FindDevicesAsync()
        {
            string[] portNames = SerialPort.GetPortNames();
            List<string> listPortNames = new List<string>();

            foreach (string portName in portNames)
            {
                try
                {
                    Open(portName);

                    // Probe device using SCPI request *IDN?
                    SCPIIdentity identity = await GetIdnAsync();

                    if (identity.Model.Contains("Active Load"))
                    {
                        Debug.WriteLine("Found device at: " + portName);

                        listPortNames.Add(portName);
                    }

                    Close();
                }
                catch (Exception)
                {
                    // Continue with next device
                    Close();
                }
            }

            return listPortNames.ToArray();
        }

        /// <summary>
        /// Connect to a specific device (if given) or to the first found Active Load device.
        /// </summary>
        /// <returns></returns>
        public async Task<SCPIIdentity> OpenAsync()
        {
            if (PortName == null)
            {
                // Try to find first device and open it
                string[] portNames = await FindDevicesAsync();

                if (portNames.Length == 0)
                {
                    // Nothing found
                    throw new DeviceNotFoundException("No device could be found.");
                }

                PortName = portNames[0];
            }

            Open(PortName);

            // send an empty line to flush buffers
            try
            {
                await scpiProtocol.SendAwaitResponseAsync("", 100);
            }
            catch (Exception)
            {
                // ignore errors in this case, they are expected
            }

            // get SCPI identity object
            SCPIIdentity identity = await GetIdnAsync();

            // check if device is calibrated
            await GetCalibrationAsync();

            return identity;
        }

        /// <summary>
        /// Connect to a device using a known COM port.
        /// </summary>
        /// <param name="portName"></param>
        void Open(string portName)
        {
            // Try to open specified device
            serialPort = new SerialPortBuffered(portName);
            serialPort.Open();

            scpiProtocol = new SimpleSCPIProtocol(serialPort);
        }

        /// <summary>
        /// Disconnect from the device.
        /// </summary>
        public void Close()
        {
            serialPort.Close();
        }
        #endregion

        #region Functions: Get system state
        /// <summary>
        /// Get actual current that is sinked by the active load.
        /// </summary>
        /// <returns>Current [A]</returns>
        public async Task<double> GetActualCurrentAsync()
        {
            string response = await scpiProtocol.RequestAsync("MEAS:CURR");

            if (response.EndsWith(" mA"))
            {
                try
                {
                    ActualCurrent = double.Parse(response.Split(' ')[0]) / 1000;
                    return ActualCurrent;
                }
                catch (Exception e)
                {
                    throw new UnexpectedResponseException("Could not parse: " + response, e);
                }
            }
            else
            {
                throw new UnexpectedResponseException("Unexpected response: " + response);
            }
        }

        /// <summary>
        /// Get actual setpoint current.
        /// </summary>
        /// <returns>Setpoint current [A]</returns>
        public async Task<double> GetSetpointCurrentAsync()
        {
            string response = await scpiProtocol.RequestAsync("CURR");

            if (response.EndsWith(" mA"))
            {
                try
                {
                    SetpointCurrent = double.Parse(response.Split(' ')[0]) / 1000;
                    return SetpointCurrent;
                }
                catch (Exception e)
                {
                    throw new UnexpectedResponseException("Could not parse: " + response, e);
                }
            }
            else
            {
                throw new UnexpectedResponseException("Unexpected response: " + response);
            }
        }

        /// <summary>
        /// Get actual voltage of power source.
        /// </summary>
        /// <returns>Voltage [V]</returns>
        public async Task<double> GetActualVoltageAsync()
        {
            string response = await scpiProtocol.RequestAsync("MEAS:VOLT");

            if (response.EndsWith(" mV"))
            {
                try
                {
                    ActualVoltage = double.Parse(response.Split(' ')[0]) / 1000;
                    return ActualVoltage;
                }
                catch (Exception e)
                {
                    throw new UnexpectedResponseException("Could not parse: " + response, e);
                }
            }
            else
            {
                throw new UnexpectedResponseException("Unexpected response: " + response);
            }
        }

        /// <summary>
        /// Get current temperature at power transistor.
        /// </summary>
        /// <returns>Temperature [Celsius] or null if device cannot read temperature</returns>
        public async Task<double?> GetTemperatureAsync()
        {
            string response = await scpiProtocol.RequestAsync("MEAS:TEMP");

            if (response.EndsWith(" C"))
            {
                try
                {
                    string temperatureString = response.Split(' ')[0];

                    // if device cannot read temperature (it returns a ?), return null
                    if (temperatureString == "?")
                    {
                        return null;
                    }
                    else
                    {
                        Temperature = double.Parse(temperatureString, NumberStyles.Any, CultureInfo.InvariantCulture);

                        // check for overtemperature condition
                        Overtemperature = Temperature >= 70;

                        return Temperature;
                    }
                }
                catch (Exception e)
                {
                    throw new UnexpectedResponseException("Could not parse: " + response, e);
                }
            }
            else
            {
                throw new UnexpectedResponseException("Unexpected response: " + response);
            }
        }

        /// <summary>
        /// Get actual dissipated power.
        /// </summary>
        /// <returns>Dissipated power [W]</returns>
        public async Task<double> GetDissipatedPowerAsync()
        {
            string response = await scpiProtocol.RequestAsync("MEAS:POW");

            if (response.EndsWith("W"))
            {
                try
                {
                    return double.Parse(response.Split('W')[0]);
                }
                catch (Exception e)
                {
                    throw new UnexpectedResponseException("Could not parse: " + response, e);
                }
            }
            else
            {
                throw new UnexpectedResponseException("Unexpected response: " + response);
            }
        }

        /// <summary>
        /// Get device uptime since last restart.
        /// </summary>
        /// <returns>Uptime in seconds</returns>
        public async Task<uint> GetUptimeAsync()
        {
            string response = await scpiProtocol.RequestAsync("UPTI");

            if (response.EndsWith(" seconds"))
            {
                try
                {
                    return uint.Parse(response.Split(' ')[0]);
                }
                catch (Exception e)
                {
                    throw new UnexpectedResponseException("Could not parse: " + response, e);
                }
            }
            else
            {
                throw new UnexpectedResponseException("Unexpected response: " + response);
            }
        }

        /// <summary>
        /// Get SCPI identity.
        /// </summary>
        /// <returns>SCPIIdentity object</returns>
        public async Task<SCPIIdentity> GetIdnAsync()
        {
            string response = "";

            // workaround: first request does not work sometimes, so retry once in this case
            for (int retry = 0; retry < 2; retry++)
            {
                try
                {
                    response = await scpiProtocol.RequestAsync("*IDN");

                    // dissect IDN string
                    SCPIIdentity identity = new SCPIIdentity(response);

                    // save identity object
                    Identity = identity;

                    break;
                }
                catch (Exception)
                {
                    if (retry == 1)
                    {
                        throw;
                    }
                }
            }

            return Identity;
        }

        public async Task<CalibrationData> GetCalibrationAsync()
        {
            // check if "current" is calibrated
            string response = await scpiProtocol.RequestAsync("CAL:CURR");

            if (response.StartsWith("Calibrated: YES, "))
            {
                Calibration.Current = new CalibrationData.CurrentCalibration(response.Replace("Calibrated: YES, ", ""));
            }
            else if (response.StartsWith("Calibrated: NO"))
            {
            }
            else
            {
                throw new UnexpectedResponseException("Unexpected response: " + response);
            }

            // check if "voltage" is calibrated
            response = await scpiProtocol.RequestAsync("CAL:VOLT");

            if (response.StartsWith("Calibrated: YES, "))
            {
                Calibration.Voltage = new CalibrationData.VoltageCalibration(response.Replace("Calibrated: YES, ", ""));
            }
            else if (response.StartsWith("Calibrated: NO"))
            {
            }
            else
            {
                throw new UnexpectedResponseException("Unexpected response: " + response);
            }

            IsCalibrated = Calibration.IsCalibrated;

            return Calibration;
        }

        #endregion

        #region Functions: Set system state
        /// <summary>
        /// Change setpoint current.
        /// </summary>
        /// <param name="current">New current [A]</param>
        /// <returns></returns>
        public async Task SetSetpointCurrentAsync(double current)
        {
            if (current < 0)
            {
                throw new ArgumentOutOfRangeException("current", current, "Current must be positive.");
            }

            await scpiProtocol.CommandAsync("CURR " + (int)(current * 1000), "OK");
        }

        /// <summary>
        /// Reset device to a safe state.
        /// </summary>
        /// <returns></returns>
        public async Task ResetAsync()
        {
            await scpiProtocol.CommandAsync("*RST", "OK");
        }

        /// <summary>
        /// Jump to embedded bootloader (device firmare upgrade).
        /// </summary>
        /// <returns></returns>
        public async Task BootloaderAsync()
        {
            await scpiProtocol.CommandAsync("*DFU", "");
        }

        /// <summary>
        /// Calibrate measurement of actual voltage using a two point calibration.
        /// </summary>
        /// <param name="realVoltage">Two voltages [V] measured with a calibrated device</param>
        /// <param name="displayedVoltage">Displayed voltages [V] by device corresponding to the two voltages in realVoltage</param>
        /// <returns></returns>
        public async Task CalibrateVoltageAsync(double[] realVoltage, double[] displayedVoltage)
        {
            Calibration.Voltage = new CalibrationData.VoltageCalibration(realVoltage, displayedVoltage);

            string calibString = "CAL:VOLT ";
            calibString += Calibration.Voltage.ToString();

            await scpiProtocol.CommandAsync(calibString, "OK");
        }

        /// <summary>
        /// Calibrate setpoint current and measurement of actual current using a two point calibration.
        /// </summary>
        /// <param name="deviceSetpointCurrent">Two setpoint currents [A]</param>
        /// <param name="realCurrent">Two actual currents [A] measured with a calibrated device corresponding to deviceSetpointCurrent</param>
        /// <param name="displayedCurrent">Two actual currents [A] measured by the device corresponding to deviceSetpointCurrent</param>
        /// <returns></returns>
        public async Task CalibrateCurrentAsync(double[] deviceSetpointCurrent, double[] realCurrent, double[] displayedCurrent)
        {
            Calibration.Current = new CalibrationData.CurrentCalibration(deviceSetpointCurrent, realCurrent, displayedCurrent);

            string calibString = "CAL:CURR ";
            calibString += Calibration.Current.ToString();

            await scpiProtocol.CommandAsync(calibString, "OK");
        }

        /// <summary>
        /// Clear current calibration data.
        /// </summary>
        /// <returns></returns>
        public async Task ClearCalibrationCurrentAsync()
        {
            await scpiProtocol.CommandAsync("CAL:CLEAR CURR", "OK");
            Calibration.Current = null;
        }

        /// <summary>
        /// Clear voltage calibration data.
        /// </summary>
        /// <returns></returns>
        public async Task ClearCalibrationVoltageAsync()
        {
            await scpiProtocol.CommandAsync("CAL:CLEAR VOLT", "OK");
            Calibration.Voltage = null;
        }

        #endregion

        public class CalibrationData
        {
            [Serializable]
            public class CalibrationInvalidException : Exception
            {
                public CalibrationInvalidException() { }
                public CalibrationInvalidException(string message) : base(message) { }
                public CalibrationInvalidException(string message, Exception inner) : base(message, inner) { }
                protected CalibrationInvalidException(
                  System.Runtime.Serialization.SerializationInfo info,
                  System.Runtime.Serialization.StreamingContext context) : base(info, context)
                { }
            }

            public class CurrentCalibration
            {
                public double[] DeviceCurrentSetpoint;
                public double[] ActualCurrent;
                public double[] DeviceMeasuredCurrent;

                public CurrentCalibration(double[] deviceCurrentSetpoint, double[] actualCurrent, double[] deviceMeasuredCurrent)
                {
                    if (deviceCurrentSetpoint.Length != 2 || actualCurrent.Length != 2 || deviceMeasuredCurrent.Length != 2)
                    {
                        throw new ArgumentException("Parameters must be arrays with length 2.");
                    }

                    this.DeviceCurrentSetpoint = deviceCurrentSetpoint;
                    this.ActualCurrent = actualCurrent;
                    this.DeviceMeasuredCurrent = deviceMeasuredCurrent;
                }

                public CurrentCalibration(string calibTuples)
                {
                    DeviceCurrentSetpoint = new double[2];
                    ActualCurrent = new double[2];
                    DeviceMeasuredCurrent = new double[2];

                    string[] tuples = calibTuples.Split(' ');

                    if (tuples.Length != 6)
                    {
                        throw new CalibrationInvalidException("calibTuples must contain exactly 6 values separated with a space character.");
                    }

                    try
                    {
                        DeviceCurrentSetpoint[0] = double.Parse(tuples[0]) / 1000.0;
                        ActualCurrent[0] = double.Parse(tuples[1]) / 1000.0;
                        DeviceMeasuredCurrent[0] = double.Parse(tuples[2]) / 1000.0;

                        DeviceCurrentSetpoint[1] = double.Parse(tuples[3]) / 1000.0;
                        ActualCurrent[1] = double.Parse(tuples[4]) / 1000.0;
                        DeviceMeasuredCurrent[1] = double.Parse(tuples[5]) / 1000.0;
                    }
                    catch (Exception)
                    {
                        throw new CalibrationInvalidException("One or more parameters is not a valid number (" + calibTuples + ").");
                    }
                }

                public override string ToString()
                {
                    string calibTuples = string.Format("{0:0} {1:0} {2:0} {3:0} {4:0} {5:0}",
                        Math.Round(DeviceCurrentSetpoint[0] * 1000),
                        Math.Round(ActualCurrent[0] * 1000),
                        Math.Round(DeviceMeasuredCurrent[0] * 1000),
                        Math.Round(DeviceCurrentSetpoint[1] * 1000),
                        Math.Round(ActualCurrent[1] * 1000),
                        Math.Round(DeviceMeasuredCurrent[1] * 1000)
                        );

                    return calibTuples;
                }
            }

            public class VoltageCalibration
            {
                public double[] ActualVoltage;
                public double[] DeviceMeasuredVoltage;

                public VoltageCalibration(double[] actualVoltage, double[] deviceMeasuredVoltage)
                {
                    if (actualVoltage.Length != 2 || deviceMeasuredVoltage.Length != 2)
                    {
                        throw new ArgumentException("Parameters must be arrays with length 2.");
                    }

                    this.ActualVoltage = actualVoltage;
                    this.DeviceMeasuredVoltage = deviceMeasuredVoltage;
                }

                public VoltageCalibration(string calibTuples)
                {
                    ActualVoltage = new double[2];
                    DeviceMeasuredVoltage = new double[2];

                    string[] tuples = calibTuples.Split(' ');

                    if (tuples.Length != 4)
                    {
                        throw new CalibrationInvalidException("calibTuples must contain exactly 4 values separated with a space character.");
                    }

                    try
                    {
                        ActualVoltage[0] = double.Parse(tuples[0]) / 1000.0;
                        DeviceMeasuredVoltage[0] = double.Parse(tuples[1]) / 1000.0;

                        ActualVoltage[1] = double.Parse(tuples[2]) / 1000.0;
                        DeviceMeasuredVoltage[1] = double.Parse(tuples[3]) / 1000.0;
                    }
                    catch (Exception)
                    {
                        throw new CalibrationInvalidException("One or more parameters is not a valid number (" + calibTuples + ").");
                    }
                }

                public override string ToString()
                {
                    string calibTuples = string.Format("{0:0} {1:0} {2:0} {3:0}",
                        Math.Round(ActualVoltage[0] * 1000),
                        Math.Round(DeviceMeasuredVoltage[0] * 1000),
                        Math.Round(ActualVoltage[1] * 1000),
                        Math.Round(DeviceMeasuredVoltage[1] * 1000)
                        );

                    return calibTuples;
                }
            }

            /// <summary>
            /// Current calibration data.
            /// </summary>
            public CurrentCalibration Current
            {
                get; internal set;
            }

            /// <summary>
            /// Voltage calibration data.
            /// </summary>
            public VoltageCalibration Voltage
            {
                get; internal set;
            }

            /// <summary>
            /// Indicates if the device is calibrated.
            /// </summary>
            public bool IsCalibrated
            {
                get
                {
                    return Current != null && Voltage != null;
                }
            }
        }
    }
}