using Corgibytes.Freshli.Cli.Functionality.Engine;

namespace Corgibytes.Freshli.Cli.Commands.Cache;

public class CacheDestroyedEvent : IApplicationEvent
{
    public string ResultMessage { get; init; }

    public void Handle(IApplicationActivityEngine eventClient)
    {
    }
}
