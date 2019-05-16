using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading.Tasks;
using ThinkerThings.TitanFlash.Bus.Contracts;
using ThinkerThings.TitanFlash.Bus.Topology;
using ThinkerThings.TitanFlash.Bus.Topology.Contracts;

namespace ThinkerThings.TitanFlash.RabbitMq
{
    public class TitanFlashPublisherRabbitMq : TitanFlashHelperRabbitMq, ITitanFlashPublisher
    {
        private const string CONTENT_TYPE = "application/json";

        public TitanFlashPublisherRabbitMq(IRabbitMQPersistentConnection persistentConnection,
                                           ILoggerFactory loggerFactory,
                                           IServiceProvider serviceProvider,
                                           ITitanFlashEndPointFactory titanFlashEndPointFactory)
            : base(loggerFactory.CreateLogger<TitanFlashPublisherRabbitMq>(), serviceProvider, persistentConnection, titanFlashEndPointFactory)
        {
        }

        public async Task Publish(IRequest<Unit> @event, IEndPointConfigurator endPointConfigurator)
        {
            await Publish(@event, Header.Default(), endPointConfigurator).ConfigureAwait(false);
        }

        public async Task Publish(IRequest<Unit> @event, Header header, IEndPointConfigurator endPointConfigurator)
        {
            using (var channel = VerifyConnection())
            {
                await RunPublishSettings(@event, header, channel, endPointConfigurator).ConfigureAwait(false);
            }
        }

        private async Task RunPublishSettings(IRequest<Unit> @event, Header header, IModel channel, IEndPointConfigurator endPointConfigurator)
        {
            await Task.Run(() =>
            {
                CreateEndPoints(channel, endPointConfigurator).GetAwaiter().GetResult();

                var message = JsonConvert.SerializeObject(@event);
                var body = Encoding.UTF8.GetBytes(message);

                var basicProperties = channel.CreateBasicProperties();

                basicProperties.Persistent = true;
                basicProperties.ContentType = CONTENT_TYPE;
                basicProperties.Type = @event.GetType().AssemblyQualifiedName;
                basicProperties.Headers = header.Values;

                var policy = CreatePolicy();

                policy.Execute(() =>
                {
                    _logger.LogDebug($"Publicando Mensagem na Exchange {endPointConfigurator.ExchangeConfiguration.Name} no RoutingKey {endPointConfigurator.RoutingKey}. Mensagem {message}");

                    channel.BasicPublish(endPointConfigurator.ExchangeConfiguration.Name,
                                         endPointConfigurator.RoutingKey,
                                         basicProperties,
                                         body);
                });
            }).ConfigureAwait(false);
        }
    }
}
