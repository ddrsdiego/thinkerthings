using System.Collections.Generic;

namespace ThinkerThings.TitanFlash.Bus.Topology.Contracts
{
    public interface IExchange
    {
        string ExchangeName { get; }
        string Type { get; }
        bool Durable { get; }
        bool AutoDelete { get; }
        IDictionary<string,object> Arguments { get; }
    }
}
