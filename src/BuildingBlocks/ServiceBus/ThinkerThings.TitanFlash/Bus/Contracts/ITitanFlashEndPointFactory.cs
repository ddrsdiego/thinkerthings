using RabbitMQ.Client;
using System.Threading.Tasks;
using ThinkerThings.TitanFlash.Bus.Topology.Contracts;

namespace ThinkerThings.TitanFlash.Bus.Contracts
{
    public interface ITitanFlashEndPointFactory
    {
        Task CreateEndPoints(IModel channel, IReceiveEndpoint receiveEndpoint);

        Task CreateEndPoints(IModel channel, IEndPointConfigurator endPointConfig);

        Task ReCreateEndPoints(IModel channel, IReceiveEndpoint receiveEndpoint, string consumerTag);
    }
}
