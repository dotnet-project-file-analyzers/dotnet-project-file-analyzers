using DotNetProjectFile.BuildAgents;

namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class EnableRestoreLockedMode() : MsBuildProjectFileAnalyzer(Rule.EnableRestoreLockedMode)
{
    /// <inheritdoc />
    public override ImmutableArray<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        var project = context.File;
        if (!project.PackagesRestoredWithLockFile())
        {
            return;
        }

        var nodes = context.File.Properties<RestoreLockedMode>().ToImmutableArray();

        if (nodes.Length <= 0)
        {
            context.ReportDiagnostic(Descriptor, context.File);
        }
        else if (nodes.None(node => node.HasAnyCondition(GetAllowedConditions()) && node.Value == true))
        {
            context.ReportDiagnostic(Descriptor, nodes[0]);
        }
    }

    private static IEnumerable<string> GetAllowedConditions()
    {
        yield return "'$(ContinuousIntegrationBuild)'=='true'";
        yield return "'true'=='$(ContinuousIntegrationBuild)'";

        foreach (var condition in BuildAgentExtensions.GetActiveAllowedConditions())
        {
            yield return condition;
        }
    }
}
