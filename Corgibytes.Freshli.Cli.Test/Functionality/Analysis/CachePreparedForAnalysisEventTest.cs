using System;
using System.Threading.Tasks;
using Corgibytes.Freshli.Cli.Functionality;
using Corgibytes.Freshli.Cli.Functionality.Analysis;
using Corgibytes.Freshli.Cli.Functionality.Engine;
using Moq;
using Xunit;

namespace Corgibytes.Freshli.Cli.Test.Functionality.Analysis;

[UnitTest]
public class CachePreparedForAnalysisEventTest
{
    [Fact(Timeout = Constants.DefaultTestTimeout)]
    public async Task CorrectlyDispatchesRestartAnalysisActivity()
    {
        var serviceProvider = new Mock<IServiceProvider>();
        var configuration = new Mock<IConfiguration>();
        var cacheManager = new Mock<ICacheManager>();
        var historyIntervalParser = new Mock<IHistoryIntervalParser>();
        var cancellationToken = new System.Threading.CancellationToken(false);

        var cacheEvent = new CachePreparedForAnalysisEvent
        {
            RepositoryUrl = "https://git.example.com",
            RepositoryBranch = "main",
            HistoryInterval = "1m"
        };

        serviceProvider.Setup(mock => mock.GetService(typeof(IConfiguration))).Returns(configuration.Object);
        serviceProvider.Setup(mock => mock.GetService(typeof(ICacheManager))).Returns(cacheManager.Object);
        serviceProvider.Setup(mock => mock.GetService(typeof(IHistoryIntervalParser)))
            .Returns(historyIntervalParser.Object);

        var engine = new Mock<IApplicationActivityEngine>();
        engine.Setup(mock => mock.ServiceProvider).Returns(serviceProvider.Object);

        await cacheEvent.Handle(engine.Object, cancellationToken);

        engine.Verify(
            mock => mock.Dispatch(
                It.Is<RestartAnalysisActivity>(value =>
                    value.RepositoryUrl == "https://git.example.com" &&
                    value.RepositoryBranch == "main" &&
                    value.HistoryInterval == "1m"
                ),
                cancellationToken,
                ApplicationTaskMode.Tracked
            )
        );
    }
}
