using Microsoft.EntityFrameworkCore;
using RedisPatterns.Domain.Entities;

namespace RedisPatterns.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<Product> Products => Set<Product>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Mechanical Keyboard", Price = 1200, Stock = 15 },
            new Product { Id = 2, Name = "Wireless Mouse", Price = 500, Stock = 30 },
            new Product { Id = 3, Name = "Monitor 27 inch", Price = 3500, Stock = 10 },
            new Product { Id = 4, Name = "Speaker", Price = 750, Stock = 12 },
            new Product { Id = 5, Name = "Microphone", Price = 300, Stock = 5 }
        );
    }

}