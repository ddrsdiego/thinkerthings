namespace ThinkerThings.TitanFlash.RabbitMq.Model
{
    public interface IExchangeConfiguration : IConfiguration
    {
        string Type { get; set; }
    }
}