using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageQueueServer
{
    internal class PersistentTopicQueue :BaseQueue
    {
        private Mutex mutex = new Mutex();

        public PersistentTopicQueue(string queueIdentifier) : base(queueIdentifier)
        {
        }

 
        /// <summary>
        /// Notifies the Queue that the message with the Id given was sent to the process
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="process"></param>
        public override void messageSent(Guid messageId, Guid process)
        {
            mutex.WaitOne();

            var pendentReceiversForMessage = this.PendentReceivers[messageId];

            if(pendentReceiversForMessage != null)
            {
                pendentReceiversForMessage.Remove(process);
                
                if(pendentReceiversForMessage.Count() == 0)
                {
                    this.Content = this.Content.Where(m => m.MessageId != messageId).ToList();
                }

            }

            mutex.ReleaseMutex();
           
        }


        /// <summary>
        /// Get the next message to be sent to the process given. Or null if there are no messages
        /// </summary>
        /// <param name="process">Descriptor of the subscribed Process</param>
        /// <returns></returns>
        public override Message? getNextMessage(ClientProcess process)
        {
            mutex.WaitOne();
            foreach(Message msg in this.Content)
            {
                if (PendentReceivers[msg.MessageId].Contains(process.Id))
                {
                    mutex.ReleaseMutex();
                    return msg;
                }
            }

            mutex.ReleaseMutex();
            return null;
        }

    }
}
