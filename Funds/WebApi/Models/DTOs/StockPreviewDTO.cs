namespace WebApi.Models.DTOs
{
    public class StockPreviewDTO
    {
        public bool active { get; set; }
        public string? cik { get; set; }
        public string? composite_Figi { get; set; }
        public string? currency_Name { get; set; }
        public DateTime last_Updated_Utc { get; set; }
        public string? locale { get; set; }
        public string? market { get; set; }
        public string? name { get; set; }
        public string? primary_Exchange { get; set; }
        public string? share_Class_Figi { get; set; }
        public string ticker { get; set; } = null!;
        public string? type { get; set; }
    }
}
