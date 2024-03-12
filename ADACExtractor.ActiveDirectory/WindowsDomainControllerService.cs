using ADACExtractor.ActiveDirectory.Models.Mappers;
using ADACExtractor.Models;
using Microsoft.Extensions.Logging;
using System.Runtime.Versioning;

namespace ADACExtractor.ActiveDirectory;

[SupportedOSPlatform("windows")]
public class WindowsDomainControllerService(ILogger<WindowsDomainControllerService> logger, IDomainControllerMapper domainControllerMapper) : IDomainControllerService
{
    public async ValueTask<IEnumerable<DomainController>> GetDomainControllersAsync(Domain? domain)
    {
        ArgumentNullException.ThrowIfNull(domain);

        var adDomain = ADDomain.GetDomain(new System.DirectoryServices.ActiveDirectory.DirectoryContext(System.DirectoryServices.ActiveDirectory.DirectoryContextType.Domain, domain.Name ?? string.Empty));
        if (adDomain is null)
        {
            logger.LogError("Cannot find Domain '{}'", domain.Name);

            return Enumerable.Empty<DomainController>();
        }

        var adDomainControllers = adDomain.DomainControllers;
        if (adDomainControllers.Count == 0)
        {
            logger.LogError("Cannot find Domain Controller for domain '{}'", domain.Name);

            return Enumerable.Empty<DomainController>();
        }

        var domainControllers = new DomainController[adDomainControllers.Count];
        for (int i = 0; i < domainControllers.Length; i++)
        {
            domainControllers[i] = await domainControllerMapper.MapAsync(adDomainControllers[i]);
        }

        return domainControllers;
    }
}
