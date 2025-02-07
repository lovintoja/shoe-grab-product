using Microsoft.EntityFrameworkCore;
using ShoeGrabCommonModels;

namespace ShoeGrabProductManagement.Contexts;

public class ProductContext : DbContext
{
    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<Basket> Baskets { get; set; }
    public DbSet<BasketItem> BasketItems { get; set; }

    public ProductContext(DbContextOptions<ProductContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Basket>()
            .HasKey(b => b.Id);

        modelBuilder.Entity<Basket>()
            .HasIndex(b => b.UserId)
            .IsUnique();

        modelBuilder.Entity<Basket>()
            .HasMany(b => b.Items) 
            .WithOne(bi => bi.Basket) 
            .HasForeignKey(bi => bi.BasketId); 

        modelBuilder.Entity<BasketItem>()
            .HasKey(bi => bi.Id); 

        modelBuilder.Entity<BasketItem>()
            .HasOne(bi => bi.Product)
            .WithMany()
            .HasForeignKey(bi => bi.ProductId);


        modelBuilder.Entity<BasketItem>()
            .Property(bi => bi.Quantity)
            .HasDefaultValue(1)
            .IsRequired();
    }
}
