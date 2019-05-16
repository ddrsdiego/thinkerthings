using MediatR;
using ThinkerThings.TitanFlash.Bus.Topology.Contracts;

namespace ThinkerThings.TitanFlash.Bus
{
    public interface IMessageConnector<in TMessage> where TMessage : IRequest<Unit>
    {
        void Publish(TMessage message);

        IEndPointConfigurator EndPoint();
    }
}