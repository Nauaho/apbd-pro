namespace WebApi.Models
{
    public class Bar
    {
        public float C { get; set; }
        public float H { get; set; }
        public float L { get; set; }
        public long N { get; set; }
        public float O { get; set; }
        public long T { get; set; }
        public long V { get; set; }
        public float Vw { get; set; }
    }

    public class TickerOHLC
    {
        public bool Adjusted { get; set; }
        public string NextUrl { get; set; } = null!;
        public long QueryCount { get; set; }
        public string RequestId { get; set; } = null!;
        public List<Bar> Bars { get; set; } = new List<Bar>();
        public long ResultsCount { get; set; }
        public string Ticker { get; set; } = null!;
    }
}
