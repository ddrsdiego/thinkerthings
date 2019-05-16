using RabbitMQ.Client;

namespace ThinkerThings.TitanFlash.RabbitMq.Model
{
    public static class ExchangeCustomType
    {
        public const string Direct = ExchangeType.Direct;
        public const string Fanout = ExchangeType.Fanout;
        public const string Headers = ExchangeType.Headers;
        public const string Topic = ExchangeType.Topic;
        public const string Delayed = "x-delayed-message";
    }
}
