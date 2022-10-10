using System;
using Corgibytes.Freshli.Cli.Functionality.Analysis;
using Corgibytes.Freshli.Cli.Functionality.Engine;
using Microsoft.Extensions.DependencyInjection;

namespace Corgibytes.Freshli.Cli.Functionality.FreshliWeb;

public class CreateApiHistoryStopActivity : IApplicationActivity
{
    public CreateApiHistoryStopActivity(Guid cachedAnalysisId, IHistoryStopData historyStopData)
    {
        CachedAnalysisId = cachedAnalysisId;
        HistoryStopData = historyStopData;
    }

    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
    public Guid CachedAnalysisId { get; set; }

    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
    public IHistoryStopData HistoryStopData { get; set; }

    public void Handle(IApplicationEventEngine eventClient)
    {
        var resultsApi = eventClient.ServiceProvider.GetRequiredService<IResultsApi>();
        var cacheManager = eventClient.ServiceProvider.GetRequiredService<ICacheManager>();
        var cacheDb = cacheManager.GetCacheDb();

        var cachedAnalysis = cacheDb.RetrieveAnalysis(CachedAnalysisId);
        resultsApi.CreateHistoryPoint(cachedAnalysis!.ApiAnalysisId!.Value, HistoryStopData.AsOfDate!.Value);

        eventClient.Fire(new ApiHistoryStopCreatedEvent(CachedAnalysisId, HistoryStopData));
    }
}
