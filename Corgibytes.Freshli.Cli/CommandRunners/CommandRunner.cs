﻿using System;
using System.Collections.Generic;
using System.CommandLine.Invocation;
using Corgibytes.Freshli.Lib;

namespace Corgibytes.Freshli.Cli.CommandRunners;

public abstract class CommandRunner<T> : ICommandRunner<T> where T : CommandOptions.CommandOptions
{
    public CommandRunner(IServiceProvider serviceProvider, Runner runner)
    {
        Runner = runner;
        Services = serviceProvider;
    }

    protected Runner Runner { get; }
    protected IServiceProvider Services { get; }

    public abstract int Run(T options, InvocationContext context);

    protected static bool Confirm(string message, InvocationContext context, bool defaultYes = false)
    {
        // Prompt the user whether they want to proceed
        var prompt = defaultYes ? "[Y/n]" : "[y/N]";
        context.Console.Out.Write($"{message} {prompt} ");
        var choice = Console.In.ReadLine();

        var yesChoices = new List<string>
        {
            "y",
            "Y"
        };
        var noChoices = new List<string>
        {
            "n",
            "N"
        };

        return defaultYes ? !noChoices.Contains(choice) : yesChoices.Contains(choice);
    }
}
