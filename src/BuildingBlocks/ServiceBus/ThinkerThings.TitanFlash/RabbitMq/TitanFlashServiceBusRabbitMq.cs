using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using ThinkerThings.TitanFlash.Bus.Contracts;
using ThinkerThings.TitanFlash.Bus.Topology;
using ThinkerThings.TitanFlash.Bus.Topology.Contracts;
using ThinkerThings.TitanFlash.RabbitMq.Model;

namespace ThinkerThings.TitanFlash.RabbitMq
{
    public class TitanFlashServiceBusRabbitMq : ITitanFlashServiceBus
    {
        private readonly ILogger _logger;
        private readonly ITitanFlashConsumer _titanFlashConsumer;
        private readonly ITitanFlashPublisher _titanFlashPublisher;
        private readonly ITitanFlashEndPointFactory _titanFlashEndPointFactory;

        public TitanFlashServiceBusRabbitMq(ITitanFlashPublisher titanFlashPublisher, ITitanFlashConsumer titanFlashConsumer, ITitanFlashEndPointFactory titanFlashEndPointFactory, ILoggerFactory loggerFactory)
        {
            if (loggerFactory == null)
                throw new ArgumentNullException(nameof(loggerFactory));

            _logger = loggerFactory.CreateLogger<TitanFlashServiceBusRabbitMq>();
            _titanFlashConsumer = titanFlashConsumer ?? throw new ArgumentNullException(nameof(titanFlashConsumer));
            _titanFlashPublisher = titanFlashPublisher ?? throw new ArgumentNullException(nameof(titanFlashPublisher));
            _titanFlashEndPointFactory = titanFlashEndPointFactory ?? throw new ArgumentNullException(nameof(titanFlashEndPointFactory));
        }

        public void Publish(IRequest<Unit> @event) => Publish(@event, Header.Default());

        public void Publish(IRequest<Unit> @event, Header header)
        {
            var endPointConfigurator = new EndPointConfigurator(new ExchangeConfiguration($"EX.{@event.GetType().Namespace}"),
                                                                new QueueConfiguration($"QL.{@event.GetType().FullName}"),
                                                                @event.GetType().FullName);
            Publish(@event, header, endPointConfigurator);
        }

        public void Publish(IRequest<Unit> @event, IEndPointConfigurator endPointConfigurator) => Publish(@event, Header.Default(), endPointConfigurator);

        public void Publish(IRequest<Unit> @event, Header header, IEndPointConfigurator endPointConfigurator)
        {
            _titanFlashPublisher.Publish(@event, header, endPointConfigurator);
        }

        public void ReceiveEndpoint(Action<IReceiveEndpoint> endPoint)
        {
            _titanFlashConsumer.ReceiveEndpoint(endPoint);
        }

        public async Task CreateEndPointsAsync() => await _titanFlashConsumer.CreateEndPoints().ConfigureAwait(false);

        public void Dispose()
        {
        }
    }
}