using System.DirectoryServices.ActiveDirectory.Extensions;
using System.DirectoryServices.Models;
using System.Runtime.Versioning;

namespace System.DirectoryServices.ActiveDirectory;

[SupportedOSPlatform("windows")]
internal sealed class WindowsDomainControllerProvider : IDomainControllerProvider
{
    public ValueTask<IDomainControllerContainer> GetDomainControllersAsync(IDomainContainer? domainContainer)
    {
        ArgumentNullException.ThrowIfNull(domainContainer);

        return GetDomainControllersAsync(domainContainer.Domain);
    }

    public ValueTask<IDomainControllerContainer> GetDomainControllersAsync(DomainInfo? domain)
    {
        ArgumentNullException.ThrowIfNull(domain);

        var nativeDomain = domain.GetNativeDomain();
        if (nativeDomain is null)
        {
            return ValueTask.FromResult(ActiveDirectoryDomainControllerContainer.Error($"Failed to get native Domain '{domain.Name}'."));
        }

        var domainControllers = nativeDomain.DomainControllers ?? nativeDomain.FindAllDomainControllers(nativeDomain.Name) ?? nativeDomain.FindAllDiscoverableDomainControllers(nativeDomain.Name);
        if (domainControllers.Count == 0)
        {
            return ValueTask.FromResult(ActiveDirectoryDomainControllerContainer.Error($"No domain controllers for domain '{domain.Name}'."));
        }

        var domainControllerInfos = Enumerable.Empty<DomainControllerInfo>();
        foreach (var controller in domainControllers)
        {
            if (controller is not DomainController domainController)
            {
                continue;
            }

            domainControllerInfos = domainControllerInfos.Append(new DomainControllerInfo
            {
                Name = domainController.Name,
                SiteName = domainController.SiteName,
                IPAddress = domainController.IPAddress,
                OSVersion = domainController.OSVersion
            });
        }

        return ValueTask.FromResult(ActiveDirectoryDomainControllerContainer.Success(domainControllerInfos));
    }
}