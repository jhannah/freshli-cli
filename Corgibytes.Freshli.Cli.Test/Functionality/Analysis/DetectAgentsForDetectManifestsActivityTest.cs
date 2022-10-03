using System;
using System.Collections.Generic;
using Corgibytes.Freshli.Cli.Commands;
using Corgibytes.Freshli.Cli.Functionality.Analysis;
using Corgibytes.Freshli.Cli.Functionality.Engine;
using Moq;
using Xunit;

namespace Corgibytes.Freshli.Cli.Test.Functionality.Analysis;

[UnitTest]
public class DetectAgentsForDetectManifestsActivityTest
{
    private readonly Mock<IAgentsDetector> _agentsDetector = new();
    private readonly Mock<IServiceProvider> _serviceProvider = new();
    private readonly Mock<IApplicationEventEngine> _eventEngine = new();
    private readonly Mock<IAnalysisLocation> _analysisLocation = new();


    public DetectAgentsForDetectManifestsActivityTest()
    {

        _serviceProvider.Setup(mock => mock.GetService(typeof(IAgentsDetector))).Returns(_agentsDetector.Object);
        _eventEngine.Setup(mock => mock.ServiceProvider).Returns(_serviceProvider.Object);
    }

    [Fact]
    public void VerifyItDispatchesAgentDetectedForDetectManifestEvent()
    {
        var agentPaths = new List<string>
        {
            "/usr/local/bin/freshli-agent-java",
            "/usr/local/bin/freshli-agent-dotnet"
        };

        _agentsDetector.Setup(mock => mock.Detect()).Returns(agentPaths);

        var activity = new DetectAgentsForDetectManifestsActivity(_analysisLocation.Object);

        activity.Handle(_eventEngine.Object);

        _eventEngine.Verify(mock =>
            mock.Fire(It.Is<AgentDetectedForDetectManifestEvent>(appEvent =>
                appEvent.AnalysisLocation == _analysisLocation.Object &&
                appEvent.AgentExecutablePath == "/usr/local/bin/freshli-agent-java")));

        _eventEngine.Verify(mock =>
            mock.Fire(It.Is<AgentDetectedForDetectManifestEvent>(appEvent =>
                appEvent.AnalysisLocation == _analysisLocation.Object &&
                appEvent.AgentExecutablePath == "/usr/local/bin/freshli-agent-dotnet")));
    }
}
