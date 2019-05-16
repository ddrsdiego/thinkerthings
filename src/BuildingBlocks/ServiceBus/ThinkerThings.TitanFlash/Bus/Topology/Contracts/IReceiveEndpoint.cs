using MediatR;
using System.Collections.Generic;

namespace ThinkerThings.TitanFlash.Bus.Topology.Contracts
{
    public interface IReceiveEndpoint
    {
        void Subscribe<T>(IEndPointConfigurator endPointConfig) where T : IRequest<Unit>;

        IDictionary<string, IEndPointConfigurator> EndPointConfigurators { get; }
    }
}