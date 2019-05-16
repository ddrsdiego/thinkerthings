using MediatR;
using System.Threading.Tasks;
using ThinkerThings.TitanFlash.Bus.Topology;
using ThinkerThings.TitanFlash.Bus.Topology.Contracts;

namespace ThinkerThings.TitanFlash.Bus.Contracts
{
    public interface ITitanFlashPublisher
    {
        Task Publish(IRequest<Unit> @event, IEndPointConfigurator endPointConfigurator);

        Task Publish(IRequest<Unit> @event, Header header, IEndPointConfigurator endPointConfigurator);
    }
}