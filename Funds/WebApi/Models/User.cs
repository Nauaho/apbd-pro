using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class User
    {
        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; } = null!;
        [MaxLength(100)]
        [Required]
        public string Login { get; set; } = null!;

        [MaxLength(100)]
        public string Password { get; set; } = null!;

        [MaxLength(100)]
        public string RefreshToken { get; set; } = null!;

        public DateTime RefreshTokenExp { get; set; } = DateTime.Now.AddDays(5);

        public virtual ICollection<TickerDetails> TickersWatching { get; set;} = new List<TickerDetails>();
    }
}
