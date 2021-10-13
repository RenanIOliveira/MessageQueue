using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MessageQueueClientLib
{
    internal class ServerModel
    {
        public IPEndPoint endPoint  {get; set;}

        public ServerModel(string ipAdress, int port)
        {
            var ip = IPAddress.Parse(ipAdress);

            this.endPoint = new IPEndPoint(ip,port);
        }
    }
}
