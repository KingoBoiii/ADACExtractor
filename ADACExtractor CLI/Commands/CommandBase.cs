using Microsoft.Extensions.Logging;

namespace ADACExtractor.CLI.Commands;

public abstract class CommandBase<TCommand>(ILogger<TCommand> logger)
{
    protected ILogger<TCommand> Logger { get; } = logger;
}