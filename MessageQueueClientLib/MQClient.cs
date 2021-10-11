using System.Net;

namespace MessageQueueClientLib
{
    public class MQClient
    {
        IPAddress ServerAddress;

        //accept one or more messageQueue server address it prioritizes the first of the list
        public MQClient(IPAddress  server)
        {
            this.ServerAddress = server;

        }

        /// <summary>
        /// Subscribes to the defined queue Throws an exception if the queue doesn't exist
        /// </summary>
        /// <param name="queueIdentifier"></param>
        public void Subscribe(string queueIdentifier)
        {

        }

        /// <summary>
        /// Publishes a message in the given queue
        /// </summary>
        /// <param name="queueIdentifier"></param>
        /// <param name="message"></param>
        public void Publish(string queueIdentifier, string message)
        {

        }

        

    }
}