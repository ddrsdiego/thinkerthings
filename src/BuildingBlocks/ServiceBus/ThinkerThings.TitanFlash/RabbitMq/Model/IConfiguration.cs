using System.Collections.Generic;

namespace ThinkerThings.TitanFlash.RabbitMq.Model
{
    public interface IConfiguration
    {
        IDictionary<string, object> Arguments { get; set; }
        bool AutoDelete { get; set; }
        bool Durable { get; set; }
        string Name { get; }
    }
}