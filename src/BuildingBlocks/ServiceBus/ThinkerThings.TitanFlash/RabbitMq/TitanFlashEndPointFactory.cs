using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Linq;
using System.Threading.Tasks;
using ThinkerThings.TitanFlash.Bus.Contracts;
using ThinkerThings.TitanFlash.Bus.Topology;
using ThinkerThings.TitanFlash.Bus.Topology.Contracts;
using ThinkerThings.TitanFlash.RabbitMq.Model;

namespace ThinkerThings.TitanFlash.RabbitMq
{
    public class TitanFlashEndPointFactory : ITitanFlashEndPointFactory
    {
        private readonly ILogger _logger;
        private readonly IRabbitMQPersistentConnection _persistentConnection;

        public TitanFlashEndPointFactory(ILoggerFactory loggerFactory, IRabbitMQPersistentConnection persistentConnection)
        {
            _persistentConnection = persistentConnection;
            _logger = loggerFactory.CreateLogger(nameof(TitanFlashConsumerRabbitMq)) ?? throw new ArgumentException(nameof(loggerFactory));
        }

        public async Task CreateEndPoints(IModel channel, IReceiveEndpoint receiveEndpoint)
        {
            if (receiveEndpoint == null
                || receiveEndpoint.EndPointConfigurators == null
                || receiveEndpoint.EndPointConfigurators.Count == 0)
            {
                _logger.LogWarning("Não existe configuração de endpoint definidos");
            }

            foreach (var endPoint in receiveEndpoint.EndPointConfigurators.Values)
            {
                await CreateEndPoints(channel, endPoint).ConfigureAwait(false);
            }
        }

        public async Task ReCreateEndPoints(IModel channel, IReceiveEndpoint receiveEndpoint, string consumerTag)
        {
            if (receiveEndpoint == null
                || receiveEndpoint.EndPointConfigurators == null
                || receiveEndpoint.EndPointConfigurators.Count == 0)
            {
                _logger.LogWarning("Não existe configuração de endpoint definidos");
            }

            foreach (var endPoint in receiveEndpoint.EndPointConfigurators.Values
                .Where(v => v.QueueConfiguration.Name.Equals(consumerTag, StringComparison.InvariantCultureIgnoreCase)))
            {
                await CreateEndPoints(channel, endPoint).ConfigureAwait(false);
            }
        }

        public async Task CreateEndPoints(IModel channel, IEndPointConfigurator endPointConfig)
        {
            await DeclareExchange(channel, CreateExchangeConfiguration(endPointConfig.ExchangeConfiguration)).ConfigureAwait(false);
            await DeclareQueue(channel, CreateQueueConfiguration(endPointConfig.QueueConfiguration)).ConfigureAwait(false);
            await Bind(channel, endPointConfig).ConfigureAwait(false);
        }

        private static IExchange CreateExchangeConfiguration(IExchangeConfiguration exchangeConfiguration)
        {
            return new Exchange(
                name: exchangeConfiguration.Name,
                type: exchangeConfiguration.Type,
                durable: exchangeConfiguration.Durable,
                autoDelete: exchangeConfiguration.AutoDelete,
                arguments: exchangeConfiguration.Arguments);
        }

        private static IQueue CreateQueueConfiguration(IQueueConfiguration queueConfiguration)
        {
            return new Queue(
                    name: queueConfiguration.Name,
                    durable: queueConfiguration.Durable,
                    exclusive: queueConfiguration.Exclusive,
                    autoDelete: queueConfiguration.AutoDelete,
                    arguments: queueConfiguration.Arguments);
        }

        private async Task DeclareExchange(IModel channel, IExchange exchange)
        {
            await Task.Run(() =>
            {
                _logger.LogDebug($"Declarando Exchange {exchange.ExchangeName}");

                channel.ExchangeDeclare(exchange.ExchangeName, exchange.Type, exchange.Durable, exchange.AutoDelete, exchange.Arguments);
            }).ConfigureAwait(false);
        }

        private async Task DeclareQueue(IModel channel, IQueue queue)
        {
            await Task.Run(() =>
            {
                _logger.LogDebug($"Declarando Fila {queue.QueueName}");

                channel.QueueDeclare(queue.QueueName, queue.Durable, queue.Exclusive, queue.AutoDelete, queue.Arguments);
            }).ConfigureAwait(false);
        }

        private async Task Bind(IModel channel, IEndPointConfigurator endPointConfig)
        {
            await Task.Run(() =>
            {
                _logger.LogDebug($"Criando o Bind entre a exchange {endPointConfig.ExchangeConfiguration.Name} e a fila {endPointConfig.QueueConfiguration.Name}");

                channel.QueueBind(endPointConfig.QueueConfiguration.Name, endPointConfig.ExchangeConfiguration.Name, endPointConfig.RoutingKey, endPointConfig.Arguments);
            }).ConfigureAwait(false);
        }
    }
}