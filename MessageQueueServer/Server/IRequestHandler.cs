using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageQueueServer.Server
{
    internal interface IRequestHandler
    {
        string HandleRequest(string data);
    }
}
