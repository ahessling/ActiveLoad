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

        public bool IsOpen
        {
            get
            {
                return serialPort != null && serialPort.IsOpen;
            }
        }

        public bool Overtemperature
        {
            get; private set;
        }
        #endregion

        #region Constructors
        public ActiveLoadDevice()
        {
        }

        public ActiveLoadDevice(string portName)
        {
            PortName = portName;
        }

        public ActiveLoadDevice(SerialPortBuffered serialPort)
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

            return await GetIdnAsync();
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
                    return double.Parse(response.Split(' ')[0]) / 1000;
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
                    return double.Parse(response.Split(' ')[0]) / 1000;
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
                    return double.Parse(response.Split(' ')[0]) / 1000;
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
                        double temperature = double.Parse(temperatureString, NumberStyles.Any, CultureInfo.InvariantCulture);

                        // check for overtemperature condition
                        Overtemperature = temperature >= 70;

                        return temperature;
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
            string response = await scpiProtocol.RequestAsync("*IDN");

            // dissect IDN string
            SCPIIdentity identity = new SCPIIdentity(response);

            // save identity object
            Identity = identity;

            return identity;
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
            if (realVoltage.Length != 2 || displayedVoltage.Length != 2)
            {
                throw new ArgumentException("realVoltage and displayedVoltage need two points.");
            }

            string calibString = "CAL:VOLT ";
            calibString += string.Format("{0:0} {1:0} {2:0} {3:0}",
                Math.Round(realVoltage[0] * 1000),
                Math.Round(displayedVoltage[0] * 1000),
                Math.Round(realVoltage[1] * 1000),
                Math.Round(displayedVoltage[1] * 1000)
                );

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
            if (deviceSetpointCurrent.Length != 2 || realCurrent.Length != 2 || displayedCurrent.Length != 2)
            {
                throw new ArgumentException("deviceSetpointCurrent, realCurrent and displayedCurrent need two points.");
            }

            string calibString = "CAL:CURR ";
            calibString += string.Format("{0:0} {1:0} {2:0} {3:0} {4:0} {5:0}",
                Math.Round(deviceSetpointCurrent[0] * 1000),
                Math.Round(realCurrent[0] * 1000),
                Math.Round(displayedCurrent[0] * 1000),
                Math.Round(deviceSetpointCurrent[1] * 1000),
                Math.Round(realCurrent[1] * 1000),
                Math.Round(displayedCurrent[1] * 1000)
                );

            await scpiProtocol.CommandAsync(calibString, "OK");
        }

        /// <summary>
        /// Clear current calibration data.
        /// </summary>
        /// <returns></returns>
        public async Task ClearCalibrationCurrentAsync()
        {
            await scpiProtocol.CommandAsync("CAL:CLEAR CURR", "OK");
        }

        /// <summary>
        /// Clear voltage calibration data.
        /// </summary>
        /// <returns></returns>
        public async Task ClearCalibrationVoltageAsync()
        {
            await scpiProtocol.CommandAsync("CAL:CLEAR VOLT", "OK");
        }

        #endregion
    }
}