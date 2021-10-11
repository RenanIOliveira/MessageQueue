using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageQueueServer
{
    internal class PersistentTopicQueue
    {
        public string QueueIdentifier { get; private set; }

        public List<ClientProcess> Subscribers { get; private set; } = new List<ClientProcess>();

        public List<Message> Content { get; private set; } = new List<Message>();

        public Dictionary<Guid, List<Guid>> PendentReceivers { get; private set; } = new Dictionary<Guid, List<Guid>>();

        public PersistentTopicQueue(string queueIdentifier)
        {
            this.QueueIdentifier = queueIdentifier;
        }

        public void Publish(Message message)
        {
            Content.Append(message);
            PendentReceivers[message.MessageId] = Subscribers.Select(s => s.Id).ToList();
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
        public void messageSent(Guid messageId, Guid process)
        {
            var pendentReceiversForMessage = this.PendentReceivers[messageId];

            if(pendentReceiversForMessage != null)
            {
                pendentReceiversForMessage.Remove(process);
                
                if(pendentReceiversForMessage.Count() == 0)
                {
                    this.Content = this.Content.Where(m => m.MessageId != messageId).ToList();
                }

            }
           
        }


        /// <summary>
        /// Get the next message to be sent to the process given. Or null if there are no messages
        /// </summary>
        /// <param name="process">Descriptor of the subscribed Process</param>
        /// <returns></returns>
        public Message? getNextMessage(ClientProcess process)
        {
            foreach(Message msg in this.Content)
            {
                if (PendentReceivers[msg.MessageId].Contains(process.Id))
                {
                    return msg;
                }
            }

            return null;
        }

    }
}
