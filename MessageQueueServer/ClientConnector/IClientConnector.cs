using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageQueueServer.ClientConnector
{
    /// <summary>
    /// Describes the interface of a component responsible to communicating with a client
    /// </summary>
    internal interface IClientConnector : IDisposable
    {

        ClientProcess ClientDescriptor {  get; init;}

        Task<bool> SendMessage(Message msg);

        Task<bool> IsAlive();
        
    }
}
