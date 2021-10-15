using MessageQueueClientLib.ServerConnector;
using System.Net;

namespace MessageQueueClientLib
{
    public class MQClient
    {
        ServerModel Server;
        IServerConnector Connector;
        public Guid ClientId {get; private set;}
        TcpServer Listener;

        enum QueueTypes
        {
            Persistent,
            Ephemeral,
        }

        
        public MQClient(Action<string> handler,string clientIp, string  server, int clientPort, int serverPort = 13000)
        {
            this.Server = new ServerModel(server, serverPort);
            this.Connector = new TCPServerConnector(Server);

            this.Listener = new TcpServer(clientIp, clientPort, handler);
            Task.Run(() => this.Listener.Start());

            var registeredId = Register();

            if (registeredId == null || registeredId == "")
            {
                throw new Exception($"Error: It was not possible to register in Message Queue Server");
            }
            else
            {
                this.ClientId = Guid.Parse(registeredId);
            }

        }

        /// <summary>
        /// Subscribes to the defined queue Throws an exception if the queue doesn't exist
        /// </summary>
        /// <param name="queueIdentifier"></param>
        public bool Subscribe(string queueIdentifier)
        {
            var args = new List<string>();
            args.Add(queueIdentifier);

            NetworkMessage msg = new NetworkMessage("Subscribe",ClientId,args);

            Task<(bool,string)> taskResult = Connector.SendMessage(msg);

            var result = taskResult.Result;

            return result.Item1;
        }

        /// <summary>
        /// Publishes a message in the given queue
        /// </summary>
        /// <param name="queueIdentifier"></param>
        /// <param name="message"></param>
        public bool Publish(string queueIdentifier, string message)
        {
            var args = new List<string>();
            args.Add(queueIdentifier);
            args.Add(message);

            NetworkMessage msg = new NetworkMessage("Publish", ClientId, args);

            Task<(bool, string)> taskResult = Connector.SendMessage(msg);

            var result = taskResult.Result;

            return result.Item1;
        }


        public bool CreateQueue(string queueIdentifier,string queueType = "Persistent")
        {
            var args = new List<string>();
            args.Add(queueIdentifier);
            args.Add(QueueTypeFromString(queueType).ToString());

            NetworkMessage msg = new NetworkMessage("CreateQueue", ClientId, args);

            Task<(bool, string)> taskResult = Connector.SendMessage(msg);

            var result = taskResult.Result;

            return result.Item1;
        }


        public string? Register()
        {
            var args = new List<string>();

            args.Add(this.Listener.endPoint.Address.ToString());
            args.Add(this.Listener.endPoint.Port.ToString());


            NetworkMessage msg = new NetworkMessage("Register", ClientId, args);

            Task<(bool, string)> taskResult = Connector.SendMessage(msg);

            var result = taskResult.Result;

            if(result.Item1 == true)
            {
                return result.Item2;
            }
            else{
                return null;
            }
            

        }


        private int QueueTypeFromString(string type)
        {
            switch (type)
            {
                case "Persistent":
                        return (int)QueueTypes.Persistent;
                case "Ephemeral":
                        return (int)QueueTypes.Ephemeral;
                default: 
                    throw new Exception($"Invalid Queue Type: {type}");
            }
        }


    }
}