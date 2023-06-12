using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Data
{
    public class ProContext : DbContext
    {
        public DbSet<User> Users { get; set; }
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
                entity.Property(e => e.Login).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Password).HasMaxLength(100).IsRequired();
            });
        }
    }
}
