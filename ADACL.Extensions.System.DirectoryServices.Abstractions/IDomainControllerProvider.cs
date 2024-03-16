using System.DirectoryServices.Models;

namespace System.DirectoryServices;

public interface IDomainControllerProvider
{
    ValueTask<IDomainControllerContainer> GetDomainControllersAsync(IDomainContainer? domainContainer);
    ValueTask<IDomainControllerContainer> GetDomainControllersAsync(DomainInfo? domain);
}