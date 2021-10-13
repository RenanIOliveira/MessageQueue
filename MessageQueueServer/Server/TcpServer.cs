using MessageQueueServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MessageQueueServer
{
    internal class TcpServer
    {
        int Port;

        TcpListener tcpListener;

        IRequestHandler handler;


        public TcpServer(int port)
        {
            this.Port = port;

            IPAddress localAddr = IPAddress.Parse("127.0.0.1");
            this.tcpListener = new TcpListener(localAddr,port);

            this.handler = new App();
        }

        public void Start()
        {
            // Buffer for reading data
            Byte[] bytes = new Byte[4096];
            String data = "";

            while (true)
            {

                TcpClient client = this.tcpListener.AcceptTcpClient();

                data = "";

                NetworkStream stream = client.GetStream();

                int i;

                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    data += System.Text.Encoding.UTF8.GetString(bytes, 0, i);
                }


                byte[] answer;
                try
                {
                    var result = handler.HandleRequest(data);
                    answer = Encoding.UTF8.GetBytes($"OK::{result}");
                }
                catch (Exception ex)
                {
                    answer = Encoding.UTF8.GetBytes($"ERROR::");
                }


                stream.Write(answer, 0, answer.Length);


                tcpListener.Start();
            }
        }

        
    }
}
