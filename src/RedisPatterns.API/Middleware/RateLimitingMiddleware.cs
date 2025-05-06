using RedisPatterns.Application.Interfaces;

namespace RedisPatterns.API.Middleware;

public class RateLimitingMiddleware
{
    private readonly RequestDelegate _next;

    public RateLimitingMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task InvokeAsync(HttpContext context, IRateLimiterService rateLimiter)
    {
        var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        var allowed = await rateLimiter.IsRequestAllowedAsync(ip, limit: 5, period: TimeSpan.FromMinutes(1));
        if (!allowed)
        {
            context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            await context.Response.WriteAsync("Too many requests. Please try again later.");
            return;
        }

        await _next(context);
    }
}