namespace WebApi.Models
{
    public class Bar
    {
        
    }

    public class TickerOHLC
    {
        public float C { get; set; }
        public float H { get; set; }
        public float L { get; set; }
        public long N { get; set; }
        public float O { get; set; }
        public long T { get; set; }
        public long V { get; set; }
        public float Vw { get; set; }

        //Primaty Key Start
        public long Multuplier { get; set; }
        public string Timespan { get; set; } = null!;
        public string Symbol { get; set; } = null!;
        //Primaty Key End
        public virtual TickerDetails Ticker { get; set; } = null!;
    }
}
