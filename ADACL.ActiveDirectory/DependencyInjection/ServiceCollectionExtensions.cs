using ADACL.ActiveDirectory;
using ADACL.ActiveDirectory.Models.Mappers;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.Versioning;

namespace ADACL.Extensions.DependencyInjection;

[SupportedOSPlatform("windows")]
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWindowsActiveDirectory(this IServiceCollection services)
    {
        services.AddSingleton<IDomainMapper, WindowsDomainMapper>();
        services.AddSingleton<IDomainControllerMapper, WindowsDomainControllerMapper>();

        services.AddTransient<IDomainService, WindowsDomainService>();
        services.AddTransient<IDomainControllerService, WindowsDomainControllerService>();

        return services;
    }
}
