using RabbitMQ.Client;
using System;

namespace ThinkerThings.TitanFlash.Bus.Contracts
{
    public interface IRabbitMQPersistentConnection : IDisposable
    {
        bool IsConnected { get; }
        bool TryConnect();
        IModel CreateModel();
        bool HealthCheck();
    }
}