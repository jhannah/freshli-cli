using System;
using Corgibytes.Freshli.Cli.Functionality.Analysis;
using Corgibytes.Freshli.Cli.Functionality.Engine;
using Corgibytes.Freshli.Cli.Functionality.History;

namespace Corgibytes.Freshli.Cli.Functionality.Git;

public class GitRepositoryInLocalDirectoryVerifiedEvent : IApplicationEvent
{
    public Guid AnalysisId { get; init; }
    public AnalysisLocation AnalysisLocation { get; init; }

    public void Handle(IApplicationActivityEngine eventClient) =>
        eventClient.Dispatch(new ComputeHistoryActivity(AnalysisId, AnalysisLocation));
}
