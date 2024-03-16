using Microsoft.Extensions.Logging;
using System.Text;

namespace ADACL.CLI.Services;

public interface ISystemInfoService
{
    ValueTask PrintSystemInfoAsync();
}


internal sealed class SystemInfoService(ILogger<SystemInfoService> logger) : ISystemInfoService
{
    private const int LENGTH = 40;

    public async ValueTask PrintSystemInfoAsync()
    {
        var str = await GetPrintableStringAsync().ConfigureAwait(false);
        logger.LogInformation(str);
    }

    private ValueTask<string> GetPrintableStringAsync()
    {
        var sb = new StringBuilder();
        sb.AppendLine();
        sb.AppendLine(new string('=', LENGTH));
        sb.AppendLine($"MachineName:        {Environment.MachineName}");
        sb.AppendLine($"CurrentDirectory:   {Environment.CurrentDirectory}");
        sb.AppendLine($"PrivilegedProcess:  {Environment.IsPrivilegedProcess}");
        sb.AppendLine($"64BitOS:            {Environment.Is64BitOperatingSystem}");
        sb.AppendLine($"64BitProcess:       {Environment.Is64BitProcess}");
        sb.AppendLine($"OS Version:         {Environment.OSVersion.VersionString}");
        sb.AppendLine($"OS Platform:        {Environment.OSVersion.Platform}");
        sb.AppendLine($"UserDomainName:     {Environment.UserDomainName}");
        sb.AppendLine($"UserName:           {Environment.UserName}");
        sb.AppendLine(new string('=', LENGTH));

        return ValueTask.FromResult(sb.ToString());
    }
}
