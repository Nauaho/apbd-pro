namespace WebApi.Models.DTOs
{
    public class BarDTO
    {
        public float c { get; set; }
        public float h { get; set; }
        public float l { get; set; }
        public long n { get; set; }
        public float o { get; set; }
        public long t { get; set; }
        public long v { get; set; }
        public float vw { get; set; }
    }

    public class TickerOhlcDTO
    {
        public bool adjusted { get; set; }
        public string next_url { get; set; } = null!;
        public long queryCount { get; set; }
        public string request_id { get; set; } = null!;
        public List<BarDTO> results { get; set; } = new List<BarDTO>();
        public long resultsCount { get; set; }
        public string status { get; set; } = null!;
        public string ticker { get; set; } = null!;
    }

}
