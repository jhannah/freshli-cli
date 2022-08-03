using System;
using System.CommandLine.Invocation;
using System.CommandLine.IO;
using Corgibytes.Freshli.Cli.CommandOptions;
using Corgibytes.Freshli.Cli.Commands;
using Corgibytes.Freshli.Cli.Extensions;
using Corgibytes.Freshli.Cli.Functionality;
using Corgibytes.Freshli.Cli.Resources;
using Corgibytes.Freshli.Lib;

namespace Corgibytes.Freshli.Cli.CommandRunners.Cache;

public class CachePrepareCommandRunner : CommandRunner<CacheCommand, CachePrepareCommandOptions>
{
    private ICacheManager CacheManager { get; }

    public CachePrepareCommandRunner(IServiceProvider serviceProvider, ICacheManager cacheManager, Runner runner)
        : base(serviceProvider, runner)
    {
        CacheManager = cacheManager;
    }

    public override int Run(CachePrepareCommandOptions options, InvocationContext context)
    {
        context.Console.Out.WriteLine(
            string.Format(CliOutput.CachePrepareCommandRunner_Run_Preparing_cache, options.CacheDir)
        );
        try
        {
            return CacheManager.Prepare(options.CacheDir).ToExitCode();
        }
        catch (CacheException e)
        {
            context.Console.Error.WriteLine(e.Message);
            return false.ToExitCode();
        }
    }
}
