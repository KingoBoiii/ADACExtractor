using ADACL.CLI.Services;
using Cocona;
using Microsoft.Extensions.Logging;
using System.Runtime.Versioning;

namespace ADACL.CLI.Commands;

[SupportedOSPlatform("windows")]
[HasSubCommands(typeof(ActiveDirectoryShowCommands), "show")]
public class ActiveDirectoryCommands(ILogger<ActiveDirectoryCommands> logger, IActiveDirectoryService activeDirectoryService) : CommandBase<ActiveDirectoryCommands>(logger)
{
    [PrimaryCommand, Command("inspect")]
    public async ValueTask InspectAsync()
    {
        await activeDirectoryService.PrintActiveDirectoryInfoAsync().ConfigureAwait(false);
    }
}
