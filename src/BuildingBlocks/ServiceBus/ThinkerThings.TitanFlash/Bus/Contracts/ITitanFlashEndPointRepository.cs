using System.Collections.Generic;
using System.Threading.Tasks;
using ThinkerThings.TitanFlash.Bus.Topology.Contracts;

namespace ThinkerThings.TitanFlash.Bus.Contracts
{
    public interface ITitanFlashEndPointRepository
    {
        Task<IEndPointConfigurator> Get(string queueName);

        Task Set(IEndPointConfigurator endPointsConfig);

        Task Set(IEnumerable<IEndPointConfigurator> endPointsConfigs);
    }
}