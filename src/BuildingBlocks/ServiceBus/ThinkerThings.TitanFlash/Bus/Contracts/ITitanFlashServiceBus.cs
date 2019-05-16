using MediatR;
using System;
using System.Threading.Tasks;
using ThinkerThings.TitanFlash.Bus.Topology;
using ThinkerThings.TitanFlash.Bus.Topology.Contracts;

namespace ThinkerThings.TitanFlash.Bus.Contracts
{
    public interface ITitanFlashServiceBus : IDisposable
    {
        void Publish(IRequest<Unit> @event);

        void Publish(IRequest<Unit> @event, Header header);

        void Publish(IRequest<Unit> @event, IEndPointConfigurator endPointConfigurator);

        void Publish(IRequest<Unit> @event, Header header, IEndPointConfigurator endPointConfigurator);

        void ReceiveEndpoint(Action<IReceiveEndpoint> endPoint);

        Task CreateEndPointsAsync();
    }
}