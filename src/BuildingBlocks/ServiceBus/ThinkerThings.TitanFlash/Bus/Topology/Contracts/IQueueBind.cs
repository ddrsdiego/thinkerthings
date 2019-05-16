using System.Collections.Generic;

namespace ThinkerThings.TitanFlash.Bus.Topology.Contracts
{
    public interface IQueueBind
    {
        string RoutingKey { get; set; }
        IExchange Exchange { get; set; }
        IQueue Queue { get; set; }
        IDictionary<string, object> Arguments { get; set; }
    }
}