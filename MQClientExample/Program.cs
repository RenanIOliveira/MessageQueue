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
            Console.WriteLine("Iniciando Cliente...");

            var client = new MQClient((value) => Console.WriteLine(value),"127.0.0.1");

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

        }
    }
}