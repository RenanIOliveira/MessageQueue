using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageQueueServer
{
    internal class EphemeralQueue : BaseQueue
    {
        private Mutex mutex = new Mutex();


        public EphemeralQueue(string queueIdentifier) : base(queueIdentifier)
        {

        }

        public override Message? getNextMessage(ClientProcess process)
        {

            mutex.WaitOne();

            foreach (Message msg in this.Content)
            {
                var pendentReceiversForMessage = PendentReceivers[msg.MessageId];
               
                if (pendentReceiversForMessage.Contains(process.Id))
                {
                    pendentReceiversForMessage.Remove(process.Id);

                    if (pendentReceiversForMessage.Count() == 0)
                    {
                        this.Content = this.Content.Where(m => m.MessageId != msg.MessageId).ToList();
                    }
                    mutex.ReleaseMutex();
                    return msg;
                }
            }
            mutex.ReleaseMutex();
            return null;
        }

        public override void messageSent(Guid messageId, Guid process)
        {
            return;
        }
    }
}
