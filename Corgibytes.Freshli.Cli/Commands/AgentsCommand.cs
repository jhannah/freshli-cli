using System;
using System.CommandLine;
using Corgibytes.Freshli.Cli.CommandOptions;

namespace Corgibytes.Freshli.Cli.Commands;

public class AgentsCommand : Command
{
    public AgentsCommand() : base("agents", "Detects all of the language agents that are available for use")
    {
        AgentsDetectCommand detect = new();
        AddCommand(detect);
        AgentsVerifyCommand verify = new();
        AddCommand(verify);
    }
}

public class AgentsDetectCommand : RunnableCommand<AgentsDetectCommand, EmptyCommandOptions>
{
    public AgentsDetectCommand() : base("detect",
        "Outputs the detected language name and the path to the language agent binary in a tabular format")
    {
    }
}

public class AgentsVerifyCommand : RunnableCommand<AgentsVerifyCommand, AgentsVerifyCommandOptions>
{
    public AgentsVerifyCommand() : base("verify",
        "Used to test all of the language agents that are available for use.")
    {
        Argument<string> languageName = new("language-name", "Name of the language")
        {
            Arity = ArgumentArity.ZeroOrOne
        };

        AddArgument(languageName);

        Option<int> formatOption = new(new[] { "--workers" },
            description:
            "Represents the number of worker processes that should be running at any given time. This defaults to twice the number of CPU cores.",
            getDefaultValue: () => Environment.ProcessorCount + 1)
        {
            AllowMultipleArgumentsPerToken = false,
            Arity = ArgumentArity.ExactlyOne
        };
        AddOption(formatOption);
    }
}
