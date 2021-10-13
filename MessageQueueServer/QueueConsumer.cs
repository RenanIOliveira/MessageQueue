using MessageQueueServer.ClientConnector;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace MessageQueueServer
{
    internal class QueueConsumer
    {
        private ConcurrentDictionary<Guid, IClientConnector> clientConnectors;
        private BaseQueue Queue;

        Thread thread; 
        bool stopLoop = true;
        
       
       public void Start()
       {
            //if already started do nothing
            if (stopLoop == false) return;

            stopLoop = false;
            thread.Start();
       }

        public void Stop()
        {
            stopLoop = true;
        }

       

        public QueueConsumer(BaseQueue queue, ConcurrentDictionary<Guid,IClientConnector> connectors)
        {
            this.clientConnectors = connectors;
            
            this.Queue = queue;

            thread = new Thread(Loop);

        }


        public void Loop()
        {
            while (stopLoop == false)
            {
                ConsumeMessages();

                Thread.Sleep(1000);
            }
        }

        public void ConsumeMessages()
        {
            var succeeded = new List<(ClientProcess,Message, Task<bool>)>();

            foreach(ClientProcess subscriber in this.Queue.Subscribers)
            {
                clientConnectors.TryGetValue(subscriber.Id, out var connector);

                if (connector == null) throw new Exception($"Connector with Id {subscriber.Id} not found in dictionary");

                var msg = this.Queue.getNextMessage(subscriber);

                if (msg != null)
                    succeeded.Add((subscriber, msg, connector.SendMessage(msg)));
            }


            foreach((var subscriber,var msg, var result) in succeeded)
            {
                if (result.Result == true)
                {
                    this.Queue.messageSent(msg.MessageId, subscriber.Id);
                }

            }

        }

    }
}
