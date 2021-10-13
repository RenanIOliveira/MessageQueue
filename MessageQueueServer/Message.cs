namespace MessageQueueServer
{
    /// <summary>
    /// Corresponds to a message inserted in a queue
    /// </summary>
    internal class Message
    {
        public Guid MessageId { get; private set; }

        /// <summary>
        /// Client Process that sent the message
        /// </summary>
        public Guid Sender  { get; private set; }

        public string Content {  get; private set; }


        public Message(Guid sender, string content)
        {
            this.MessageId = Guid.NewGuid();
            this.Sender = sender;
            this.Content = content;
        }
    }
}