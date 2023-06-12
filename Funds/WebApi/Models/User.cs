using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class User
    {
        [MaxLength(100)]
        [Required]
        public string Login { get; set; } = null!;

        [MaxLength(100)]
        public string Password { get; set; } = null!;

        [MaxLength(100)]
        public string RefreshToken { get; set; } = null!;

        public DateTime RefreshTokenExp { get; set; } = DateTime.Now.AddDays(5);
    }
}
