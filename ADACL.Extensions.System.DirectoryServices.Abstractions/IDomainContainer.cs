using System.DirectoryServices.Models;

namespace System.DirectoryServices;

public interface IDomainContainer : IDirectoryServiceContainer
{
    DomainInfo? Domain { get; }
}
