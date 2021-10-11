using System.Net;

namespace MessageQueueServer
{

    /// <summary>
    /// A subscriber is a process that is subscribed to one or more queues
    /// </summary>
    internal class ClientProcess
    {
        public IPAddress ipAdress { get; private set; }

        public Guid Id { get; private set; }

        public ClientProcess(IPAddress ipAdress)
        {
            this.ipAdress = ipAdress;
            Id = Guid.NewGuid();
        }
    }
}