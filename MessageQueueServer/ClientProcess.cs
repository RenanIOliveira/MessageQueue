using System.Net;

namespace MessageQueueServer
{

    /// <summary>
    /// A subscriber is a process that is subscribed to one or more queues
    /// </summary>
    internal class ClientProcess
    {
        public IPEndPoint ConnectionEndPoint { get; private set; }

        public Guid Id { get; private set; }

        public ClientProcess(IPEndPoint ipEndPoint, Guid id)
        {
            this.ConnectionEndPoint = ipEndPoint;
            if (id == Guid.Empty)
                Id = Guid.NewGuid();
            else
                Id = id;
        }
    }
}