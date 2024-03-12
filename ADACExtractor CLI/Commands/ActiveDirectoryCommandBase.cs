using Microsoft.Extensions.Logging;
using System.DirectoryServices.ActiveDirectory;

namespace ADACExtractor.CLI.Commands;

public abstract class ActiveDirectoryCommandBase<TCommand>(ILogger<TCommand> logger) : CommandBase<TCommand>(logger)
{
    protected static bool TryGetComputerDomain(out Domain computerDomain, out string? errorMessage)
    {
        try
        {
            computerDomain = Domain.GetComputerDomain();
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
