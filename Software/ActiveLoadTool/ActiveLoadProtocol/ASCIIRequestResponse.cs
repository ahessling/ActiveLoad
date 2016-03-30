using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ActiveLoadProtocol
{
    class ASCIIRequestResponse
    {
        IASCIIReadWrite readWriteInterface;
        SemaphoreSlim semRequest;

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

            semRequest = new SemaphoreSlim(1);
        }

        public async void SendAsync(string request)
        {
            await SendAwaitResponseAsync(request);
        }

        public async Task<string> SendAwaitResponseAsync(string request)
        {
            return await SendAwaitResponseAsync(request, ResponseTimeout);
        }

        public async Task<string> SendAwaitResponseAsync(string request, int timeout)
        {
            // Clear the incoming buffer
            readWriteInterface.FlushIncoming();

            // Add end line suffix
            request += EndLineSuffix;

            // wait for end of (possible) already active request and take semaphore
            await semRequest.WaitAsync();

            // Send request
            readWriteInterface.Write(request);

            // Wait for answer
            try
            {
                string response = await readWriteInterface.ReadUntilAsync(EndLineSuffix, timeout);

                semRequest.Release();

                return response;
            }
            catch (Exception)
            {
                // release semaphore in every case
                semRequest.Release();

                throw;
            }
        }
    }
}
