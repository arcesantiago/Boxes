using Boxes.Application.Contracts.Infrastructure;
using Boxes.Application.Contracts.Interfaces;
using Boxes.Application.Contracts.Persistence;
using Boxes.Application.Contracts.Persistence.Read;
using Boxes.Application.Contracts.Persistence.Write;
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

        services.AddSingleton(typeof(IReadRepository<>), typeof(InMemoryRepositoryBase<>));
        services.AddSingleton(typeof(IWriteRepository<>), typeof(InMemoryRepositoryBase<>));
        services.AddSingleton<IAppointmentRepository, InMemoryAppointmentRepository>();

        services.AddSingleton<IAppointmentUnitOfWork, InMemoryAppointmentUnitOfWork>();

        services.AddSingleton<ICacheService, MemoryCacheService>();

        services.AddHttpClient<WorkshopService>();

        services.AddSingleton<IWorkshopService>(serviceProvider =>
        {
            var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
            var httpClient = httpClientFactory.CreateClient();
            var mapper = serviceProvider.GetRequiredService<AutoMapper.IMapper>();
            var inner = new WorkshopService(httpClient, mapper);
            var cache = serviceProvider.GetRequiredService<IMemoryCache>();
            return new CachedWorkshopService(inner, cache);
        });



        return services;
    }
}