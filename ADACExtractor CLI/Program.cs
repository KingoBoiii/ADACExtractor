using Cocona;

var builder = CoconaApp.CreateBuilder(args, c =>
{
    c.TreatPublicMethodsAsCommands = false;
});

var app = builder.Build();

app.AddCommand(() =>
{
    Console.WriteLine("Hello, world!");
});

app.AddCommands<MyCommands>();

app.Run();
