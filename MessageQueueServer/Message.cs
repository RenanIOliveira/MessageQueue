namespace MessageQueueServer
{
    /// <summary>
    /// Corresponds to a message inserted in a queue
    /// </summary>
    internal class Message
    {
        public Guid MessageId { get; private set; }

        public string Sender  { get; private set; }

        public byte[] Payload {  get; private set; }


        public Message(string sender, byte[] payload)
        {
            this.MessageId = Guid.NewGuid();
            this.Sender = sender;
            this.Payload = payload;
        }
    }
}