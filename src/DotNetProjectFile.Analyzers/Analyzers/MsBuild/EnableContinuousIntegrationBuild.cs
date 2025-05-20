using DotNetProjectFile.BuildAgents;
using System.Text.RegularExpressions;

namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class EnableContinuousIntegrationBuild() : MsBuildProjectFileAnalyzer(Rule.EnableContinuousIntegrationBuild)
{
    private static readonly ImmutableArray<BuildAgent> BuildAgentValues = GetBuildAgentValues();

    /// <inheritdoc />
    public override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (context.File
            .Walk()
            .OfType<PackageReferenceBase>()
            .Any(p => p is not PackageVersion && p.Include.IsMatch("DotNet.ReproducibleBuilds")))
        {
            return;
        }

        var active = GetPossibleBuildAgents();
        var handled = GetDefinitions(context.File);

        if (active.Count == 0)
        {
            if (handled.Count == 0)
            {
                context.ReportDiagnostic(Descriptor, context.File);
            }

            // Locally trust that the handled agent is the one actually being used.
            return;
        }

        if (active.None(handled.Contains))
        {
            context.ReportDiagnostic(Descriptor, context.File);
        }
    }

    private static ImmutableArray<BuildAgent> GetBuildAgentValues()
    {
        var values = Enum.GetValues(typeof(BuildAgent));
        var result = new BuildAgent[values.Length - 1];
        var index = 0;

        for (var i = 0; i < values.Length; i++)
        {
            var value = (BuildAgent)values.GetValue(i);
            if (value != BuildAgent.Local)
            {
                result[index++] = value;
            }
        }

        return result.ToImmutableArray();
    }

    private static HashSet<BuildAgent> GetPossibleBuildAgents()
        => [.. BuildAgentValues.Where(a => a.IsActive())];

    private static HashSet<BuildAgent> GetDefinitions(Project project)
        => [.. BuildAgentValues.Where(a => HasDefinition(project, a))];

    private static bool HasDefinition(Project project, BuildAgent agent)
        => project
            .Properties<ContinuousIntegrationBuild>()
            .Any(prop => IsValidDefinition(prop, agent));

    private static bool IsValidDefinition(ContinuousIntegrationBuild prop, BuildAgent agent)
    {

        var cur = (Node)prop;
        while (cur is not null)
        {
            if (IsValidCondition(cur.Condition, agent))
            {
                return true;
            }

            cur = cur.Parent;
        }

        return false;
    }

    private static bool IsValidCondition(string? cond, BuildAgent agent)
    {
        if (cond is null)
        {
            return false;
        }

        var normalized = Regex.Replace(cond, @"\s+", string.Empty);
        var (trueVars, nonEmptyVars) = agent.GetRequirements();

        if (trueVars.Any(v => !IsValidTruthCondition(normalized, v)))
        {
            return false;
        }

        if (nonEmptyVars.Any(v => !IsValidNonEmptyCondition(normalized, v)))
        {
            return false;
        }

        return true;
    }

    private static bool IsValidTruthCondition(string cond, string envVariable)
        => cond == $"'$({envVariable})'=='true'"
        || cond == $"'true'=='$({envVariable})'";

    private static bool IsValidNonEmptyCondition(string cond, string envVariable)
        => cond == $"'$({envVariable})'!=''"
        || cond == $"''!='$({envVariable})'";
}
