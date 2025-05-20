using DotNetProjectFile.BuildAgents;

namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class EnableContinuousIntegrationBuild() : MsBuildProjectFileAnalyzer(Rule.EnableContinuousIntegrationBuild)
{
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
        var handled = GetDefinitions();

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

    private static HashSet<BuildAgent> GetPossibleBuildAgents()
    {
        var result = new HashSet<BuildAgent>();
        var values = Enum.GetValues(typeof(BuildAgent));

        for (var i = 0; i < values.Length; i++)
        {
            var value = (BuildAgent)values.GetValue(i);
            if (value != BuildAgent.Local && value.IsActive())
            {
                result.Add(value);
            }
        }

        return result;
    }

    private static HashSet<BuildAgent> GetDefinitions()
    {
        var result = new HashSet<BuildAgent>();

        return result;
    }
}
