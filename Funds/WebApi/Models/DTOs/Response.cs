using System;
using System.Collections.Generic;

namespace WebApi.Models
{

    public class AddressDTO
    {
        public string? address1 { get; set; }
        public string? city { get; set; }
        public string? state { get; set; }
        public string? postal_code { get; set; }
    }

    public class BrandingDTO
    {
        public string? logo_url { get; set; }
        public string? icon_url { get; set; }
    }

    public class TickerDetailsDTO
    {
        public string ticker { get; set; } = null!;
        public string? name { get; set; }
        public string? market { get; set; }
        public string? locale { get; set; }
        public string? primary_exchange { get; set; }
        public string? type { get; set; }
        public bool active { get; set; }
        public string? currency_name { get; set; }
        public string? cik { get; set; }
        public string? composite_figi { get; set; }
        public string? share_class_figi { get; set; }
        public string? phone_number { get; set; }
        public AddressDTO? address { get; set; }
        public string? description { get; set; }
        public string? sic_code { get; set; }
        public string? sic_description { get; set; }
        public string? ticker_root { get; set; }
        public string? homepage_url { get; set; }
        public long total_employees { get; set; }
        public DateTime list_date { get; set; }
        public BrandingDTO? branding { get; set; }
        public long share_class_shares_outstanding { get; set; }
        public long weighted_shares_outstanding { get; set; }
        public int round_lot { get; set; }
    }

    public class Response
    {
        public string? request_id { get; set; }
        public TickerDetailsDTO results { get; set; } = null!;
        public string? status { get; set; }
    }


}
