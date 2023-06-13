namespace WebApi.Models
{
    public class TickerOpenClose
    {
        public float AfterHours { get; set; }
        public float Close { get; set; }
        public DateTime From { get; set; }
        public float High { get; set; }
        public float Low { get; set; }
        public float Open { get; set; }
        public float PreMarket { get; set; }
        public string Symbol { get; set; } = null!;
        public long Volume { get; set; }
    }
}
