using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveLoadProtocol
{
    interface IASCIIReadWrite
    {
        void Write(string request);

        string Read(int timeout);

        void FlushIncoming();
    }
}
