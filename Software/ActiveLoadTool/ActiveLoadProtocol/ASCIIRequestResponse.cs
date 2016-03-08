using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveLoadProtocol
{
    class ASCIIRequestResponse
    {
        IASCIIReadWrite readWriteInterface;

        /// <summary>
        /// If true, end every request with CR and LF characters.
        /// </summary>
        public bool EndWithNewLine
        {
            get; set;
        }

        /// <summary>
        /// Default response timeout in ms.
        /// </summary>
        public int ResponseTimeout
        {
            get; set;
        }

        public ASCIIRequestResponse(IASCIIReadWrite readWriteInterface)
        {
            this.readWriteInterface = readWriteInterface;

            // Default response timeout
            ResponseTimeout = 1000;

            // Clear all possibly queued characters
            readWriteInterface.FlushIncoming();
        }

        public void Send(string request)
        {
            SendAwaitResponse(request);
        }

        public string SendAwaitResponse(string request)
        {
            return SendAwaitResponse(request, ResponseTimeout);
        }

        public virtual string SendAwaitResponse(string request, int timeout)
        {
            readWriteInterface.FlushIncoming();

            if (EndWithNewLine)
            {
                request += "\n";
            }

            readWriteInterface.Write(request);

            return readWriteInterface.Read(timeout);
        }
    }
}
