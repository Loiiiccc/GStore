using GStore.Models;
using Microsoft.EntityFrameworkCore;

namespace GStore.Data
{
    public class GStoreDbContext(DbContextOptions<GStoreDbContext> options) : DbContext(options)
    {
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<User> Users { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Category>()
            //    .HasMany(c => c.Products);

            //modelBuilder.Entity<Product>()
            //    .Property(p => p.Price).HasPrecision(14, 2);
        }
    };

}
