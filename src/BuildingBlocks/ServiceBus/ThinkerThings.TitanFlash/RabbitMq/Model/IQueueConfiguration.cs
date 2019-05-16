namespace ThinkerThings.TitanFlash.RabbitMq.Model
{
    public interface IQueueConfiguration : IConfiguration
    {
        bool Exclusive { get; set; }
        bool AutoRecreate { get; set; }
    }
}