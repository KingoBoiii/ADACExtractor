using ADACL;
using Cocona;
using Microsoft.Extensions.Logging;
using System.Runtime.Versioning;

namespace ADACL.CLI.Commands;

[SupportedOSPlatform("windows")]
[HasSubCommands(typeof(ActiveDirectoryShowCommands), "show")]
public class ActiveDirectoryCommands(ILogger<ActiveDirectoryCommands> logger, IDomainService domainService, IDomainControllerService domainControllerService) : CommandBase<ActiveDirectoryCommands>(logger)
{
    [PrimaryCommand, Command("inspect")]
    public async ValueTask InspectAsync()
    {
        var computerDomain = await domainService.GetComputerDomainAsync().ConfigureAwait(false);
        if (computerDomain is null)
        {
            Logger.LogError("No domain found");

            return;
        }

        Logger.LogInformation("Domain found: {}", computerDomain.Name);

        var domainControllers = await domainControllerService.GetDomainControllersAsync(computerDomain).ConfigureAwait(false);

        foreach (var domainController in domainControllers)
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
    }
}
