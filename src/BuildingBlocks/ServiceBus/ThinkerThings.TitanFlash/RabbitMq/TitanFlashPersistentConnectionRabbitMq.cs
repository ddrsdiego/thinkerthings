using Microsoft.Extensions.Logging;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Net.Sockets;
using ThinkerThings.TitanFlash.Bus.Contracts;
using ThinkerThings.TitanFlash.Bus.Topology.Contracts;
using ThinkerThings.TitanFlash.RabbitMq.Monitor;

namespace ThinkerThings.TitanFlash.RabbitMq
{
    public class TitanFlashPersistentConnectionRabbitMq : IRabbitMQPersistentConnection
    {
        private bool _disposed;

        private IConnection _connection;
        private readonly IHostSetting _hostSetting;
        private readonly ILogger _logger;
        private readonly ITitanFlashMonitor _titanFlashMonitor;
        private readonly IConnectionFactory _connectionFactory;
        private readonly object sync_root = new object();

        public TitanFlashPersistentConnectionRabbitMq(IConnectionFactory connectionFactory, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(nameof(TitanFlashPersistentConnectionRabbitMq)) ?? throw new ArgumentException(nameof(loggerFactory));
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        }

        public TitanFlashPersistentConnectionRabbitMq(IHostSetting hostSetting, ILoggerFactory loggerFactory)
        {
            _titanFlashMonitor = TitanFlashMonitor.Create(_connectionFactory, loggerFactory);
            _hostSetting = hostSetting ?? throw new ArgumentNullException(nameof(hostSetting));
            _logger = loggerFactory.CreateLogger(nameof(TitanFlashPersistentConnectionRabbitMq)) ?? throw new ArgumentException(nameof(loggerFactory));

            _connectionFactory = CreateConnection();
        }

        private IConnectionFactory CreateConnection()
        {
            return new ConnectionFactory
            {
                Uri = _hostSetting.BuildUri(),
                AutomaticRecoveryEnabled = _hostSetting.AutomaticRecoveryEnabled,
                RequestedHeartbeat = 30,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(5)
            };
        }

        public bool IsConnected => _connection?.IsOpen == true && !_disposed;

        public IModel CreateModel()
        {
            if (!IsConnected)
            {
                _logger.LogError("Não há uma RabbitMQ connections disponível para executar essa ação.");
                throw new InvalidOperationException("Não há uma RabbitMQ connections disponível para executar essa ação.");
            }

            return _connection.CreateModel();
        }

        public bool TryConnect()
        {
            if (_disposed) return true;

            lock (sync_root)
            {
                var policy = Policy.Handle<SocketException>()
                                        .Or<BrokerUnreachableException>()
                                        .WaitAndRetry(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, _) =>
                                        {
                                            var endpoint = ((ConnectionFactory)_connectionFactory).Endpoint;
                                            _logger.LogWarning($"Problemas ao obter uma conexão com RabbitMQ. Endpoint: {endpoint} Erro: {ex.Message}");
                                        });

                policy.Execute(() => _connection = _connectionFactory.CreateConnection());

                if (IsConnected)
                {
                    _connection.ConnectionBlocked += OnConnectionBlocked;
                    _connection.ConnectionShutdown += OnConnectionShutdown;
                    _connection.CallbackException += OnCallbackException;

                    _logger.LogInformation($"RabbitMQ Persistent Connection adquirida uma conexão {_connection.Endpoint.HostName} e esta inscrita para eventos de falhas.");
                    return true;
                }
                else
                {
                    _logger.LogCritical("FATAL ERROR: RabbitMQ connections não pode ser criada e aberta.");
                    return false;
                }
            }
        }

        public bool HealthCheck() => _titanFlashMonitor.HealthCheck();

        public void Dispose()
        {
            if (_disposed) return;

            _disposed = true;

            if (_connection != null)
            {
                try
                {
                    _connection.Dispose();
                }
                catch (Exception ex)
                {
                    _logger.LogError("Ocorreu um erro ao realizar o dispose do objeto", ex);
                }
            }
        }

        private void OnConnectionBlocked(object sender, ConnectionBlockedEventArgs args)
        {
            if (_disposed) return;

            _logger.LogWarning($"Uma conexão RabbitMQ foi bloqueada. Motivo: {args.Reason}. Tentando o re-connect...");

            TryConnect();
        }

        private void OnConnectionShutdown(object sender, ShutdownEventArgs args)
        {
            if (_disposed) return;

            _logger.LogWarning($"Uma conexão RabbitMQ exceutou um shutdown. Causa: {args.Cause}. Tentando o re-connect...");

            TryConnect();
        }

        private void OnCallbackException(object sender, CallbackExceptionEventArgs args)
        {
            if (_disposed) return;

            _logger.LogWarning($"Uma conexão RabbitMQ lançou uma exceção. Detalhe: {args.Detail} + Exceção: {args.Exception.Message}. Trying to re-connect...");

            TryConnect();
        }
    }
}