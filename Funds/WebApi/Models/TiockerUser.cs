﻿namespace WebApi.Models
{
    public class TiockerUser
    {
        public string UserEmail { get; set; } = null!;
        public string TickerSymbol { get; set; } = null!;

        public virtual User User { get; set;} = null!;
        public virtual TickerDetails Ticker { get; set; } = null!;
    }
}
