using System.Collections.Generic;

namespace ThinkerThings.TitanFlash.RabbitMq.Model
{
    public class ExchangeConfiguration : IExchangeConfiguration
    {
        public ExchangeConfiguration(string exchangeName)
        {
            Name = exchangeName;
        }

        public string Name { get; }
        public string Type { get; set; } = ExchangeCustomType.Direct;
        public bool Durable { get; set; }
        public bool AutoDelete { get; set; }
        public IDictionary<string, object> Arguments { get; set; } = new Dictionary<string, object>();
    }
}
