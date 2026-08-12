using DiscoverGeorgia.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DiscoverGeorgia.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Place> Places { get; set; }
        public DbSet<Favorite> Favorites { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. User-ის კონფიგურაცია
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.Property(e => e.Email).HasColumnName("Email_UQ");
                entity.Ignore(e => e.ProfileImage);
            });

            // 2. Place-ის კონფიგურაცია
            modelBuilder.Entity<Place>(entity =>
            {
                entity.ToTable("Places");
                entity.Ignore(e => e.CityId);
                entity.Ignore(e => e.RegionId);
            });

            // 3. Favorite-ის კონფიგურაცია
            modelBuilder.Entity<Favorite>(entity =>
            {
                entity.ToTable("Favorites");
            });
        }
    }
}