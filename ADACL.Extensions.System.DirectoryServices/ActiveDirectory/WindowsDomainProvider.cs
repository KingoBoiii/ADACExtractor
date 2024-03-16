using System.DirectoryServices.Models;
using System.Runtime.Versioning;

namespace System.DirectoryServices.ActiveDirectory;

[SupportedOSPlatform("windows")]
internal class WindowsDomainProvider : IDomainProvider
{
    public ValueTask<IDomainContainer> GetDomainAsync()
    {
        try
        {
            var computerDomain = Domain.GetComputerDomain() ?? throw new ActiveDirectoryObjectNotFoundException();

            var domainInfo = new DomainInfo
            {
                Name = computerDomain.Name
            };

            return ValueTask.FromResult<IDomainContainer>(ActiveDirectoryDomainContainer.Success(domainInfo));
        }
        catch (Exception ex)
        {
            return ValueTask.FromResult<IDomainContainer>(ActiveDirectoryDomainContainer.Error(ex.Message));
        }
    }
}
