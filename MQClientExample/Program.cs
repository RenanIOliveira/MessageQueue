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
            Console.WriteLine("Iniciando Cliente");

            var client = new MQClient((value) => Console.WriteLine(value),"127.0.0.1");


            Console.WriteLine("Registrado Com Sucesso");
            Console.ReadLine();

            
        }
    }
}