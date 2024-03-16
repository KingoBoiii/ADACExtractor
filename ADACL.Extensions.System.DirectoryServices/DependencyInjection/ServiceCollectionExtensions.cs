using Microsoft.Extensions.DependencyInjection;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Runtime.Versioning;

namespace ADACL.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    [SupportedOSPlatform("windows")]
    public static IServiceCollection AddActiveDirectoryServices(this IServiceCollection services)
    {
        services.AddScoped<IDomainProvider, WindowsDomainProvider>();
        services.AddScoped<IDomainControllerProvider, WindowsDomainControllerProvider>();

        return services;
    }

    public static IServiceCollection AddLdapDirectoryServices(this IServiceCollection services)
    {
        throw new NotImplementedException();
    }
}
