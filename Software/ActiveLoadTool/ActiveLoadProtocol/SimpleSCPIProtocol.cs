using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveLoadProtocol
{
    #region Exceptions
    [Serializable]
    public class UnexpectedResponseException : Exception
    {
        public UnexpectedResponseException() { }
        public UnexpectedResponseException(string message) : base(message) { }
        public UnexpectedResponseException(string message, Exception inner) : base(message, inner) { }
        protected UnexpectedResponseException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context)
        { }
    }
    #endregion

    class SimpleSCPIProtocol : ASCIIRequestResponse
    {
        public SimpleSCPIProtocol(IASCIIReadWrite readWriteInterface) : base(readWriteInterface)
        {
            // End with new line
            EndLineSuffix = "\n";
        }

        public async Task<string> CommandAsync(string command)
        {
            return await SendAwaitResponseAsync(command);
        }

        public async Task CommandAsync(string command, string demandResponse)
        {
            string response = await SendAwaitResponseAsync(command);

            if (response != demandResponse)
                throw new UnexpectedResponseException("Unexpected response: " + response + " (expected: " + demandResponse + ").");
        }

        public async Task<string> RequestAsync(string request)
        {
            return await SendAwaitResponseAsync(request + "?");
        }
    }
}
