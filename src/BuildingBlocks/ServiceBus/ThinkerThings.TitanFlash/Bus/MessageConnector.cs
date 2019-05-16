using MediatR;
using RabbitMQ.Client;
using System.Collections.Generic;
using ThinkerThings.TitanFlash.Bus.Contracts;
using ThinkerThings.TitanFlash.Bus.Topology;
using ThinkerThings.TitanFlash.Bus.Topology.Contracts;
using ThinkerThings.TitanFlash.RabbitMq.Model;

namespace ThinkerThings.TitanFlash.Bus
{
    public abstract class MessageConnector<TMessage> : IMessageConnector<TMessage>
        where TMessage : IRequest<Unit>
    {
        private readonly MessagingOption _messagingOption;
        private readonly ITitanFlashServiceBus _titanFlashServiceBus;

        protected MessageConnector(ITitanFlashServiceBus titanFlashServiceBus, MessagingOption messagingOption)
        {
            _messagingOption = messagingOption;
            _titanFlashServiceBus = titanFlashServiceBus;
        }

        public virtual IEndPointConfigurator EndPoint()
        {
            var exchange = new ExchangeConfiguration(_messagingOption.Exchange)
            {
                Type = _messagingOption.ExchangeType ?? ExchangeType.Direct,
                Durable = true,
                AutoDelete = false,
                Arguments = new Dictionary<string, object>()
            };

            var queue = new QueueConfiguration(_messagingOption.Queue)
            {
                Durable = true,
                Exclusive = false,
                AutoDelete = false,
                Arguments = new Dictionary<string, object>()
            };

            return new EndPointConfigurator(exchange,
                                            queue,
                                            routingKey: _messagingOption.RoutingKey);
        }

        public virtual void Publish(TMessage message)
            => _titanFlashServiceBus.Publish(message, EndPoint());
    }
}