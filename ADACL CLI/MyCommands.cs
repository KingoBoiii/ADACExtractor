using Cocona;

public class MyCommands
{
    [Command("hello")]
    public Task HelloWorld([Argument] string name)
    {
        Console.WriteLine($"Hello, {name}");

        return Task.CompletedTask;
    }
}