using MessageQueueServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MessageQueueClientLib
{
    internal class TcpServer
    {
        int Port;

        public IPEndPoint endPoint { get; set; };

        TcpListener tcpListener;

        Action<string> handler;


        public TcpServer(string ip, int port, Action<string> handler)
        {
            this.Port = port;

            IPAddress localAddr = IPAddress.Parse(ip);

            this.endPoint = new IPEndPoint(localAddr, port);

            this.tcpListener = new TcpListener(localAddr,port);

            this.handler = handler;
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
                    handler(data);
                    answer = Encoding.UTF8.GetBytes($"OK::");
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
