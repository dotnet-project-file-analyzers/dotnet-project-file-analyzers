using DotNetProjectFile.BuildAgents;

namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Implements <see cref="Rule.EnableRestoreLockedMode"/></summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class EnableRestoreLockedMode() : MsBuildProjectFileAnalyzer(Rule.EnableRestoreLockedMode)
{
    /// <inheritdoc />
    public override ImmutableArray<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        var project = context.File;

        if (context.Props.RestoreLockedMode is false || !project.PackagesRestoredWithLockFile()) return;

        var nodes = context.File.Properties<RestoreLockedMode>().ToImmutableArray();

        if (nodes.Length is 0)
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
