using Microsoft.EntityFrameworkCore;
using ShoeGrabCommonModels;

namespace ShoeGrabProductManagement.Contexts;

public class ProductContext : DbContext
{
    public virtual DbSet<Product> Products { get; set; }

    public ProductContext(DbContextOptions<ProductContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
