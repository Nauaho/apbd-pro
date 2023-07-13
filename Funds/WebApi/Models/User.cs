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
        public virtual ICollection<TickerUser> TickersWatching { get; set;} = new List<TickerUser>();
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
