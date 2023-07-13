namespace WebApi.Models
{
    public class RefreshToken
    {
        public string Token { get; set; } = null!;
        public DateTime Expiration { get; set; }
        public string UserLogin { get; set; } = null!;

        public string Session { get; set; } = null!;
        public virtual User User { get; set; } = null!; 
    }
}
