namespace WebApi.Models
{
    public class TickerSimilar
    {
        public string TickerOneId { get; set; } = null!;
        public string TickerTwoId { get; set; } = null!;

        public virtual TickerDetails TickerOne { get; set; } = null!;
        public virtual TickerDetails TickerTwo { get; set; } = null!;
    }
}
