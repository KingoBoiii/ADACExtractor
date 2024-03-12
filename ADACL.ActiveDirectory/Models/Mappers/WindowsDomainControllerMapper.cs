using ADACExtractor.Models;
using System.Runtime.Versioning;

namespace ADACExtractor.ActiveDirectory.Models.Mappers;

public interface IDomainControllerMapper
{
    ValueTask<DomainController> MapAsync(ADDomainController? activeDirectoryDomainController);
}

[SupportedOSPlatform("windows")]
internal class WindowsDomainControllerMapper : IDomainControllerMapper
{
    public ValueTask<DomainController> MapAsync(ADDomainController? activeDirectoryDomainController)
    {
        ArgumentNullException.ThrowIfNull(activeDirectoryDomainController);

        return ValueTask.FromResult(new DomainController
        {
            Name = activeDirectoryDomainController.Name,
            SiteName = activeDirectoryDomainController.SiteName,
            IPAddress = activeDirectoryDomainController.IPAddress,
            OSVersion = activeDirectoryDomainController.OSVersion
        });
    }
}
