namespace MessageQueueClientLib.ServerConnector
{
    /// <summary>
    /// Describes the interface of a component responsible to communicating with a client
    /// </summary>
    internal interface IServerConnector : IDisposable
    {

        ServerModel server { get; init;}

        Task<(bool,string)> SendMessage(NetworkMessage msg);

        
    }
}
