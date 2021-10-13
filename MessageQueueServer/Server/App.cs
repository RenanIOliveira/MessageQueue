using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageQueueServer.Server
{
    

    internal class App : IRequestHandler
    {

        QueueManager manager;

        string[] commands = new string[] { 
            "SendMessage", 
            "CreateQueue", 
            "Subscribe",
            "Register"
        };

        App()
        {
            this.manager = new QueueManager();
        }

        public string HandleRequest(string data)
        {
            NetworkMessage message =  NetworkMessage.Parse(data);

            switch (message.Command)
            {
                case "CreateQueue":
                    {
                        this.manager.CreateQueue(message.Args[0],(QueueTypes) Enum.Parse(typeof(QueueTypes),message.Args[1]));
                        break;
                    }
                case "SendMessage":
                    {
                        CreateMessageAndSendToManager(message);
                        break;
                    }
                case "Subscribe":
                    {

                        this.manager.SubscribeToQueue(message.SenderId, message.Args[0]);
                        break;
                    }
                case "Register":
                    {
                        return this.manager.RegisterClient(message.Args[0], Int32.Parse(message.Args[1])).ToString();
                    }
                default:
                    {
                        throw new Exception("Invalid Command");
                    }
            }

            return "";
        }


        public void CreateMessageAndSendToManager(NetworkMessage netMessage)
        {
            string queueId = netMessage.Args[0];

            Message msg = new Message(netMessage.SenderId,netMessage.Args[1]);

            this.manager.PublishMessage(netMessage.Args[0],msg);
        }

    }
}
