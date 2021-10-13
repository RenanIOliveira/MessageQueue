using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageQueueClientLib
{
    internal class NetworkMessage
    {
        public string Command { get; set; }

        public Guid SenderId { get; set; }

        public List<string> Args {  get; set; }

        public NetworkMessage(string message)
        {
            var msg = Parse(message);

            this.Command = msg.Command;
            this.Args = msg.Args;
        }

        public NetworkMessage(string command,Guid SenderId, List<string> args)
        {
            this.Command = command; 
            this.Args = args;
            this.SenderId = SenderId;
        }


        public static NetworkMessage Parse(string data)
        {
            var splitted = data.Split("::");

            if (splitted.Length < 2) throw new Exception($"invalid Message Format");

            var command = splitted[0];
            var sender = Guid.Parse(splitted[1]);

            if(splitted.Length == 1)
            {
                return new NetworkMessage(command,sender, new List<string>());
            }
            else
            {
                List<string> args = new List<string>();
                 
                for(int i = 1; i< splitted.Length; i++)
                {
                    args.Add(splitted[i]);
                }

                return new NetworkMessage(command,sender, args);
            }
        }

        public string AsString()
        {
            string result = $"{this.Command}::";
            
            if (Args.Count == 0)
            {
                return result;
            }
            else
            {
                return result + String.Join("::", this.Args);
            }
        }
    }
}
