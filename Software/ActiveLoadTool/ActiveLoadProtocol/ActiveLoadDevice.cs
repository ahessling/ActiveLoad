using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveLoadProtocol
{
    class ActiveLoadDevice
    {
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

        string FindDevice()
        {
            throw new NotImplementedException();
        }

        public void Open()
        {
            if (PortName != "")
            {
                // Try to open specified device
            }
            else
            {
                // Try to find device and open it
                PortName = FindDevice();
            }
        }

        public void Close()
        {

        }
    }
}
