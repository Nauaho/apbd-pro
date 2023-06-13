using System;
using System.Collections.Generic;

namespace WebApi.Models
{

    public class AddressDTO
    {
        public string address1 { get; set; } = null!;
        public string city { get; set; } = null!;
        public string state { get; set; } = null!;
        public string postal_code { get; set; } = null!;
    }

    public class BrandingDTO
    {
        public string logo_url { get; set; } = null!;
        public string icon_url { get; set; } = null!;
    }

    public class TickerDetailsDTO
    {
        public string ticker { get; set; } = null!;
        public string name { get; set; } = null!;
        public string market { get; set; } = null!;
        public string locale { get; set; } = null!;
        public string primary_exchange { get; set; } = null!;
        public string type { get; set; } = null!;
        public bool active { get; set; }
        public string currency_name { get; set; } = null!;
        public string cik { get; set; } = null!;
        public string composite_figi { get; set; } = null!;
        public string share_class_figi { get; set; } = null!;
        public string phone_number { get; set; } = null!;
        public Address address { get; set; } = null!;
        public string description { get; set; } = null!;
        public string sic_code { get; set; } = null!;
        public string sic_description { get; set; } = null!;
        public string ticker_root { get; set; } = null!;
        public string homepage_url { get; set; } = null!;
        public long total_employees { get; set; }
        public DateTime list_date { get; set; }
        public Branding branding { get; set; } = null!;
        public long share_class_shares_outstanding { get; set; }
        public long weighted_shares_outstanding { get; set; }
        public int round_lot { get; set; }
    }

    public class Response
    {
        public string request_id { get; set; } = null!;
        public TickerDetailsDTO results { get; set; } = null!;
        public string status { get; set; } = null!;
    }

}
