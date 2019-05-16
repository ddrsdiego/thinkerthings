using System;
using System.Collections.Generic;
using ThinkerThings.TitanFlash.RabbitMq.Model;

namespace ThinkerThings.TitanFlash.Bus.Topology.Contracts
{
    public interface IEndPointConfigurator
    {
        string RoutingKey { get; }
        IExchangeConfiguration ExchangeConfiguration { get; }
        IQueueConfiguration QueueConfiguration { get; }
        IDictionary<string, object> Arguments { get; }
        bool IsComplete { get; }
        Type MessageType { get; set; }
    }
}