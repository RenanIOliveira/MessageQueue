using System;
using System.Collections.Generic;
using System.Linq;

namespace MessageQueueServer // Note: actual namespace depends on the project name.
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var server = new TcpServer(13000);

            server.Start();
        }
    }
}