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

    public class SCPIIdentity
    {
        /// <summary>
        /// Manufacturer name of device
        /// </summary>
        public string Manufacturer
        {
            get; private set;
        }

        /// <summary>
        /// Model name or type of device
        /// </summary>
        public string Model
        {
            get; private set;
        }

        /// <summary>
        /// Serial number or string of device
        /// </summary>
        public string SerialNumber
        {
            get; private set;
        }

        /// <summary>
        /// Version string of device
        /// </summary>
        public string Version
        {
            get; private set;
        }

        /// <summary>
        /// Try to parse identity string and fill internal data structures.
        /// </summary>
        /// <param name="idnString">SCPI identity string</param>
        public SCPIIdentity(string idnString)
        {
            try
            {
                string[] idnElements = idnString.Split(',');

                Manufacturer = idnElements[0];
                Model = idnElements[1];
                SerialNumber = idnElements[2];
                Version = idnElements[3];
            }
            catch (Exception)
            {
                throw new UnexpectedResponseException("Unexpected identity string: " + idnString + ".");
            }
        }
    }

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
