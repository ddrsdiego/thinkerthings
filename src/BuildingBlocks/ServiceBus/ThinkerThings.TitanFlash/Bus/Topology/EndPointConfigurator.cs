using System;
using System.Collections.Generic;
using ThinkerThings.TitanFlash.Bus.Topology.Contracts;
using ThinkerThings.TitanFlash.RabbitMq.Model;

namespace ThinkerThings.TitanFlash.Bus.Topology
{
    public class EndPointConfigurator : IEndPointConfigurator
    {
        public EndPointConfigurator(IExchangeConfiguration exchange, IQueueConfiguration queue, string routingKey)
        {
            RoutingKey = routingKey;
            ExchangeConfiguration = exchange;
            QueueConfiguration = queue;
        }

        public string RoutingKey { get; private set; }
        public IExchangeConfiguration ExchangeConfiguration { get; private set; }
        public IQueueConfiguration QueueConfiguration { get; private set; }
        public IDictionary<string, object> Arguments { get; private set; }

        public bool IsComplete
        {
            get; private set;
        }

        public Type MessageType { get; set; }
    }
}
