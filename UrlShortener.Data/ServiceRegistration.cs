using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UrlShortener.Data.Interfaces;

namespace UrlShortener.Data;

public static class DataServiceRegistration
{
    public static IServiceCollection AddDataServices(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ILinkRepository, LinkRepository>();
        services.AddScoped<IClickRepository, ClickRepository>();

        services.AddDbContext<UrlShortenerDbContext>(
            o => o.UseInMemoryDatabase("UrlShortenerDb"));
        
        return services;
    }
}