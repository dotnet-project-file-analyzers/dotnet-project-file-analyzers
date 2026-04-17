namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Implements <see cref="Rule.BuildActionsShouldHaveSingleTask" />.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class BuildActionsShouldHaveSingleTask() : MsBuildProjectFileAnalyzer(Rule.BuildActionsShouldHaveSingleTask)
{
    /// <inheritdoc />
    public override ImmutableArray<ProjectFileType> ApplicableTo => ProjectFileTypes.AllExceptSDK;

    /// <inheritdoc />
    public override bool DisableOnFailingImport => false;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var node in context.File.ItemGroups.Children<BuildAction>(HasMutlpleTasks))
        {
            context.ReportDiagnostic(Descriptor, node, node.LocalName);
        }
    }

    [Pure]
    private static bool HasMultipleTasks(BuildAction node)
        => node.Include.Count
        + node.Remove.Count
        + node.Update.Count
        > 1;
}
