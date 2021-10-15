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
            "Publish", 
            "CreateQueue", 
            "Subscribe",
            "Register"
        };

        public App()
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
                        Console.WriteLine($"Criando Fila ID: \"{message.Args[0]}\" Tipo: \"{Enum.Parse(typeof(QueueTypes), message.Args[1])}\"...");
                        this.manager.CreateQueue(message.Args[0],(QueueTypes) Enum.Parse(typeof(QueueTypes),message.Args[1]));
                        Console.WriteLine($"Fila criada com sucesso");

                        break;
                    }
                case "Publish":
                    {
                        Console.WriteLine($"Publicando mensagem...");
                        CreateMessageAndSendToManager(message);
                        Console.WriteLine($"Mensagem publicada com sucesso");

                        break;
                    }
                case "Subscribe":
                    {
                        Console.WriteLine($"Subscrevendo cliente {message.SenderId} à Fila {message.Args[0]}...");
                        this.manager.SubscribeToQueue(message.SenderId, message.Args[0]);
                        Console.WriteLine($"Subscrito com sucesso");
                        break;
                    }
                case "Register":
                    {
                        Console.WriteLine($"Registrando novo cliente com Endereço {message.Args[0]}:{message.Args[1]}");
                        return this.manager.RegisterClient(message.SenderId, message.Args[0], Int32.Parse(message.Args[1])).ToString();
                        Console.WriteLine($"Registrado com sucesso");
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
