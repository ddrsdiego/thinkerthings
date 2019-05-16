using System.Collections.Generic;

namespace ThinkerThings.TitanFlash.RabbitMq.Model
{
    public class QueueConfiguration : IQueueConfiguration
    {
        public QueueConfiguration(string queueName)
        {
            Name = queueName;
            AutoRecreate = true;
        }

        public string Name { get; }
        public bool Exclusive { get; set; }
        public bool Durable { get; set; }
        public bool AutoDelete { get; set; }
        public IDictionary<string, object> Arguments { get; set; }
        public bool AutoRecreate { get; set; }
    }
}
