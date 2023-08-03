namespace Funds.Models
{
    public class StocksPreview
    {
        public string Ticker { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? IconUrl { get; set; }
        public string? Locale { get; set; }
    }
}
