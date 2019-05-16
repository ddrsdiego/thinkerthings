using System.Collections.Generic;
using ThinkerThings.TitanFlash.Bus.Topology.Contracts;

namespace ThinkerThings.TitanFlash.Bus.Topology
{
    public class Exchange : IExchange
    {

        public Exchange(string name, string type, bool durable, bool autoDelete, IDictionary<string, object> arguments)
        {
            ExchangeName = name;
            Type = type;
            Durable = durable;
            AutoDelete = autoDelete;
            Arguments = arguments;
        }

        public string ExchangeName { get; private set; }
        public bool Durable { get; private set; }
        public bool AutoDelete { get; private set; }
        public IDictionary<string, object> Arguments { get; private set; }
        public string Type { get; private set; }
    }
}
