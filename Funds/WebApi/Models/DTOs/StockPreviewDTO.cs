namespace WebApi.Models.DTOs
{
    public class StockPreviewDTO
    {
        public bool Active { get; set; }
        public string? Cik { get; set; }
        public string? Composite_Figi { get; set; }
        public string? Currency_Name { get; set; }
        public DateTime Last_Updated_Utc { get; set; }
        public string? Locale { get; set; }
        public string? Market { get; set; }
        public string? Name { get; set; }
        public string? Primary_Exchange { get; set; }
        public string? Share_Class_Figi { get; set; }
        public string? Ticker { get; set; }
        public string? Type { get; set; }
    }
}
