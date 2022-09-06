namespace Corgibytes.Freshli.Cli.Services;

public class AgentManager : IAgentManager
{
    public IAgentReader GetReader(string agentExecutablePath) => new AgentReader(agentExecutablePath);
}