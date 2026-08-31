using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace AnSinhSo.Tools.Core;

public class ToolRegistry
{
    private readonly IServiceCollection _services;
    private readonly Dictionary<string, Type> _commands = new(StringComparer.OrdinalIgnoreCase);

    public ToolRegistry(IServiceCollection services)
    {
        _services = services;
    }

    public ToolRegistry RegisterCommand<TCommand>(string name) where TCommand : class, IToolCommand
    {
        _services.AddTransient<TCommand>();
        _commands[name] = typeof(TCommand);
        return this;
    }

    internal IReadOnlyDictionary<string, Type> GetRegisteredCommands() => _commands;
}
