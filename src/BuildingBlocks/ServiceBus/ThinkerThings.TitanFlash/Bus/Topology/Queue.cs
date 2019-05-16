using System.Collections.Generic;
using ThinkerThings.TitanFlash.Bus.Topology.Contracts;

namespace ThinkerThings.TitanFlash.Bus.Topology
{
    public class Queue : IQueue
    {
        public Queue(string name, bool durable, bool exclusive, bool autoDelete, IDictionary<string, object> arguments)
        {
            QueueName = name;
            Durable = durable;
            Exclusive = exclusive;
            AutoDelete = autoDelete;
            Arguments = arguments;
        }

        public string QueueName { get; private set; }
        public bool Durable { get; private set; }
        public bool AutoDelete { get; private set; }
        public IDictionary<string, object> Arguments { get; private set; }
        public bool Exclusive { get; private set; }
    }
}
