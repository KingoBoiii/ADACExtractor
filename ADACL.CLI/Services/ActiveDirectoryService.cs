using Microsoft.Extensions.Logging;
using System.DirectoryServices;

namespace ADACL.CLI.Services;

public interface IActiveDirectoryService
{
    ValueTask PrintActiveDirectoryInfoAsync();
}

internal sealed class ActiveDirectoryService(ILogger<ActiveDirectoryService> logger, IDomainProvider domainProvider, IDomainControllerProvider domainControllerProvider) : IActiveDirectoryService
{
    public async ValueTask PrintActiveDirectoryInfoAsync()
    {
        var domainContainer = await domainProvider.GetDomainAsync().ConfigureAwait(false);
        if (!domainContainer.IsValid)
        {
            logger.LogError(domainContainer.ErrorMessage);
            return;
        }

        logger.LogInformation("Found domain {}", domainContainer?.Domain?.Name);

        var domainControllerContainer = await domainControllerProvider.GetDomainControllersAsync(domainContainer).ConfigureAwait(false);
        if (!domainControllerContainer.IsValid)
        {
            logger.LogError(domainControllerContainer.ErrorMessage);
            return;
        }

        logger.LogInformation(@"""===
Domain:     {}
DomainControllers:
    {}
===""", domainContainer?.Domain?.Name, domainControllerContainer?.DomainControllers?.SelectMany(dc => $" - {dc.Name}"));
    }
}
