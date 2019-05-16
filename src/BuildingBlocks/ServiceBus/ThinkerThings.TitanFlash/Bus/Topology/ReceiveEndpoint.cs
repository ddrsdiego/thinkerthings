using MediatR;
using System.Collections.Generic;
using ThinkerThings.TitanFlash.Bus.Topology.Contracts;

namespace ThinkerThings.TitanFlash.Bus.Topology
{
    public class ReceiveEndpoint : IReceiveEndpoint
    {
        public ReceiveEndpoint()
        {
            EndPointConfigurators = new Dictionary<string, IEndPointConfigurator>();
        }

        public IDictionary<string, IEndPointConfigurator> EndPointConfigurators { get; }

        public void Subscribe<T>(IEndPointConfigurator endPointConfig) where T : IRequest<Unit>
        {
            if (!EndPointConfigurators.ContainsKey(endPointConfig.RoutingKey))
            {
                endPointConfig.MessageType = typeof(T);
                EndPointConfigurators.Add(endPointConfig.RoutingKey, endPointConfig);
            }
        }
    }
}