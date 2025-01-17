using System;
using System.Threading.Tasks;
using Corgibytes.Freshli.Cli.DataModel;
using Corgibytes.Freshli.Cli.Functionality;
using Corgibytes.Freshli.Cli.Functionality.Engine;
using Corgibytes.Freshli.Cli.Functionality.FreshliWeb;
using Moq;
using Xunit;

namespace Corgibytes.Freshli.Cli.Test.Functionality.FreshliWeb;

[UnitTest]
public class CreateApiHistoryStopActivityTest
{
    [Fact(Timeout = Constants.DefaultTestTimeout)]
    public async Task Handle()
    {
        var cachedAnalysisId = Guid.NewGuid();
        var apiAnalysisId = Guid.NewGuid();

        const string repositoryUrl = "repository-url";
        const string repositoryBranch = "branch";
        var asOfDateTime = new DateTimeOffset(2022, 1, 1, 12, 52, 28, 0, TimeSpan.Zero);

        var cachedAnalysis =
            new CachedAnalysis(repositoryUrl, repositoryBranch, "1m", CommitHistory.AtInterval,
                RevisionHistoryMode.AllRevisions)
            { ApiAnalysisId = apiAnalysisId };

        const int historyStopPointId = 29;
        var cacheDb = new Mock<ICacheDb>();
        var historyStopPoint = new CachedHistoryStopPoint { AsOfDateTime = asOfDateTime };
        cacheDb.Setup(mock => mock.RetrieveAnalysis(cachedAnalysisId)).ReturnsAsync(cachedAnalysis);
        cacheDb.Setup(mock => mock.RetrieveHistoryStopPoint(historyStopPointId)).ReturnsAsync(historyStopPoint);

        var cacheManager = new Mock<ICacheManager>();
        cacheManager.Setup(mock => mock.GetCacheDb()).Returns(cacheDb.Object);

        var resultsApi = new Mock<IResultsApi>();

        var serviceProvider = new Mock<IServiceProvider>();
        serviceProvider.Setup(mock => mock.GetService(typeof(ICacheManager))).Returns(cacheManager.Object);
        serviceProvider.Setup(mock => mock.GetService(typeof(IResultsApi))).Returns(resultsApi.Object);

        var eventClient = new Mock<IApplicationEventEngine>();
        eventClient.Setup(mock => mock.ServiceProvider).Returns(serviceProvider.Object);

        var activity = new CreateApiHistoryStopActivity(cachedAnalysisId, historyStopPointId);

        var cancellationToken = new System.Threading.CancellationToken();
        await activity.Handle(eventClient.Object, cancellationToken);

        resultsApi.Verify(mock => mock.CreateHistoryPoint(cacheDb.Object, cachedAnalysisId, historyStopPointId));

        eventClient.Verify(mock =>
            mock.Fire(
                It.Is<ApiHistoryStopCreatedEvent>(value =>
                    value.CachedAnalysisId == cachedAnalysisId &&
                    value.HistoryStopPointId == historyStopPointId
                ),
                cancellationToken,
                ApplicationTaskMode.Tracked
            )
        );
    }
}
