#pragma warning disable RS1035 // Avoid IO. Unfortunately necessary to detect in current build.

namespace DotNetProjectFile.BuildAgents;

/// <summary>
/// Represents the build agent type.
/// </summary>
public enum BuildAgent
{
    Local = 0,
    AnyCI = 1,
    AzurePipelines = 2,
    GitHubActions = 3,
    AppVeyor = 4,
    TravisCI = 5,
    CircleCI = 6,
    AWSCodeBuild = 7,
    JenkinsOrGoogleCloud = 8,
    TeamCity = 9,
    JetBrainsSpace = 10,
}

public static class BuildAgentExtensions
{
    private static readonly Dictionary<BuildAgent, (ImmutableArray<string> TrueValues, ImmutableArray<string> NonEmptyValues)> requirements = new()
    {
        [BuildAgent.Local] = ([], []),
        [BuildAgent.AnyCI] = (["CI"], []),
        [BuildAgent.AzurePipelines] = (["TF_BUILD"], []),
        [BuildAgent.GitHubActions] = (["GITHUB_ACTIONS"], []),
        [BuildAgent.AppVeyor] = (["APPVEYOR"], []),
        [BuildAgent.TravisCI] = (["TRAVIS"], []),
        [BuildAgent.CircleCI] = (["CIRCLECI"], []),

        [BuildAgent.AWSCodeBuild] = ([], ["CODEBUILD_BUILD_ID"]),
        [BuildAgent.JenkinsOrGoogleCloud] = ([], ["BUILD_ID"]),
        [BuildAgent.TeamCity] = ([], ["TEAMCITY_VERSION"]),
        [BuildAgent.JetBrainsSpace] = ([], ["JB_SPACE_API_URL"]),
    };

    public static (ImmutableArray<string> TrueValues, ImmutableArray<string> NonEmptyValues) GetRequirements(this BuildAgent agent)
        => requirements[agent];

    public static bool IsActive(this BuildAgent agent)
    {
        var (trueVars, nonEmptyVars) = agent.GetRequirements();

        if (trueVars.Any(static v => Environment.GetEnvironmentVariable(v) != "true"))
        {
            return false;
        }

        if (nonEmptyVars.Any(static v => string.IsNullOrEmpty(Environment.GetEnvironmentVariable(v))))
        {
            return false;
        }

        return true;
    }

    public static ImmutableArray<BuildAgent> GetActive()
    {
        var reallyActive = EnumCache.GetValues<BuildAgent>()
            .Where(x => x != BuildAgent.Local && x.IsActive())
            .ToImmutableArray();

        if (reallyActive.Length > 0)
        {
            return reallyActive;
        }

        return EnumCache.GetValues<BuildAgent>()
            .Where(x => x != BuildAgent.Local)
            .ToImmutableArray();
    }

    public static ImmutableArray<string> GetActiveAllowedConditions()
    {
        var active = GetActive();

        IEnumerable<string> Inner()
        {
            foreach (var agent in active)
            {
                var (trueValues, nonEmptyValues) = agent.GetRequirements();

                foreach (var value in trueValues)
                {
                    yield return $"'$({value})'=='true'";
                    yield return $"'true'=='$({value})'";
                }

                foreach (var value in nonEmptyValues)
                {
                    yield return $"'$({value})'!=''";
                    yield return $"''!='$({value})'";
                }
            }
        }

        return Inner().ToImmutableArray();
    }
}
