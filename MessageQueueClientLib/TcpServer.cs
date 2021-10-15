
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

        public IPEndPoint endPoint { get; set; }

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
            
            this.tcpListener.Start();
            
            while (true)
            {

                TcpClient client = this.tcpListener.AcceptTcpClient();

                data = "";

                NetworkStream stream = client.GetStream();
                
                using var reader = new StreamReader(stream, Encoding.UTF8);
                using var writer = new StreamWriter(stream, Encoding.UTF8);

                string? msg = reader.ReadLine();
                if (msg != null) data = msg;

                string answer;
                try
                {
                    handler(data);
                    answer = "OK";
                }
                catch (Exception ex)
                {
                    answer = $"ERROR::{ex.Message}";
                }

                writer.WriteLine(answer);

            }
        }

        
    }
}
