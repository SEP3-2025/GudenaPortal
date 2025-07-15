using Gudena.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gudena.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<AccountDetails> AccountDetails { get; set; }
    public DbSet<Basket> Baskets { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Media> Media { get; set; }
    public DbSet<Favourite> Favourites { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Shipping> Shippings { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<WarrantyClaim> WarrantyClaims { get; set; }
    public DbSet<ProductReturn> ProductReturns { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Add custom configurations here if needed.
    }
}