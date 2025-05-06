using Microsoft.EntityFrameworkCore;
using RedisPatterns.Application.Interfaces;
using RedisPatterns.Infrastructure.Data;
using StackExchange.Redis;

namespace RedisPatterns.Application.Services;

public class ProductPurchaseService : IProductPurchaseService
{
    private readonly AppDbContext _context;
    private readonly IDatabase _cache;
    private const string LockKeyPrefix = "lock_product_";

    public ProductPurchaseService(AppDbContext context, IConnectionMultiplexer redis)
    {
        _context = context;
        _cache = redis.GetDatabase();
    }

    public async Task<bool> PurchaseProductAsync(int productId, int quantity)
    {
        var lockKey = $"{LockKeyPrefix}{productId}";
        var lockToken = Guid.NewGuid().ToString();
        var acquired = await _cache.LockTakeAsync(lockKey, lockToken, TimeSpan.FromSeconds(5));

        if (!acquired)
            return false;

        try
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);
            if (product is null || product.Stock < quantity)
                return false;

            product.Stock -= quantity;
            await _context.SaveChangesAsync();
            await _cache.KeyDeleteAsync("products"); // cache invalidate
            return true;
        }
        finally
        {
            await _cache.LockReleaseAsync(lockKey, lockToken);
        }
    }

}