using Gudena.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Gudena.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
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
    public DbSet<BasketItem> BasketItems { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ApplicationUser to AccountDetails (one-to-one)
        modelBuilder.Entity<ApplicationUser>()
            .HasOne(u => u.AccountDetails)
            .WithOne(ad => ad.ApplicationUser)
            .HasForeignKey<AccountDetails>(ad => ad.ApplicationUserId)
            .OnDelete(DeleteBehavior.Cascade);

        // ApplicationUser to Basket (one-to-one)
        modelBuilder.Entity<Basket>()
            .HasOne(b => b.ApplicationUser)
            .WithOne(u => u.Basket)
            .HasForeignKey<Basket>(b => b.ApplicationUserId);

        // ApplicationUser to Order (one-to-many)
        modelBuilder.Entity<Order>()
            .HasOne(o => o.ApplicationUser)
            .WithMany(u => u.Orders)
            .HasForeignKey(o => o.ApplicationUserId);

        // ApplicationUser to Favourite (one-to-many)
        modelBuilder.Entity<Favourite>()
            .HasOne(f => f.ApplicationUser)
            .WithMany(u => u.Favourites)
            .HasForeignKey(f => f.ApplicationUserId);

        // ApplicationUser to Product (one-to-many, owner relationship)
        modelBuilder.Entity<Product>()
            .HasOne(p => p.Owner)
            .WithMany(u => u.Products)
            .HasForeignKey(p => p.OwnerId);

        // Email uniqueness constraint
        modelBuilder.Entity<ApplicationUser>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // Basket Item to Product (one to many)
        modelBuilder.Entity<BasketItem>()
            .HasOne(bi => bi.Product)
            .WithMany()
            .HasForeignKey(bi => bi.ProductId);

        // Basket Item to Basket (one to many)
        modelBuilder.Entity<BasketItem>()
            .HasOne(bi => bi.Basket)
            .WithMany(b => b.BasketItems)
            .HasForeignKey(bi => bi.BasketId);

        // Category inheritance (one to many)
        modelBuilder.Entity<Category>()
            .HasOne(c => c.ParentCategory)
            .WithMany(c => c.Children)
            .IsRequired(false);

        // Shipping to OrderItems (M:M)
        modelBuilder.Entity<OrderItem>()
            .HasMany(oi => oi.Shippings)
            .WithMany(s => s.OrderItems)
            .UsingEntity(j => j.ToTable("OrderItemShippings"));

        // OrderItem to WarrantyClaim (optional 1:1)
        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.WarrantyClaim)
            .WithOne(wc => wc.OrderItem)
            .HasForeignKey<WarrantyClaim>(wc => wc.OrderItemId)
            .OnDelete(DeleteBehavior.Cascade);

        // OrderItem to ProductReturn (optional 1:1)
        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.ProductReturn)
            .WithOne(pr => pr.OrderItem)
            .HasForeignKey<ProductReturn>(pr => pr.OrderItemId)
            .OnDelete(DeleteBehavior.Cascade);

        //Order to Payment (1:1)
        modelBuilder.Entity<Order>()
            .HasMany(o => o.Payments)
            .WithOne(p => p.Order)
            .HasForeignKey(p => p.OrderId)
            .OnDelete(DeleteBehavior.SetNull);

        // Order to Shipping (1:M)
        modelBuilder.Entity<Order>()
            .HasMany(o => o.Shippings)
            .WithOne(s => s.Order)
            .HasForeignKey(s => s.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}