using System.DirectoryServices.Models;

namespace System.DirectoryServices;

public interface IDomainControllerContainer : IDirectoryServiceContainer
{
    IEnumerable<DomainControllerInfo> DomainControllers { get; }
}
