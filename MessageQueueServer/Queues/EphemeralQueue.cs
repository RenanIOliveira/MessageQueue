using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageQueueServer
{
    internal class EphemeralQueue : BaseQueue
    {
        public EphemeralQueue(string queueIdentifier) : base(queueIdentifier)
        {

        }

        public override Message? getNextMessage(ClientProcess process)
        {
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

                    return msg;
                }
            }

            return null;
        }

        public override void messageSent(Guid messageId, Guid process)
        {
            return;
        }
    }
}
