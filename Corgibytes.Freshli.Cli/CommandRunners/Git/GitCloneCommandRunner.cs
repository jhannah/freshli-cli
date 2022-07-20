using System;
using System.CommandLine.Invocation;
using System.CommandLine.IO;
using Corgibytes.Freshli.Cli.CommandOptions.Git;
using Corgibytes.Freshli.Cli.Commands.Git;
using Corgibytes.Freshli.Cli.Extensions;
using Corgibytes.Freshli.Cli.Functionality.Git;
using Corgibytes.Freshli.Lib;

namespace Corgibytes.Freshli.Cli.CommandRunners.Git;

public class GitCloneCommandRunner : CommandRunner<GitCloneCommand, GitCloneCommandOptions>
{
    public GitCloneCommandRunner(IServiceProvider serviceProvider, Runner runner)
        : base(serviceProvider, runner)
    {
    }

    public override int Run(GitCloneCommandOptions options, InvocationContext context)
    {
        // Clone or pull the given repository and branch.
        var gitRepository = new GitSource(options.RepoUrl, options.Branch, options.CacheDir);
        try
        {
            gitRepository.CloneOrPull(options.GitPath);
        }
        catch (GitException e)
        {
            context.Console.Error.WriteLine(e.Message);
            return false.ToExitCode();
        }

        // Output the hash to the command line for use by the caller.
        context.Console.Out.WriteLine(gitRepository.Hash);
        return true.ToExitCode();
    }
}