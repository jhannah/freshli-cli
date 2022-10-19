using System;
using Corgibytes.Freshli.Cli.DataModel;
using Corgibytes.Freshli.Cli.Functionality;
using Corgibytes.Freshli.Cli.Functionality.Engine;
using Corgibytes.Freshli.Cli.Functionality.LibYear;
using Corgibytes.Freshli.Cli.Services;
using Moq;
using PackageUrl;
using Xunit;

namespace Corgibytes.Freshli.Cli.Test.Functionality.LibYear;

[UnitTest]
public class ComputeLibYearForPackageActivityTest
{
    [Fact]
    public void HandleComputesLibYearAndFiresLibYearComputedForPackageEvent()
    {
        var analysisId = Guid.NewGuid();
        var asOfDateTime = new DateTimeOffset(2021, 1, 29, 12, 30, 45, 0, TimeSpan.Zero);
        const string agentExecutablePath = "/path/to/agent/smith";
        var package = new PackageURL("pkg:nuget/org.corgibytes.calculatron/calculatron@14.6");
        var packageLibYear = new PackageLibYear(
            asOfDateTime,
            package,
            asOfDateTime,
            package,
            6.2,
            asOfDateTime
        );

        const int historyStopPointId = 29;
        const int packageLibYearId = 9;
        var activity = new ComputeLibYearForPackageActivity
        {
            AnalysisId = analysisId,
            HistoryStopPointId = historyStopPointId,
            AgentExecutablePath = agentExecutablePath,
            Package = package
        };

        var eventClient = new Mock<IApplicationEventEngine>();
        var serviceProvider = new Mock<IServiceProvider>();
        var calculator = new Mock<IPackageLibYearCalculator>();
        var agentManager = new Mock<IAgentManager>();
        var agentReader = new Mock<IAgentReader>();
        var cacheManager = new Mock<ICacheManager>();
        var cacheDb = new Mock<ICacheDb>();
        var historyStopPoint = new CachedHistoryStopPoint { AsOfDateTime = asOfDateTime };

        agentManager.Setup(mock => mock.GetReader(agentExecutablePath)).Returns(agentReader.Object);
        calculator.Setup(mock => mock.ComputeLibYear(agentReader.Object, package, asOfDateTime))
            .Returns(packageLibYear);
        cacheManager.Setup(mock => mock.GetCacheDb()).Returns(cacheDb.Object);
        cacheDb.Setup(mock => mock.RetrieveHistoryStopPoint(historyStopPointId)).Returns(historyStopPoint);
        cacheDb.Setup(mock => mock.AddPackageLibYear(It.IsAny<CachedPackageLibYear>())).Returns(packageLibYearId);

        eventClient.Setup(mock => mock.ServiceProvider).Returns(serviceProvider.Object);
        serviceProvider.Setup(mock => mock.GetService(typeof(IPackageLibYearCalculator))).Returns(calculator.Object);
        serviceProvider.Setup(mock => mock.GetService(typeof(IAgentManager))).Returns(agentManager.Object);
        serviceProvider.Setup(mock => mock.GetService(typeof(ICacheManager))).Returns(cacheManager.Object);

        activity.Handle(eventClient.Object);

        eventClient.Verify(mock => mock.Fire(It.Is<LibYearComputedForPackageEvent>(value =>
            value.AnalysisId == analysisId &&
            value.HistoryStopPointId == historyStopPointId &&
            value.PackageLibYearId == packageLibYearId &&
            value.AgentExecutablePath == agentExecutablePath)));
    }
}