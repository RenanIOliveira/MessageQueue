using System;
using System.Collections.Generic;
using System.Linq;
using MessageQueueClientLib;

namespace MQClientExample 
{
    public class Program
    {

        

        public static void Main(string[] args)
        {
            var nameMapper = new NameMapper();

            Console.WriteLine("Digite o ip");
            var Ip = Console.ReadLine();
            Console.WriteLine("Digite a porta");
            var Port = Console.ReadLine();

            Console.WriteLine("Digite o ip do servidor");
            var ServerIp = Console.ReadLine();

            Console.WriteLine("Digite um nome para esse processo");
            var name = Console.ReadLine();

            var clientId = nameMapper.getIdFromName(name);

            if (Port == null) Port = "13000";
            if (Ip == null) Ip = "127.0.0.1";
            if (ServerIp == null) ServerIp = "127.0.0.1";



            Console.WriteLine("Iniciando Cliente...");
            var client = new MQClient((value) => Console.WriteLine($"Mensagem Recebida: {value}"),Ip,ServerIp,Int32.Parse(Port),clientId);

            if(name != null)
                nameMapper.AddName(name, client.ClientId.ToString());

            Console.WriteLine("Registrado Com Sucesso");
            
            var command = "";
            
            while(command != "Exit")
            {
                command = AskForCommand();
                switch (command) {
                    case "Publish":
                        PublishCommand(client);
                        break;
                    case "CreateQueue":
                        CreateQueueCommand(client);
                        break;
                    case "Subscribe":
                        SubscribeCommand(client);
                        break;
                    case "Exit":
                        break;

                }

            }
            Console.ReadLine();
        }

        public static string? AskForCommand()
        {
            string[] commands = new string[] { "Publish", "CreateQueue", "Subscribe", "Exit" };

            var  validCommand = false;
            
            Console.WriteLine("Digite um Comando [Publish, CreateQueue, Subscribe, Exit]");
          
            var command = Console.ReadLine();

            if(commands.Contains(command)) validCommand = true;

            while (validCommand == false)
            {
                Console.WriteLine("Comando Inválido");

                Console.WriteLine("Digite um Comando [Publish, CreateQueue, Subscribe]");
                command = Console.ReadLine();

                if (commands.Contains(command)) validCommand = true;
            }

            return command;

        }


        static void PublishCommand(MQClient client)
        {
            Console.WriteLine("Digite o Id da Fila:");
            var Id = Console.ReadLine();

            Console.WriteLine("Digite a mensagem:");
            var msg = Console.ReadLine();

            bool succeded = false;
            if (Id!= null && msg != null)
                succeded = client.Publish(Id, msg);

            if (succeded)
            {
                Console.WriteLine("Mensagem Publicada Com Sucesso");
            }
            else
            {
                Console.WriteLine("Erro Ao Publicar Mensagem");
            }
        }


        static void CreateQueueCommand(MQClient client)
        {
            
            
            Console.WriteLine("Digite o tipo de  Fila: [Persistent, Ephemeral]");
            var type = Console.ReadLine();

            Console.WriteLine("Digite o Id da Fila:");
            var Id = Console.ReadLine();

            if(Id!= null && type != null)
            {
               var success = client.CreateQueue(Id, type);
                if(success == true)
                {
                    Console.WriteLine("Fila Criada Com Sucesso");
                }
                else
                {
                    Console.WriteLine("Erro ao criar Fila");
                }
            }


        }

        static void SubscribeCommand(MQClient client)
        {
            Console.WriteLine("Digite o Id da Fila:");
            var Id = Console.ReadLine();

            if(Id != null)
            {
                var success = client.Subscribe(Id);

                if (success == true)
                {
                    Console.WriteLine("Subscrito Com Sucesso");
                }
                else
                {
                    Console.WriteLine("Erro ao Subscrever");
                }
            }
        }
    }
}