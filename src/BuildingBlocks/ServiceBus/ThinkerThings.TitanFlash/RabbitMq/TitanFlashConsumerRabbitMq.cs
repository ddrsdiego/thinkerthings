using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ThinkerThings.TitanFlash.Bus.Contracts;
using ThinkerThings.TitanFlash.Bus.Topology;
using ThinkerThings.TitanFlash.Bus.Topology.Contracts;
using ThinkerThings.TitanFlash.RabbitMq.Model;

namespace ThinkerThings.TitanFlash.RabbitMq
{
    public class TitanFlashConsumerRabbitMq : TitanFlashHelperRabbitMq, ITitanFlashConsumer
    {
        private IModel _consumerChannel;
        private IReceiveEndpoint _receiveEndpoint;
        private const int errorCodeDisconnected = 10060;

        public TitanFlashConsumerRabbitMq(ILoggerFactory loggerFactory, IServiceProvider serviceProvider, IRabbitMQPersistentConnection persistentConnection, ITitanFlashEndPointFactory titanFlashEndPointFactory)
            : base(loggerFactory.CreateLogger<TitanFlashConsumerRabbitMq>(), serviceProvider, persistentConnection, titanFlashEndPointFactory)
        {
        }

        public async Task CreateEndPoints() => await CreateConsumerChannel().ConfigureAwait(false);

        public void ReceiveEndpoint(Action<IReceiveEndpoint> endPoint)
        {
            if (endPoint == null)
                throw new ArgumentNullException(nameof(endPoint));

            _receiveEndpoint = new ReceiveEndpoint();
            endPoint?.Invoke(_receiveEndpoint);
        }

        private async Task<IModel> CreateConsumerChannel()
        {
            var channel = VerifyConnection();

            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            await CreateEndPoints(channel, _receiveEndpoint).ConfigureAwait(false);

            foreach (var endPointConfig in _receiveEndpoint.EndPointConfigurators.Values)
            {
                RegistryConsumer(channel, endPointConfig);
            }

            channel.CallbackException += (sender, eventArgs) => SetCallBackException().GetAwaiter().GetResult();

            return channel;
        }

        private async Task<IModel> ReCreateConsumerChannel(string consumerTag)
        {
            var channel = VerifyConnection();

            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            await ReCreateEndPoints(channel, _receiveEndpoint, consumerTag).ConfigureAwait(false);

            var _endPointConfigurators = _receiveEndpoint.EndPointConfigurators.Values.Where(e
                => string.IsNullOrEmpty(consumerTag) || e.QueueConfiguration.Name.Equals(consumerTag, StringComparison.InvariantCultureIgnoreCase));

            foreach (var endPointConfig in _endPointConfigurators)
            {
                RegistryConsumer(channel, endPointConfig);
            }

            channel.CallbackException += (sender, eventArgs) => SetCallBackException().GetAwaiter().GetResult();

            return channel;
        }

        protected virtual void RegistryConsumer(IModel channel, IEndPointConfigurator endPointConfig)
        {
            var consumer = new EventingBasicConsumer(channel);

            AutoRecreateQueue(consumer, endPointConfig.QueueConfiguration);

            consumer.Received += async (model, eventArgs) =>
            {
                try
                {
                    await ProcessEvent(eventArgs, endPointConfig).ConfigureAwait(continueOnCapturedContext: false);
                    channel.BasicAck(deliveryTag: eventArgs.DeliveryTag, multiple: false);
                }
                catch (JsonReaderException ex)
                {
                    var message = eventArgs?.Body == null ? string.Empty : Encoding.UTF8.GetString(eventArgs?.Body);

                    _logger.LogError($"Erro ao deserializar o evento. Motivo: {ex.Message}", ex);
                    channel.BasicNack(deliveryTag: eventArgs.DeliveryTag, multiple: false, requeue: false);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Erro ao processar o evento: {eventArgs.BasicProperties?.Type?.ToString()}. Motivo: {ex.Message}", ex);
                    channel.BasicNack(deliveryTag: eventArgs.DeliveryTag, multiple: false, requeue: true);
                }
            };

            channel.BasicConsume(queue: endPointConfig.QueueConfiguration.Name, autoAck: false, consumer: consumer, consumerTag: $"{endPointConfig.QueueConfiguration.Name}: MachineName {System.Environment.MachineName}");
        }

        private void AutoRecreateQueue(EventingBasicConsumer consumer, IQueueConfiguration queueConfiguration)
        {
            if (queueConfiguration.AutoRecreate)
            {
                consumer.ConsumerCancelled += async (sender, eventArgs) =>
                {
                    try
                    {
                        var consumerSender = (EventingBasicConsumer)sender;

                        var errorCode = ((SocketException)consumerSender.ShutdownReason?.Cause)?.ErrorCode;

                        if (!consumerSender.Model.IsClosed && consumerSender.Model.IsOpen && errorCode.GetValueOrDefault() != errorCodeDisconnected)
                        {
                            await ReCreateConsumerChannel(ClearConsumerTag(consumerSender.ConsumerTag)).ConfigureAwait(false);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Erro ao processar evento de cancelamento da fila: {eventArgs?.ConsumerTag}. Motivo: {ex.Message}");
                    }
                };
            }
        }

        protected async Task ProcessEvent(BasicDeliverEventArgs eventArgs, IEndPointConfigurator endPointConfig)
        {
            var message = Encoding.UTF8.GetString(eventArgs.Body);

            var eventType = endPointConfig.MessageType;

            var integrationEvent = (IRequest<Unit>)JsonConvert.DeserializeObject(message, eventType);

            using (var scope = _serviceProvider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                await mediator.Send(integrationEvent).ConfigureAwait(continueOnCapturedContext: false);
            }
        }

        private async Task SetCallBackException()
        {
            _consumerChannel?.Dispose();
            _consumerChannel = await CreateConsumerChannel().ConfigureAwait(false);
        }

        private static string ClearConsumerTag(string consumerTag)
        {
            if (!string.IsNullOrEmpty(consumerTag))
                return consumerTag.Split(new char[] { ':' }).FirstOrDefault()?.Trim();

            return consumerTag;
        }
    }
}
