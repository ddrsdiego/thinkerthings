namespace ThinkerThings.TitanFlash.Bus
{
    public class MessagingOption
    {
        public string Exchange { get; set; }
        public string Queue { get; set; }
        public string RoutingKey { get; set; }
        public string ExchangeType { get; set; }
    }
}