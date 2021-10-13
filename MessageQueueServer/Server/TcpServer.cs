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

            tcpListener.Start();

            this.handler = new App();

            Console.WriteLine("Servidor Iniciado Com Sucesso");
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


                using var reader = new StreamReader(stream, Encoding.UTF8);

                //int i;
                //while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                //{
                //    data += System.Text.Encoding.UTF8.GetString(bytes, 0, i);
                //}

                

                string? response = reader.ReadLine();
                if (response != null) data = response;

                byte[] answer;
                try
                {
                    Console.WriteLine($"Message Received: {data}");
                    var result = handler.HandleRequest(data);
                    answer = Encoding.UTF8.GetBytes($"OK::{result}");
                }
                catch (Exception ex)
                {
                    answer = Encoding.UTF8.GetBytes($"ERROR::{ex.Message}");
                }


                stream.Write(answer, 0, answer.Length);
                client.Close();
            }
        }

        
    }
}
