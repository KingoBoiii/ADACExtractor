using System.DirectoryServices.Models;

namespace System.DirectoryServices.ActiveDirectory;

internal class ActiveDirectoryDomainControllerContainer : IDomainControllerContainer
{
    public ActiveDirectoryDomainControllerContainer(string errorMessage)
    {
        DomainControllers = Enumerable.Empty<DomainControllerInfo>();
        ErrorMessage = errorMessage;
    }

    public ActiveDirectoryDomainControllerContainer(IEnumerable<DomainControllerInfo> domainControllers)
    {
        DomainControllers = domainControllers;
        ErrorMessage = default;
    }

    public IEnumerable<DomainControllerInfo> DomainControllers { get; set; }
    public bool IsValid => DomainControllers is not null && DomainControllers.Any();
    public string? ErrorMessage { get; set; }


    public static IDomainControllerContainer Success(IEnumerable<DomainControllerInfo> domainControllers)
    {
        return new ActiveDirectoryDomainControllerContainer(domainControllers);
    }

    public static IDomainControllerContainer Error(string errorMessage)
    {
        return new ActiveDirectoryDomainControllerContainer(errorMessage);
    }
}

