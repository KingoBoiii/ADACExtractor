using System.DirectoryServices.Models;

namespace System.DirectoryServices.ActiveDirectory;

internal class WindowsActiveDirectoryDomainContainer : IActiveDirectoryDomainContainer
{
    public WindowsActiveDirectoryDomainContainer(string errorMessage)
    {
        Domain = default;
        ErrorMessage = errorMessage;
    }

    public WindowsActiveDirectoryDomainContainer(DomainInfo domain)
    {
        Domain = domain;
        ErrorMessage = default;
    }

    public DomainInfo? Domain { get; set; }
    public bool IsValid => Domain is not null;
    public string? ErrorMessage { get; set; }

    public static IActiveDirectoryDomainContainer Success(DomainInfo domain)
    {
        return new WindowsActiveDirectoryDomainContainer(domain);
    }

    public static IActiveDirectoryDomainContainer Error(string errorMessage)
    {
        return new WindowsActiveDirectoryDomainContainer(errorMessage);
    }
}
