namespace WebApi.Models.DTOs
{
    public class OpenCloseDTO
    {
        public string status { get; set; } = null!;
        public DateTime from { get; set; }
        public string symbol { get; set; } = null!;
        public float open { get; set; }
        public float high { get; set; }
        public float low { get; set; }
        public float close { get; set; }
        public float afterHours { get; set; }
        public float preMarket { get; set; }
        public long volume { get; set; }
    }
}
