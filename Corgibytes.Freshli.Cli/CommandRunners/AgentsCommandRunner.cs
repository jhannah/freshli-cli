using System;
using System.CommandLine.Invocation;
using System.IO;
using System.Linq;
using System.Text;
using Corgibytes.Freshli.Cli.CommandOptions;
using Corgibytes.Freshli.Cli.Commands;
using Corgibytes.Freshli.Cli.Resources;
using Corgibytes.Freshli.Lib;
using TextTableFormatter;
using Environment = System.Environment;
using System.CommandLine;
using System.Reflection;
using CliWrap;
using System.Diagnostics;
namespace Corgibytes.Freshli.Cli.CommandRunners;

public class AgentsCommandRunner : CommandRunner<AgentsCommand, EmptyCommandOptions>
{
    public AgentsCommandRunner(IServiceProvider serviceProvider, Runner runner)
        : base(serviceProvider, runner)
    {
    }

    public override int Run(EmptyCommandOptions options, InvocationContext context) => 0;
}

public class AgentsDetectCommandRunner : CommandRunner<AgentsDetectCommand, EmptyCommandOptions>
{
    public AgentsDetectCommandRunner(IServiceProvider serviceProvider, Runner runner, AgentsDetector agentsDetector)
        : base(serviceProvider, runner) =>
        AgentsDetector = agentsDetector;

    private AgentsDetector AgentsDetector { get; }

    public override int Run(EmptyCommandOptions options, InvocationContext context)
    {
        var agents = AgentsDetector.Detect();

        var agentsAndLocations = agents.ToDictionary(Path.GetFileName);
        foreach (var agentAndLocation in agentsAndLocations)
        {
            Console.WriteLine("file\t" + $"{agentAndLocation.Key}");
            Console.WriteLine("path\t" +  $"{agentAndLocation.Value}");
        }

        if (agentsAndLocations.Count == 0)
        {
            Console.WriteLine(CliOutput.AgentsDetectCommandRunner_Run_No_detected_agents_found);
        }

        return 0;
    }
}

public class AgentsVerifyCommandRunner : CommandRunner<AgentsVerifyCommand, AgentsVerifyCommandOptions>
{
    private readonly AgentsDetector _agentsDetector;
    public AgentsVerifier AgentsVerifier { get; }
    public AgentsVerifyCommandRunner(IServiceProvider serviceProvider, Runner runner, AgentsVerifier agentsVerifier, AgentsDetector agentsDetector)
        : base(serviceProvider, runner)
    {
        _agentsDetector = agentsDetector;
        AgentsVerifier = agentsVerifier;
    }

    public override int Run(AgentsVerifyCommandOptions options, InvocationContext context)
    {
        var agents = _agentsDetector.Detect();


        if (options.LanguageName == null)
        {
            
            foreach (var agentsAndPath in agents)
            {
             
              AgentsVerifier.RunAgentsVerify(agentsAndPath,"validating-repositories", options.CacheDir, "");
            //  AgentsVerifier.RunProcessOutput(agentsAndPath,"detect-manifests");
            //  AgentsVerifier.RunProcessOutput(agentsAndPath,"process-manifests");
            }
        } else
        {
            
            foreach (var agentsAndPath in agents)
            {
                if(agentsAndPath.ToLower().Contains("freshli-agent-"+options.LanguageName.ToLower())){
                
                AgentsVerifier.RunAgentsVerify(agentsAndPath,"validating-repositories", options.CacheDir, options.LanguageName);
                // AgentsVerifier.RunProcessOutput(agentsAndPath,"detect-manifests");
                // AgentsVerifier.RunProcessOutput(agentsAndPath,"process-manifests");

                }
                
            }
            Console.WriteLine(options.LanguageName + " my word length");
        }
        
       
        


        // string executableFile = "/Users/dona/Desktop/projects/freshli-cli/agents_verify.sh";
        // var processInfo = new ProcessStartInfo();
        // processInfo.UseShellExecute = false;
        // processInfo.FileName = executableFile;   // 'sh' for bash
        //
        // var stdErrBuffer = new StringBuilder();
        // var stdOutBuffer = new StringBuilder();
        // var command = CliWrap.Cli.Wrap(gitPath).WithArguments(
        //         args => args
        //             .Add("log")
        //             // Commit hash, author date, strict ISO 8601 format
        //             // Lists commits as '583d813db3e28b9b44a29db352e2f0e1b4c6e420 2021-05-19T15:24:24-04:00'
        //             // Source: https://git-scm.com/docs/pretty-formats
        //             .Add("--pretty=format:%H %aI")
        //     )
        //     .WithValidation(CommandResultValidation.None)
        //     .WithWorkingDirectory(gitSource.Directory.FullName)
        //     .WithStandardOutputPipe(PipeTarget.ToStringBuilder(stdOutBuffer))
        //     .WithStandardErrorPipe(PipeTarget.ToStringBuilder(stdErrBuffer));
        //
        // using var task = command.ExecuteAsync().Task;
        // task.Wait();

        

        context.Console.WriteLine(options.Workers + " Dona's workers found");
        return 0;
    }
}
