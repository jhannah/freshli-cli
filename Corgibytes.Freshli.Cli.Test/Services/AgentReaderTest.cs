using System;
using System.Collections.Generic;
using Corgibytes.Freshli.Cli.DataModel;
using Corgibytes.Freshli.Cli.Extensions;
using Corgibytes.Freshli.Cli.Functionality;
using Corgibytes.Freshli.Cli.Services;
using Moq;
using PackageUrl;
using Xunit;

namespace Corgibytes.Freshli.Cli.Test.Services;

[UnitTest]
public class AgentReaderTest
{
    private string _agentExecutable;
    private Mock<ICommandInvoker> _commandInvoker;
    private Mock<ICacheManager> _cacheManager;
    private Mock<ICacheDb> _cacheDb;
    private PackageURL _packageUrl;
    private Package _alphaPackage;
    private Package _betaPackage;
    private Package _gammaPackage;
    private List<Package> _expectedPackages;
    private AgentReader _reader;

    public AgentReaderTest()
    {
        _agentExecutable = "/path/to/agent";
        _commandInvoker = new Mock<ICommandInvoker>();
        _cacheManager = new Mock<ICacheManager>();
        _cacheDb = new Mock<ICacheDb>();
        _packageUrl = new PackageURL("pkg:maven/org.example/package");
        _alphaPackage = new Package(
            new PackageURL("pkg:maven/org.example/package@1"),
            new DateTimeOffset(2021, 12, 13, 14, 15, 16, TimeSpan.FromHours(-4)));
        _betaPackage = new Package(
            new PackageURL("pkg:maven/org.example/package@2"),
            _alphaPackage.ReleasedAt.AddMonths(1));
        _gammaPackage = new Package(
            new PackageURL("pkg:maven/org.example/package@3"),
            _alphaPackage.ReleasedAt.AddMonths(2));
        _expectedPackages = new List<Package>
        {
            _alphaPackage,
            _betaPackage,
            _gammaPackage
        };

        _cacheManager.Setup(mock => mock.GetCacheDb()).Returns(_cacheDb.Object);
        _reader = new AgentReader(_cacheManager.Object, _commandInvoker.Object, _agentExecutable);
    }

    [Fact]
    public void RetrieveReleaseHistoryWritesToCache()
    {
        var agentExecutable = "/path/to/agent";

        var commandResponse =
            $"{_alphaPackage.PackageUrl.Version}\t{_alphaPackage.ReleasedAt:yyyy'-'MM'-'dd'T'HH':'mm':'ssK}\n" +
            $"{_betaPackage.PackageUrl.Version}\t{_betaPackage.ReleasedAt:yyyy'-'MM'-'dd'T'HH':'mm':'ssK}\n" +
            $"{_gammaPackage.PackageUrl.Version}\t{_gammaPackage.ReleasedAt:yyyy'-'MM'-'dd'T'HH':'mm':'ssK}\n";

        _commandInvoker.Setup(mock => mock.Run(agentExecutable,
            $"retrieve-release-history {_packageUrl.FormatWithoutVersion()}", ".")).Returns(commandResponse);

        var initialCachedPackages = new List<CachedPackage>();

        _cacheManager.Setup(mock => mock.GetCacheDb()).Returns(_cacheDb.Object);
        _cacheDb.Setup(mock => mock.RetrieveCachedReleaseHistory(_packageUrl)).Returns(initialCachedPackages);

        var reader = new AgentReader(_cacheManager.Object, _commandInvoker.Object, agentExecutable);

        var retrievedPackages = reader.RetrieveReleaseHistory(_packageUrl);

        Assert.Equal(_expectedPackages, retrievedPackages);

        _cacheDb.Verify(mock => mock.StoreCachedReleaseHistory(It.Is<List<CachedPackage>>(value =>
            value.Count == 3 &&
            value[0].ToPackage().Equals(_alphaPackage) &&
            value[1].ToPackage().Equals(_betaPackage) &&
            value[2].ToPackage().Equals(_gammaPackage)
        )));
    }

    [Fact]
    public void RetrieveReleaseHistoryReadsFromCache()
    {
        var initialCachedPackages = new List<CachedPackage>
        {
            new(_alphaPackage),
            new(_betaPackage),
            new(_gammaPackage)
        };

        _cacheDb.Setup(mock => mock.RetrieveCachedReleaseHistory(_packageUrl)).Returns(initialCachedPackages);

        var retrievedPackages = _reader.RetrieveReleaseHistory(_packageUrl);

        Assert.Equal(_expectedPackages, retrievedPackages);
    }
}
