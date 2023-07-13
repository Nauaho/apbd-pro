using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Data
{
    public class ProContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<TickerDetails> TickerDetails { get; set; }
        public DbSet<TickerOHLC> TickerOHLC { get; set; }
        public DbSet<TickerOpenClose> TickerOpenClose { get; set; }
        public DbSet<TickerSimilar> TickerSimilar { get; set; }
        public DbSet<TickerUser> TickerUser { get; set; }
        public DbSet<RefreshToken> RefreshToken { get; set; }


        public ProContext(DbContextOptions options) : base(options)
        {
        }

        protected ProContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Login).HasName("User_pk");
                entity.Property(e => e.Email).HasMaxLength(255).IsRequired();
                entity.Property(e => e.Login).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Password).HasMaxLength(100).IsRequired();
            });

            modelBuilder.Entity<TickerDetails>(e => 
            {
                e.HasKey(e => e.Ticker).HasName("Ticker_pk");
                e.Property( e => e.Ticker).HasMaxLength(100).IsRequired();
                e.Property(e => e.Name).HasMaxLength(200);
                e.Property(e => e.Market).HasMaxLength(200);
                e.Property(e => e.Locale).HasMaxLength(200);
                e.Property(e => e.PrimaryExchange).HasMaxLength(200);
                e.Property(e => e.Type).HasMaxLength(200);
                e.Property(e => e.Active);
                e.Property(e => e.CurrencyName).HasMaxLength(200);
                e.Property(e => e.Cik).HasMaxLength(200);
                e.Property(e => e.CompositeFigi).HasMaxLength(200);
                e.Property(e => e.ShareClassFigi).HasMaxLength(200);
                e.Property(e => e.PhoneNumber).HasMaxLength(200);
                e.Property(e => e.Address).HasMaxLength(500);
                e.Property(e => e.City).HasMaxLength(200);
                e.Property(e => e.State).HasMaxLength(200);
                e.Property(e => e.PostalCode).HasMaxLength(200);
                e.Property(e => e.Description);
                e.Property(e => e.SicCode).HasMaxLength(200);
                e.Property(e => e.SicDescription);
                e.Property(e => e.TickerRoot).HasMaxLength(200);
                e.Property(e => e.HomepageUrl).HasMaxLength(200);
                e.Property(e => e.TotalEmployees);
                e.Property(e => e.ListDate);
                e.Property(e => e.LogoUrl);
                e.Property(e => e.IconUrl);
                e.Property(e => e.ShareClassSharesOutstanding);
                e.Property(e => e.WeightedSharesOutstanding);
                e.Property(e => e.RoundLot);
            });

            modelBuilder.Entity<TickerSimilar>(e =>
            {
                e.HasKey(e => new { e.TickerOneId, e.TickerTwoId }).HasName("Ticker_Similar_pk");
                e.Property(e => e.TickerOneId);
                e.Property(e => e.TickerTwoId);
                e.HasOne(e => e.TickerOne).WithMany(e => e.Similar).HasForeignKey(e=> e.TickerOneId);
                e.HasOne(e => e.TickerTwo).WithMany(e => e.SimilarTo).HasForeignKey(e => e.TickerTwoId).OnDelete(DeleteBehavior.Restrict); ;
            });

            modelBuilder.Entity<TickerOHLC>(e => 
            {
                e.HasKey(e => new {e.Symbol, e.Timespan, e.Multuplier, e.T}).HasName("Ohlc_pk");
                e.Property(e => e.Symbol).HasMaxLength(100).IsRequired();
                e.Property(e => e.C).IsRequired();
                e.Property(e => e.H).IsRequired();
                e.Property(e => e.L).IsRequired();
                e.Property(e => e.N).IsRequired();
                e.Property(e => e.O).IsRequired();
                e.Property(e => e.T).IsRequired();
                e.Property(e => e.V).IsRequired();
                e.Property(e => e.Vw).IsRequired();
                e.Property(e => e.Multuplier).IsRequired();
                e.Property(e => e.Timespan).IsRequired();
                e.Property(e => e.Symbol).IsRequired();
                e.HasOne(a => a.Ticker).WithMany(b => b.TickerOHLCs).HasForeignKey(d => d.Symbol);
            });

            modelBuilder.Entity<TickerOpenClose>(e => 
            {
                e.HasKey( e => new { e.Symbol, e.From }).HasName("TockerOpenClose_pk");
                e.HasOne(e => e.Ticker).WithMany(e => e.TickerOpenCloses).HasForeignKey( e=>e.Symbol);
            });

            modelBuilder.Entity<TickerUser>(e =>
            {
                e.HasKey( e => new { e.TickerSymbol, e.UserLogin}).HasName("Ticke_rUser_pk");
                e.Property( e=> e.UserLogin).HasMaxLength(100).IsRequired();
                e.Property(e => e.TickerSymbol).HasMaxLength(100).IsRequired();
                e.HasOne(e => e.User).WithMany(e => e.TickersWatching).HasForeignKey(e => e.UserLogin);
                e.HasOne(e => e.Ticker).WithMany(e => e.UsersWatching).HasForeignKey(e => e.TickerSymbol);
            });

            modelBuilder.Entity<RefreshToken>(e =>
            {
                e.HasKey(e => new { e.UserLogin, e.Session }).HasName("RefreshToken_pk");
                e.Property(e => e.UserLogin ).HasMaxLength(100).IsRequired();
                e.Property(e => e.Session).HasMaxLength(100).IsRequired();
                e.Property(e => e.Token ).HasMaxLength(500).IsRequired();
                e.Property(e => e.Expiration ).IsRequired();
                e.HasOne( e => e.User ).WithMany( e => e.RefreshTokens).HasForeignKey(e => e.UserLogin);
            });
        }
    }
}
