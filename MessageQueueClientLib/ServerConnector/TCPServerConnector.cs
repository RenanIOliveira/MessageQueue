using System.Net.Sockets;
using System.Text;

namespace MessageQueueClientLib.ServerConnector
{
    internal class TCPServerConnector : IServerConnector
    {

        private int ReadTimeout = 2000;

        public ServerModel server { get ; init ; }
        private TcpClient tcpSocket {  get; set; }

        public TCPServerConnector(ServerModel server)
        {
            this.server = server;
            this.tcpSocket = new TcpClient();
        }

        public void Dispose()
        {
            if(tcpSocket != null) tcpSocket.Dispose();
        }

      
        public async Task<(bool,string)> SendMessage(NetworkMessage msg)
        {

            try
            {
               tcpSocket.Connect(this.server.endPoint);

                using NetworkStream networkStream = tcpSocket.GetStream();

                networkStream.ReadTimeout = this.ReadTimeout;

               //using var writer = new StreamWriter(networkStream);
               using var reader = new StreamReader(networkStream, Encoding.UTF8);

                byte[] bytes = Encoding.UTF8.GetBytes(msg.AsString());
                await networkStream.WriteAsync(bytes, 0, bytes.Length);

                string? response = reader.ReadToEnd();
                if(response != null && response.Count() >= 2 && response.Substring(0, 2) == "OK" )
                {
                    var result = response.Split("::")[1];
                    return (true, result);
                }

                return (false, "");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);

                return (false, "");
            }
            finally
            {
                tcpSocket.Close();
            }
            
        }

       
    }
}
