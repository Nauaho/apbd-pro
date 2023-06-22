namespace WebApi.Models.DTOs
{
    public class DailyOpenCloseDTO
    {
        public float AfterHours { get; set; }
        public float Close { get; set; }
        public float High { get; set; }
        public float Low { get; set; }
        public float Open { get; set; }
        public float PreMarket { get; set; }
        public long Volume { get; set; }
        public DateOnly From { get; set; }
        public string Symbol { get; set; } = null!;

    }
}
