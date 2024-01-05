using Microsoft.Extensions.DependencyInjection;
using UrlShortener.Business.Interfaces;

namespace UrlShortener.Business;

public static class BusinessServiceRegistration
{
    public static IServiceCollection AddBusinessServices(this IServiceCollection services)
    {
        services.AddScoped<ILinkService, LinkService>();

        return services;
    }
}