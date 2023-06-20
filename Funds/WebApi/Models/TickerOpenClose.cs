using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class TickerOpenClose
    {
        [Required]
        public float AfterHours { get; set; }
        [Required]
        public float Close { get; set; }
        [Required]
        public float High { get; set; }
        [Required]
        public float Low { get; set; }
        [Required]
        public float Open { get; set; }
        [Required]
        public float PreMarket { get; set; }
        [Required]
        public long Volume { get; set; }

        //Primary Key Start
        [Required]
        public DateTime From { get; set; }
        [Required]
        [MaxLength(100)]
        public string Symbol = null!;
        //Primary Key End

        //Connected Values
        public virtual TickerDetails Ticker { get; set; } = null!;
        
    }
}
