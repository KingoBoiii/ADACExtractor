using ADACL.ActiveDirectory.Models.Mappers;
using ADACL.Models;
using System.Runtime.Versioning;

namespace ADACL.ActiveDirectory;

[SupportedOSPlatform("windows")]
public class WindowsDomainService : IDomainService
{
    public WindowsDomainService(IDomainMapper domainMapper)
    {
        DomainMapper = domainMapper;
    }

    private IDomainMapper DomainMapper { get; }

    public async ValueTask<Domain?> GetComputerDomainAsync()
    {
        if (TryGetComputerDomain(out var computerDomain, out var errorMessage))
        {
            return await DomainMapper.MapAsync(computerDomain).ConfigureAwait(false);
        }

        return default;
    }

    private static bool TryGetComputerDomain(out ADDomain computerDomain, out string? errorMessage)
    {
        try
        {
            computerDomain = ADDomain.GetComputerDomain();
            if (computerDomain is null)
            {
                errorMessage = "";
                return false;
            }

            errorMessage = default;
            return true;
        }
        catch (Exception ex)
        {
            computerDomain = default!;
            errorMessage = ex.Message;
            return false;
        }
    }
}
