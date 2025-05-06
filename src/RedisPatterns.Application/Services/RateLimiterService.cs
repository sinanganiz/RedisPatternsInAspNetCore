using RedisPatterns.Application.Interfaces;
using StackExchange.Redis;

namespace RedisPatterns.Application.Services;

public class RateLimiterService : IRateLimiterService
{
    private readonly IDatabase _cache;

    public RateLimiterService(IConnectionMultiplexer redis)
    {
        _cache = redis.GetDatabase();
    }

    public async Task<bool> IsRequestAllowedAsync(string key, int limit, TimeSpan period)
    {
        var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var redisKey = $"rate_limit:{key}";

        var transaction = _cache.CreateTransaction();

        _ = transaction.SortedSetAddAsync(redisKey, now.ToString(), now);
        _ = transaction.SortedSetRemoveRangeByScoreAsync(redisKey, 0, now - (long)period.TotalSeconds);
        var countTask = transaction.SortedSetLengthAsync(redisKey);
        _ = transaction.KeyExpireAsync(redisKey, period);

        var committed = await transaction.ExecuteAsync();
        if (!committed)
            return false;

        var count = await countTask;
        return count <= limit;
    }

}