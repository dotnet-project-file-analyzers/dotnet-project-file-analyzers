
namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class BuildActionsShouldHaveSingleTask() : MsBuildProjectFileAnalyzer(Rule.BuildActionsShouldHaveSingleTask)
{
    /// <inheritdoc />
    public override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.AllExceptSDK;

    /// <inheritdoc />
    public override bool DisableOnFailingImport => false;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var node in context.File.ItemGroups
            .SelectMany(group => group.BuildActions)
            .Where(HasMutlpleTasks))
        {
            context.ReportDiagnostic(Descriptor, node, node.LocalName);
        }
    }

    [Pure]
    private static bool HasMutlpleTasks(BuildAction node)
        => node.Include.Count
        + node.Exclude.Count
        + node.Remove.Count
        + node.Update.Count
        > 1;
}
