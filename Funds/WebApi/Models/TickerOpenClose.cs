namespace WebApi.Models
{
    public class TickerOpenClose
    {
        public float AfterHours { get; set; }
        public float Close { get; set; }
        public float High { get; set; }
        public float Low { get; set; }
        public float Open { get; set; }
        public float PreMarket { get; set; }
        public long Volume { get; set; }

        //Primary Key Start
        public DateTime From { get; set; }
        public string Symbol = null!;
        //Primary Key End

        //Connected Values
        public virtual TickerDetails Ticker { get; set; } = null!;
        
    }
}
