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
            // End every line with CR+LF
            EndWithNewLine = true;
        }

        public void Command(string command)
        {
            string response = SendAwaitResponse(command);

            // todo: check response
        }

        public string Request(string request)
        {
            string response = SendAwaitResponse(request + "?");

            return response;
        }
    }
}
