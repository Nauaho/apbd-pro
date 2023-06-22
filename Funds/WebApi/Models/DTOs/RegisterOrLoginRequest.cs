using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.DTOs
{
    public class RegisterOrLoginRequest
    {
        [EmailAddress]
        [MaxLength(255)]
        public string? Email { get; set; }
        [MaxLength(100)]
        [Required]
        public string Login { get; set; } = null!;

        [MaxLength(100)]
        public string Password { get; set; } = null!;
    }
}
