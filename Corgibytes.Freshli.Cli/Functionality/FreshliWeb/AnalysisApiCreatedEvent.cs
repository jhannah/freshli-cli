using System;
using System.IO;
using Corgibytes.Freshli.Cli.Functionality.Engine;
using Corgibytes.Freshli.Cli.Functionality.Git;

namespace Corgibytes.Freshli.Cli.Functionality.FreshliWeb;

public class AnalysisApiCreatedEvent : IApplicationEvent
{
    public Guid AnalysisId { get; init; }
    public string RepositoryUrl { get; init; } = null!;

    public void Handle(IApplicationActivityEngine eventClient)
    {
        if (Directory.Exists(RepositoryUrl))
        {
            eventClient.Dispatch(new VerifyGitRepositoryInLocalDirectoryActivity { AnalysisId = AnalysisId });
        }
        else
        {
            eventClient.Dispatch(new CloneGitRepositoryActivity(AnalysisId));
        }
    }
}