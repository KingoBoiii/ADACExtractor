using ADACL.ActiveDirectory.Models.Mappers;
using ADACL.Models;
using Microsoft.Extensions.Logging;
using System.DirectoryServices;
using System.Runtime.Versioning;

namespace ADACL.ActiveDirectory;

[SupportedOSPlatform("windows")]
public class WindowsDomainService(ILogger<WindowsDomainService> logger, IDomainProvider domainProvider, IDomainMapper domainMapper) : IDomainService
{
    public async ValueTask<Domain?> GetComputerDomainAsync()
    {
        var domainContainer = await domainProvider.GetDomainAsync();
        if (!domainContainer.IsValid)
        {
            logger.LogError(message: domainContainer.ErrorMessage);
            return default;
        }

        if (TryGetComputerDomain(out var computerDomain, out var errorMessage))
        {
            return await domainMapper.MapAsync(computerDomain).ConfigureAwait(false);
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
