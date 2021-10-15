using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using MessageQueueServer.ClientConnector;

namespace MessageQueueServer
{
    internal class QueueManager
    {

        Dictionary<string, BaseQueue> Queues = new();
        Dictionary<string, QueueConsumer> QueueConsumers = new ();

        Dictionary<Guid, ClientProcess> Clients = new ();
        ConcurrentDictionary<Guid, IClientConnector> connectors= new();


        int QueuesCount => Queues.Count();

        public QueueManager()
        {
            
        }

        public void CreateQueue(string queueId, QueueTypes type)
        {
            if (Queues.ContainsKey(queueId))
            {
                throw new Exception($"Queue with Id {queueId} already exists");
            }
            else
            {
                //create Queue
                BaseQueue queue;
                switch (type)
                {
                    case QueueTypes.Persistent:
                        {
                            queue = new PersistentTopicQueue(queueId);
                            break;
                        }
                    case QueueTypes.Ephemeral:
                        {
                            queue = new EphemeralQueue(queueId);
                            break;
                        }

                    default:
                        throw new Exception($"Invalid Queue Type: {type}");
                }
            
            
                Queues.Add(queueId, queue);

                //Create consumer Thread for queue
                var consumer = new QueueConsumer(queue, this.connectors);
                QueueConsumers.Add(queueId, consumer);

                //Start Consumer Thread
                consumer.Start();


            }
        }

        public void SubscribeToQueue(Guid clientId, string QueueId)
        {

            this.Clients.TryGetValue(clientId, out var clientDescriptor);

            if (clientDescriptor == null) throw new Exception($"Client with Id {clientId} not registered");


            Queues.TryGetValue(QueueId, out var queue);

            if (queue == null) throw new Exception($"Queue with Id {QueueId} doesn't exist");


            queue.Subscribe(clientDescriptor);
        }


        public void PublishMessage(string queueId, Message msg)
        {
            Queues.TryGetValue(queueId, out var q);

            if (q == null) throw new Exception($"Queue with Id {queueId} doesn't exist");

            q.Publish(msg);
        }

        public List<ClientProcess> getSubscribers(string queueId)
        {
            Queues.TryGetValue(queueId, out var q);

            if (q == null) throw new Exception($"Queue with Id {queueId} doesn't exist");

            return q.Subscribers;
        }


        /// <summary>
        /// Receives Ip and Port in wich the client will listen for new messages
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="port"></param>
        public Guid RegisterClient(Guid clientId, string ip, int port)
        {
            ClientProcess clientDescriptor;
            
            if (clientId == Guid.Empty)
                clientDescriptor = new ClientProcess(new IPEndPoint(IPAddress.Parse(ip), port), new Guid());
            else
                clientDescriptor = new ClientProcess(new IPEndPoint(IPAddress.Parse(ip), port), clientId);

            
            if (Clients.ContainsKey(clientDescriptor.Id))
                this.Clients.Remove(clientDescriptor.Id);

            var connector = new TCPClientConnector(clientDescriptor);


            //check if a connector for this client already exists, if it doesn't create

            if (connectors.ContainsKey(clientDescriptor.Id))
            {
                connectors.TryRemove(clientDescriptor.Id, out _);
            }
            
            connectors.TryAdd(clientDescriptor.Id, connector);

            this.Clients.Add(clientDescriptor.Id, clientDescriptor);

            return clientDescriptor.Id;
        }




    }
}
