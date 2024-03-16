using ADACL.CLI.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.InteropServices;

namespace ADACL.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddADACL(this IServiceCollection services)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            services.AddActiveDirectoryServices();
        }

        services.AddScoped<ISystemInfoService, SystemInfoService>();
        services.AddScoped<IActiveDirectoryService, ActiveDirectoryService>();

        return services;
    }
}
