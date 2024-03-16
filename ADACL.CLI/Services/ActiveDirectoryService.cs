using Microsoft.Extensions.Logging;
using System.DirectoryServices;
using System.Text;

namespace ADACL.CLI.Services;

public interface IActiveDirectoryService
{
    ValueTask PrintActiveDirectoryInfoAsync();
}

internal sealed class ActiveDirectoryService(ILogger<ActiveDirectoryService> logger, IDomainProvider domainProvider, IDomainControllerProvider domainControllerProvider) : IActiveDirectoryService
{
    private const int LENGTH = 40;

    public async ValueTask PrintActiveDirectoryInfoAsync()
    {
        var domainContainer = await domainProvider.GetDomainAsync().ConfigureAwait(false);
        if (!domainContainer.IsValid)
        {
            logger.LogError(domainContainer.ErrorMessage);
            return;
        }

        var domainControllerContainer = await domainControllerProvider.GetDomainControllersAsync(domainContainer).ConfigureAwait(false);
        if (!domainControllerContainer.IsValid)
        {
            logger.LogError(domainControllerContainer.ErrorMessage);
            return;
        }

        var str = await GetPrintableStringAsync(domainContainer, domainControllerContainer).ConfigureAwait(false);
        logger.LogInformation(str);
    }

    private ValueTask<string> GetPrintableStringAsync(IDomainContainer domainContainer, IDomainControllerContainer domainControllerContainer)
    {
        var sb = new StringBuilder();
        sb.AppendLine();
        sb.AppendLine(new string('=', LENGTH));
        sb.AppendLine($"Domain:             {domainContainer?.Domain?.Name}");
        sb.AppendLine("Controllers:");
        foreach (var domainController in domainControllerContainer.DomainControllers)
        {
            sb.AppendLine($"    -               {domainController.Name}");
            sb.AppendLine($"                    {domainController.SiteName}");
            sb.AppendLine($"                    {domainController.IPAddress}");
            sb.AppendLine($"                    {domainController.OSVersion}");
        }
        sb.AppendLine(new string('=', LENGTH));

        return ValueTask.FromResult(sb.ToString());
    }
}
