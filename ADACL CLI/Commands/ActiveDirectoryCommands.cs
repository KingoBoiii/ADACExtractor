using ADACL.CLI.Services;
using Cocona;
using Microsoft.Extensions.Logging;
using System.Runtime.Versioning;

namespace ADACL.CLI.Commands;

[SupportedOSPlatform("windows")]
public class ActiveDirectoryCommands(ILogger<ActiveDirectoryCommands> logger, ISystemInfoService systemInfoService, IActiveDirectoryService activeDirectoryService) : CommandBase<ActiveDirectoryCommands>(logger)
{
    [PrimaryCommand, Command("inspect")]
    public async ValueTask InspectAsync()
    {
        await systemInfoService.PrintSystemInfoAsync().ConfigureAwait(false);

        await activeDirectoryService.PrintActiveDirectoryInfoAsync().ConfigureAwait(false);
    }
}
