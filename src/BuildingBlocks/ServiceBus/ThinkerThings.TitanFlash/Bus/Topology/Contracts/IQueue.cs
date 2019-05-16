using System.Collections.Generic;

namespace ThinkerThings.TitanFlash.Bus.Topology.Contracts
{
    public interface IQueue
    {
        string QueueName { get; }
        bool Durable { get; }
        bool Exclusive { get; }
        bool AutoDelete { get; }
        IDictionary<string, object> Arguments { get; }
    }
}
