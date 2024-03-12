using Cocona;
using Microsoft.Extensions.Logging;
using System.DirectoryServices.ActiveDirectory;
using System.Runtime.Versioning;

namespace ADACExtractor.CLI.Commands;

[SupportedOSPlatform("windows")]
[HasSubCommands(typeof(ActiveDirectoryShowCommands), "show")]
public class ActiveDirectoryCommands(ILogger<ActiveDirectoryCommands> logger) : ActiveDirectoryCommandBase<ActiveDirectoryCommands>(logger)
{
    [PrimaryCommand, Command("inspect")]
    public async ValueTask InspectAsync()
    {
        if (!TryGetComputerDomain(out var computerDomain, out var errorMessage))
        {
            Logger.LogError("No domain found: {}", errorMessage);

            return;
        }

        Logger.LogInformation("Domain found: {}", computerDomain.Name);

        await InspectDomainControllersAsync(computerDomain).ConfigureAwait(false);
    }

    private ValueTask InspectDomainControllersAsync(Domain? domain)
    {
        ArgumentNullException.ThrowIfNull(domain);

        var domainControllers = domain.DomainControllers;
        if (domainControllers.Count == 0)
        {
            Logger.LogError("Cannot find Domain Controller for {}", domain.Name);

            return ValueTask.CompletedTask;
        }

        foreach (DomainController? domainController in domainControllers)
        {
            if (domainController is null)
            {
                continue;
            }

            Logger.LogInformation(@"Domain Controller found: {}
    Name:       {}    
    Site Name:  {}
    IP Address: {}
    OS Version: {}", domainController.Name, domainController.Name, domainController.IPAddress, domainController.SiteName, domainController.OSVersion);
        }

        return ValueTask.CompletedTask;
    }
}
