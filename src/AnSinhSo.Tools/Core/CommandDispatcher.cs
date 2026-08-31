using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace AnSinhSo.Tools.Core;

public class CommandDispatcher
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ToolRegistry _registry;

    public CommandDispatcher(IServiceProvider serviceProvider, ToolRegistry registry)
    {
        _serviceProvider = serviceProvider;
        _registry = registry;
    }

    public async Task DispatchAsync(string[] args, CancellationToken cancellationToken = default)
    {
        if (args.Length == 0)
        {
            PrintUsage();
            return;
        }

        var commandName = args[0];
        var commands = _registry.GetRegisteredCommands();

        if (commands.TryGetValue(commandName, out var commandType))
        {
            var commandArgs = args.Skip(1).ToArray();
            var commandInstance = (IToolCommand)_serviceProvider.GetRequiredService(commandType);

            Console.WriteLine($"Executing: {commandInstance.Name}");
            await commandInstance.ExecuteAsync(commandArgs, cancellationToken);
        }
        else
        {
            Console.WriteLine($"Error: Unknown command '{commandName}'");
            PrintUsage();
        }
    }

    private void PrintUsage()
    {
        Console.WriteLine("AnSinhSo Enterprise Tool Center");
        Console.WriteLine("Available commands:");
        foreach (var cmd in _registry.GetRegisteredCommands().Keys)
        {
            Console.WriteLine($"  {cmd}");
        }
    }
}
