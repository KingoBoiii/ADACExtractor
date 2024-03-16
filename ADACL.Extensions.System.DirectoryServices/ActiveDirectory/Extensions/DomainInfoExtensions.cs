using System.DirectoryServices.Models;
using System.Runtime.Versioning;

namespace System.DirectoryServices.ActiveDirectory.Extensions;

internal static class DomainInfoExtensions
{
    [SupportedOSPlatform("windows")]
    internal static Domain? GetNativeDomain(this DomainInfo domainInfo, DirectoryContextType directoryContextType = DirectoryContextType.Domain)
    {
        ArgumentNullException.ThrowIfNull(domainInfo);

        var directoryContext = new DirectoryContext(directoryContextType, domainInfo.Name ?? string.Empty);

        try
        {
            var domain = Domain.GetDomain(directoryContext) ?? throw new ActiveDirectoryObjectNotFoundException();
            return domain;
        }
        catch
        {
            return default;
        }
    }
}
