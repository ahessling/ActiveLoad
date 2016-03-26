using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public string PortName
        {
            get; private set;
        }

        public SCPIIdentity Identity
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

        public async Task Open()
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
        }

        void Open(string portName)
        {
            // Try to open specified device
            serialPort = new SerialPortBuffered(portName);
            serialPort.Open();

            scpiProtocol = new SimpleSCPIProtocol(serialPort);
        }

        public void Close()
        {
            serialPort.Close();
        }
        #endregion

        #region Functions: Get system state
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

        public async Task<double> GetTemperatureAsync()
        {
            string response = await scpiProtocol.RequestAsync("MEAS:TEMP");

            if (response.EndsWith(" C"))
            {
                try
                {
                    return double.Parse(response.Split(' ')[0]);
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
        public async Task SetSetpointCurrentAsync(double current)
        {
            if (current < 0)
            {
                throw new ArgumentOutOfRangeException("current", current, "Current must be positive.");
            }

            await scpiProtocol.CommandAsync("CURR " + (int)(current * 1000), "OK");
        }

        public async Task ResetAsync()
        {
            await scpiProtocol.CommandAsync("*RST", "OK");
        }

        public async Task BootloaderAsync()
        {
            await scpiProtocol.CommandAsync("*DFU", "");
        }

        #endregion
    }
}