using Boxes.Application.Contracts.Interfaces;
using Boxes.Application.Models;
using Microsoft.Extensions.Caching.Memory;

namespace Boxes.Infrastructure.Services;

public class CachedWorkshopService : IWorkshopService
{
    private readonly IWorkshopService _inner;
    private readonly IMemoryCache _cache;
    private const string CacheKey = "active_workshops";
    private static readonly TimeSpan CacheExpiration = TimeSpan.FromMinutes(5);

    public CachedWorkshopService(IWorkshopService inner, IMemoryCache cache)
    {
        _inner = inner;
        _cache = cache;
    }

    public async Task<IEnumerable<Workshop>> GetActiveWorkshopsAsync(CancellationToken cancellationToken = default)
    {
        if (_cache.TryGetValue(CacheKey, out IEnumerable<Workshop>? cachedWorkshops) && cachedWorkshops != null)
        {
            return cachedWorkshops;
        }

        var workshops = await _inner.GetActiveWorkshopsAsync(cancellationToken);
        var workshopsList = workshops.ToList();

        // Guardar en caché
        _cache.Set(CacheKey, workshopsList, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = CacheExpiration
        });

        return workshopsList;
    }

    public async Task<Workshop?> GetWorkshopByIdAsync(int placeId, CancellationToken cancellationToken = default)
    {
        var workshops = await GetActiveWorkshopsAsync(cancellationToken);
        return workshops.FirstOrDefault(w => w.Id == placeId);
    }

    public void InvalidateCache()
    {
        _cache.Remove(CacheKey);
    }
}