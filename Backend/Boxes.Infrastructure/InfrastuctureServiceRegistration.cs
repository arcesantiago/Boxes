using Boxes.Application.Contracts.Infrastructure;
using Boxes.Application.Contracts.Interfaces;
using Boxes.Application.Contracts.Persistence;
using Boxes.Infrastructure.Cache;
using Boxes.Infrastructure.Repositories;
using Boxes.Infrastructure.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace Boxes.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddMemoryCache();

        services.AddSingleton<InMemoryAppointmentRepository>();

        services.AddScoped<IAppointmentUnitOfWork, InMemoryAppointmentUnitOfWork>();

        services.AddSingleton<ICacheService, MemoryCacheService>();

        services.AddHttpClient<WorkshopService>();

        services.AddSingleton<IWorkshopService>(serviceProvider =>
        {
            var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
            var httpClient = httpClientFactory.CreateClient();
            var inner = new WorkshopService(httpClient);
            var cache = serviceProvider.GetRequiredService<IMemoryCache>();
            return new CachedWorkshopService(inner, cache);
        });

        return services;
    }
}