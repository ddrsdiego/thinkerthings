using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using ThinkerThings.TitanFlash.Bus.Topology.Contracts;

namespace ThinkerThings.TitanFlash.Bus.Contracts
{
    public abstract class TitanFlashHelperRabbitMq
    {
        protected readonly ILogger _logger;
        protected readonly IServiceProvider _serviceProvider;
        protected readonly IRabbitMQPersistentConnection _persistentConnection;
        protected readonly ITitanFlashEndPointFactory _titanFlashEndPointFactory;

        protected TitanFlashHelperRabbitMq(ILogger logger,
                                           IServiceProvider serviceProvider,
                                           IRabbitMQPersistentConnection persistentConnection,
                                           ITitanFlashEndPointFactory titanFlashEndPointFactory)
        {
            _logger = logger;
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _persistentConnection = persistentConnection ?? throw new ArgumentNullException(nameof(persistentConnection));
            _titanFlashEndPointFactory = titanFlashEndPointFactory ?? throw new ArgumentNullException(nameof(titanFlashEndPointFactory));
        }

        protected async Task CreateEndPoints(IModel channel, IReceiveEndpoint receiveEndpoint)
        {
            await _titanFlashEndPointFactory.CreateEndPoints(channel, receiveEndpoint).ConfigureAwait(false);
        }

        protected async Task ReCreateEndPoints(IModel channel, IReceiveEndpoint receiveEndpoint, string consumerTag)
        {
            await _titanFlashEndPointFactory.ReCreateEndPoints(channel, receiveEndpoint, consumerTag).ConfigureAwait(false);
        }

        protected async Task CreateEndPoints(IModel channel, IEndPointConfigurator endPointConfig)
        {
            await _titanFlashEndPointFactory.CreateEndPoints(channel, endPointConfig).ConfigureAwait(false);
        }

        protected IModel VerifyConnection()
        {
            if (!_persistentConnection.IsConnected)
                _persistentConnection.TryConnect();

            return _persistentConnection.CreateModel();
        }

        protected RetryPolicy CreatePolicy()
        {
            return Policy.Handle<BrokerUnreachableException>().
                Or<SocketException>().
                WaitAndRetry(retryCount: 5, sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), onRetry: (ex, _) => _logger.LogWarning(ex.Message));
        }
    }
}
