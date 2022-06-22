﻿using System;
using System.Collections.Generic;
using System.CommandLine.Invocation;
using System.CommandLine;
using Corgibytes.Freshli.Lib;

namespace Corgibytes.Freshli.Cli.CommandRunners;

public abstract class CommandRunner<TCommand, TCommandOptions> : ICommandRunner<TCommand, TCommandOptions> where TCommand : Command where TCommandOptions : CommandOptions.CommandOptions
{
    protected Runner Runner { get; }
    protected IServiceProvider Services { get; }

    public CommandRunner(IServiceProvider serviceProvider, Runner runner)
    {
        Runner = runner;
        Services = serviceProvider;
    }

    protected static bool Confirm(string message, InvocationContext context, bool defaultYes = false)
    {
        // Prompt the user whether they want to proceed
        var prompt = defaultYes ? "[Y/n]" : "[y/N]";
        context.Console.Out.Write($"{message} {prompt} ");
        var choice = Console.In.ReadLine();

        var yesChoices = new List<string> { "y", "Y" };
        var noChoices = new List<string> { "n", "N" };

        return (defaultYes ? !noChoices.Contains(choice) : yesChoices.Contains(choice));
    }

    public abstract int Run(TCommandOptions options, InvocationContext context);

}
