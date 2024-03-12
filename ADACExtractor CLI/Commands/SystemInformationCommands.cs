using Cocona;
using Microsoft.Extensions.Logging;

namespace ADACExtractor.CLI.Commands;

public class SystemInformationCommands(ILogger<SystemInformationCommands> logger) : CommandBase<SystemInformationCommands>(logger)
{
    [Command("system-info")]
    public Task GetSystemInformationAsync()
    {
        const string title = "System Information";

        Logger.LogInformation(title + new string('=', Console.WindowWidth - title.Length));
        Logger.LogInformation("Machine Name:        {}", Environment.MachineName);
        Logger.LogInformation("Current Directory:   {}", Environment.CurrentDirectory);
        Logger.LogInformation("Privileged Process:  {}", Environment.IsPrivilegedProcess);
        Logger.LogInformation("64 bit OS:           {}", Environment.Is64BitOperatingSystem);
        Logger.LogInformation("64 bit Process:      {}", Environment.Is64BitProcess);
        Logger.LogInformation("OS Version:          {}", Environment.OSVersion.VersionString);
        Logger.LogInformation("OS Platform:         {}", Environment.OSVersion.Platform);
        Logger.LogInformation("User Domain Name:    {}", Environment.UserDomainName);
        Logger.LogInformation("User Name:           {}", Environment.UserName);
        Logger.LogInformation(new string('=', Console.WindowWidth));

        return Task.CompletedTask;
    }
}
