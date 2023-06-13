
namespace WebApi.Models
{
    public class Localisation
    {
        public string Address { get; set; } = null!;
        public string City { get; set; } = null!;
        public string State { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
    }

    public class Branding
    {
        public string LogoUrl { get; set; } = null!;
        public string IconUrl { get; set; } = null!;
    }

    public class TickerDetails
    {
        public string Ticker { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Market { get; set; } = null!;
        public string Locale { get; set; } = null!;
        public string PrimaryExchange { get; set; } = null!;
        public string Type { get; set; } = null!;
        public bool Active { get; set; }
        public string CurrencyName { get; set; } = null!;
        public string Cik { get; set; } = null!;
        public string CompositeFigi { get; set; } = null!;
        public string ShareClassFigi { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public Localisation Address { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string SicCode { get; set; } = null!;
        public string SicDescription { get; set; } = null!;
        public string TickerRoot { get; set; } = null!;
        public string HomepageUrl { get; set; } = null!;
        public long TotalEmployees { get; set; }
        public DateTime ListDate { get; set; }
        public Branding Branding { get; set; } = null!;
        public long ShareClassSharesOutstanding { get; set; }
        public long WeightedSharesOutstanding { get; set; }
        public int RoundLot { get; set; }
    }
}