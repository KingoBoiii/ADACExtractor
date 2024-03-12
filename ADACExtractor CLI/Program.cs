using ADACExtractor.CLI.Commands;
using ADACExtractor.Extensions.DependencyInjection;
using Cocona;
using Microsoft.Extensions.Logging;

#if DEBUG
if (args.Length == 0)
{
    args = new string[]
    {
        "active-directory",
        "inspect"
    };
}
#endif

var builder = CoconaApp.CreateBuilder(args, c =>
{
    c.TreatPublicMethodsAsCommands = false;
});

builder.Logging.AddDebug();

builder.Services.AddWindowsActiveDirectory();

var app = builder.Build();

app.AddSubCommand("active-directory", config =>
{
    config.AddCommands<ActiveDirectoryCommands>();
});

app.AddCommands<SystemInformationCommands>();
app.AddCommands<MyCommands>();

app.Run();
