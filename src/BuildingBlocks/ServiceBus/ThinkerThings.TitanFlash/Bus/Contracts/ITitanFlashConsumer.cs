using System;
using System.Threading.Tasks;
using ThinkerThings.TitanFlash.Bus.Topology.Contracts;

namespace ThinkerThings.TitanFlash.Bus.Contracts
{
    public interface ITitanFlashConsumer
    {
        void ReceiveEndpoint(Action<IReceiveEndpoint> endPoint);

        Task CreateEndPoints();
    }
}