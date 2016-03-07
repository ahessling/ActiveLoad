using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveLoadProtocol
{
    internal abstract class ASCIIRequestResponse
    {
        public void Send(string request)
        {
            SendAwaitResponse(request);
        }

        public string SendAwaitResponse(string request)
        {
            return SendAwaitResponse(request, 1000);
        }

        public virtual string SendAwaitResponse(string request, int timeout)
        {
            FlushIncoming();

            Write(request);

            return Read(timeout);
        }

        protected virtual void Write(string request)
        {
            throw new NotImplementedException();
        }

        protected virtual string Read(int timeout)
        {
            throw new NotImplementedException();
        }

        protected virtual void FlushIncoming()
        {

        }
    }
}
