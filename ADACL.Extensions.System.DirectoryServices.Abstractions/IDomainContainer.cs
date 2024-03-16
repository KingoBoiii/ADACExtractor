using System.DirectoryServices.Models;

namespace System.DirectoryServices;

public interface IDomainContainer
{
    DomainInfo? Domain { get; }

    string? ErrorMessage { get; }
    bool IsValid { get; }
}
