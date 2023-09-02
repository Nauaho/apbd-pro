namespace Funds.Models
{
    public class StocksPreview
    {
        private string? locale;
        public string Ticker { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? IconUrl { get; set; }
        public string? Locale 
        { 
            get => string.IsNullOrEmpty(locale) ? null :
                   locale == "global" ? "International" : 
                    new System.Globalization.RegionInfo(locale).DisplayName;
            set => locale = value;
        }

        public Dictionary<string, string?> YieldProps()
        {
            return new Dictionary<string, string?> { 
                {"Symbol", Ticker},
                {"Name", Name},
                {"Logo", IconUrl},
                {"Country", Locale},
            };
        }
    }
}
