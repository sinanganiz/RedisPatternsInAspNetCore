namespace RedisPatterns.Application.Interfaces;

public interface IRateLimiterService
{
    Task<bool> IsRequestAllowedAsync(string key, int limit, TimeSpan period);
}