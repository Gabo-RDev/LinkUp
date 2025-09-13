using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace LinkUp.Application.Utils;

public static class DistributedCacheExtensions
{
    public static DistributedCacheEntryOptions DefaultExpiration => new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(4)
    };

    public static async Task<T> GetOrCreateAsync<T>(
        this IDistributedCache cache,
        string key,
        Func<Task<T>> factory,
        DistributedCacheEntryOptions options = null!,
        CancellationToken cancellationToken = default
    )
    {
        var cachedData = await cache.GetStringAsync(key, cancellationToken);

        if (cachedData is not null)
        {
            Console.WriteLine($" Cache HIT for key: {key}");
            return JsonSerializer.Deserialize<T>(cachedData)!;
        }

        Console.WriteLine($" Cache MISS for key: {key}");

        var data = await factory();

        await cache.SetStringAsync(
            key,
            JsonSerializer.Serialize(data),
            options ?? DefaultExpiration,
            cancellationToken);

        Console.WriteLine($" Cached data under key: {key}");

        return data;
    }
}