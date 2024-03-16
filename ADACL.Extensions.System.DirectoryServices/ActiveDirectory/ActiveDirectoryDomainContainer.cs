using System.DirectoryServices.Models;

namespace System.DirectoryServices.ActiveDirectory;

internal class ActiveDirectoryDomainContainer : IDomainContainer
{
    public ActiveDirectoryDomainContainer(string errorMessage)
    {
        Domain = default;
        ErrorMessage = errorMessage;
    }

    public ActiveDirectoryDomainContainer(DomainInfo domain)
    {
        Domain = domain;
        ErrorMessage = default;
    }

    public DomainInfo? Domain { get; set; }
    public bool IsValid => Domain is not null;
    public string? ErrorMessage { get; set; }

    public static IDomainContainer Success(DomainInfo domain)
    {
        return new ActiveDirectoryDomainContainer(domain);
    }

    public static IDomainContainer Error(string errorMessage)
    {
        return new ActiveDirectoryDomainContainer(errorMessage);
    }
}

