namespace WebApi.Models.DTOs
{
    public class OhlcDTO
    {
        public float C { get; set; }
        public float H { get; set; }
        public float L { get; set; }
        public long N { get; set; }
        public float O { get; set; }
        public long V { get; set; }
        public float Vw { get; set; }
        public long T { get; set; }
        public long Multuplier { get; set; }
        public string Timespan { get; set; } = null!;
        public string Symbol { get; set; } = null!;
    }
}
