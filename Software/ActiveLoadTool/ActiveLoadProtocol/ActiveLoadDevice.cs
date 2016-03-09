using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveLoadProtocol
{
    public class ActiveLoadDevice
    {
        SerialPortBuffered serialPort;
        SimpleSCPIProtocol scpiProtocol;

        public string PortName
        {
            get; private set;
        }

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

        string FindDevice()
        {
            throw new NotImplementedException();
        }

        public void Open()
        {
            if (PortName == "")
            {
                // Try to find device and open it
                PortName = FindDevice();

                throw new NotImplementedException();
            }

            if (PortName != "")
            {
                // Try to open specified device
                serialPort = new SerialPortBuffered(PortName);
                serialPort.Open();

                scpiProtocol = new SimpleSCPIProtocol(serialPort);
            }
        }

        public void Close()
        {
            serialPort.Close();
        }

        public async Task<string> ReadActualCurrent()
        {
            string response = await scpiProtocol.Request("MEAS:CURR");

            Console.WriteLine("Read actual current: " + response);

            return response;
        }
    }
}
