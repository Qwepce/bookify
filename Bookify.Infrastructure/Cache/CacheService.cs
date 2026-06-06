using System.Buffers;
using System.Text.Json;
using Bookify.Application.Cache;
using Microsoft.Extensions.Caching.Distributed;

namespace Bookify.Infrastructure.Cache;

internal sealed class CacheService( IDistributedCache cache ) : ICacheService
{
    public async Task<T?> GetAsync<T>(
        string cacheKey,
        CancellationToken cancellationToken )
    {
        byte[]? bytes = await cache.GetAsync( cacheKey, cancellationToken );

        return bytes is null ? default : Deserialize<T>( bytes );
    }

    public Task SetAsync<T>(
        string key, T value,
        TimeSpan? expiration = null,
        CancellationToken cancellationToken = default )
    {
        byte[] bytes = Serialize( value );

        return cache.SetAsync( key, bytes, CacheOptions.Create( expiration ), cancellationToken );
    }

    public Task RemoveAsync(
        string key,
        CancellationToken cancellationToken = default )
    {
        return cache.RemoveAsync( key, cancellationToken );
    }

    private static T Deserialize<T>( byte[] bytes )
    {
        return JsonSerializer.Deserialize<T>( bytes )!;
    }

    private static byte[] Serialize<T>( T value )
    {
        ArrayBufferWriter<byte> buffer = new();

        using Utf8JsonWriter writer = new( buffer );

        JsonSerializer.Serialize( writer, value );

        return buffer.WrittenSpan.ToArray();
    }
}