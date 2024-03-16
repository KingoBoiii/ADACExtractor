using ADACL.CLI.Commands;
using ADACL.Extensions.DependencyInjection;
using Cocona;
using Microsoft.Extensions.Logging;

#if DEBUG
if (args.Length == 0)
{
    args = new string[]
    {
        "active-directory"
    };
}
#endif

var builder = CoconaApp.CreateBuilder(args, c =>
{
    c.TreatPublicMethodsAsCommands = false;
});

builder.Logging.AddSimpleConsole(c =>
{
    c.TimestampFormat = "HH:mm:ss ";
    c.UseUtcTimestamp = true;
});

builder.Services.AddADACL();

var app = builder.Build();

app.AddSubCommand("active-directory", config =>
{
    config.AddCommands<ActiveDirectoryCommands>();
});

app.Run();
