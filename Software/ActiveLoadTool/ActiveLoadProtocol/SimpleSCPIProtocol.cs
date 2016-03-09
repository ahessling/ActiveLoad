using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveLoadProtocol
{
    class SimpleSCPIProtocol : ASCIIRequestResponse
    {
        public SimpleSCPIProtocol(IASCIIReadWrite readWriteInterface) : base(readWriteInterface)
        {
            // End with new line
            EndLineSuffix = "\n";
        }

        public async void Command(string command)
        {
            string response = await SendAwaitResponseAsync(command);

            // todo: check response
        }

        public async Task<string> Request(string request)
        {
            return await SendAwaitResponseAsync(request + "?");
        }
    }
}
