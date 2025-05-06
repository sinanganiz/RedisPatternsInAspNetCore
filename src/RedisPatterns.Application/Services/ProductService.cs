using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using RedisPatterns.Application.Interfaces;
using RedisPatterns.Domain.Entities;
using RedisPatterns.Infrastructure.Data;
using StackExchange.Redis;

namespace RedisPatterns.Application.Services;

public class ProductService : IProductService
{
    private readonly AppDbContext _context;
    private readonly IDatabase _cache;
    private const string CacheKey = "products";

    public ProductService(AppDbContext context, IConnectionMultiplexer redis)
    {
        _context = context;
        _cache = redis.GetDatabase();
    }

    public async Task<List<Product>> GetAllProductsAsync()
    {
        var cached = await _cache.StringGetAsync(CacheKey);
        if (!string.IsNullOrEmpty(cached))
        {
            return JsonSerializer.Deserialize<List<Product>>(cached)!;
        }

        var products = await _context.Products.ToListAsync();
        var serialized = JsonSerializer.Serialize(products);
        await _cache.StringSetAsync(CacheKey, serialized, TimeSpan.FromMinutes(5));

        return products;
    }
}