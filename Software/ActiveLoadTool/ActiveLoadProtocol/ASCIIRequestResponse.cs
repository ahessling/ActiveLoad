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
        /// End every line with this string. Use this suffix also as a delimiter when receiving.
        /// </summary>
        public string EndLineSuffix
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
            ResponseTimeout = 100;

            // Clear all possibly queued characters
            readWriteInterface.FlushIncoming();

            // New line suffix as default
            EndLineSuffix = "\n";
        }

        public void Send(string request)
        {
            SendAwaitResponseAsync(request);
        }

        public Task<string> SendAwaitResponseAsync(string request)
        {
            return SendAwaitResponseAsync(request, ResponseTimeout);
        }

        public Task<string> SendAwaitResponseAsync(string request, int timeout)
        {
            // Clear the incoming buffer
            readWriteInterface.FlushIncoming();

            // Add end line suffix
            request += EndLineSuffix;

            // Send request
            readWriteInterface.Write(request);

            // Wait for answer
            return readWriteInterface.ReadUntilAsync(EndLineSuffix, timeout);
        }
    }
}
