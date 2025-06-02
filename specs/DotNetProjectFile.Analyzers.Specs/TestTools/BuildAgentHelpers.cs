using DotNetProjectFile.BuildAgents;

namespace Specs.TestTools;

public static class BuildAgentHelpers
{
    public static void Run<T>(this BuildAgent agent, Action<T> act)
        where T : DiagnosticAnalyzer, new()
    {
        DisableAgents();
        EnableAgent(agent);

        var analyzer = new T();

        try
        {
            act(analyzer);
        }
        finally
        {
            DisableAgent(agent);
        }
    }

    private static void DisableAgents()
    {
        foreach (var agent in Enum.GetValues<BuildAgent>())
        {
            DisableAgent(agent);
        }
    }

    private static void DisableAgent(BuildAgent agent)
    {
        var reqs = agent.GetRequirements();
        var vars = reqs.TrueValues.Concat(reqs.NonEmptyValues);

        foreach (var name in vars)
        {
            Environment.SetEnvironmentVariable(name, null);
        }
    }

    private static void EnableAgent(BuildAgent agent)
    {
        var reqs = agent.GetRequirements();
        var vars = reqs.TrueValues.Concat(reqs.NonEmptyValues);

        foreach (var name in vars)
        {
            Environment.SetEnvironmentVariable(name, "true");
        }
    }
}
