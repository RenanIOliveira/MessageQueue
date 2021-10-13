﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MessageQueueServer.ClientConnector
{
    internal class TCPClientConnector : IClientConnector
    {

        private int ReadTimeout = 2000;

        public ClientProcess ClientDescriptor { get ; init ; }
        private TcpClient tcpSocket {  get; set; }

        public TCPClientConnector(ClientProcess client)
        {
            this.ClientDescriptor = client;
            this.tcpSocket = new TcpClient();
        }

        public void Dispose()
        {
            if(tcpSocket != null) tcpSocket.Dispose();
        }

        /// <summary>
        /// If we send an empty string the client should respond with OK
        /// </summary>
        /// <returns></returns>
        public async Task<bool> IsAlive()
        {
            try
            {
                tcpSocket.Connect(this.ClientDescriptor.ConnectionEndPoint);

                using NetworkStream networkStream = tcpSocket.GetStream();

                networkStream.ReadTimeout = this.ReadTimeout;

                using var writer = new StreamWriter(networkStream);
                using var reader = new StreamReader(networkStream, Encoding.UTF8);

                await writer.WriteAsync("");

                string? response = reader.ReadToEnd();

                if (response != null && response == "OK")
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }
            finally
            {
               tcpSocket.Close();
            }
        }

        public async Task<bool> SendMessage(Message msg)
        {

            try
            {
               tcpSocket.Connect(this.ClientDescriptor.ConnectionEndPoint);

                using NetworkStream networkStream = tcpSocket.GetStream();

                networkStream.ReadTimeout = this.ReadTimeout;

               //using var writer = new StreamWriter(networkStream);
               using var reader = new StreamReader(networkStream, Encoding.UTF8);

                byte[] bytes = msg.Content;
                await networkStream.WriteAsync(bytes, 0, bytes.Length);

                string? response = reader.ReadToEnd();

                if(response != null && response == "OK")
                {
                    return true;
                }

                return false;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }
            finally
            {
                tcpSocket.Close();
            }
            
        }

       
    }
}
