using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageQueueServer
{

    enum QueueTypes
    {
        Persistent,
        Ephemeral,
    }

    internal abstract class BaseQueue
    {
        public string QueueIdentifier { get; protected set; }

        public List<ClientProcess> Subscribers { get; protected set; } = new List<ClientProcess>();

        public List<Message> Content { get; protected set; } = new List<Message>();

        public Dictionary<Guid, List<Guid>> PendentReceivers { get; protected set; } = new Dictionary<Guid, List<Guid>>();

        public BaseQueue(string queueIdentifier)
        {
            this.QueueIdentifier = queueIdentifier;
        }

        public void Publish(Message message)
        {
            Content.Append(message);
            PendentReceivers[message.MessageId] = Subscribers
                .Select(s => s.Id)
                .Where(id => id != message.Sender)
                .ToList();
        }

        /// <summary>
        /// Adds a Subscriber to a Queue
        /// </summary>
        /// <param name="clientProcess"></param>
        public void Subscribe(ClientProcess clientProcess)
        {
            Subscribers.Add(clientProcess);
        }

        /// <summary>
        /// Notifies the Queue that the message with the Id given was sent to the process
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="process"></param>
        public abstract void messageSent(Guid messageId, Guid process);


        /// <summary>
        /// Get the next message to be sent to the process given. Or null if there are no messages
        /// </summary>
        /// <param name="process">Descriptor of the subscribed Process</param>
        /// <returns></returns>
        public abstract Message? getNextMessage(ClientProcess process);


    }
}
