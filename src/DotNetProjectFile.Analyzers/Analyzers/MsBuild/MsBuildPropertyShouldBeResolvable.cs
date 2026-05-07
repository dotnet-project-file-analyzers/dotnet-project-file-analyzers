namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Implements <see cref="Rule.MsBuildPropertyCouldNotBeResolved"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class MsBuildPropertyShouldBeResolvable() : MsBuildProjectFileAnalyzer(Rule.MsBuildPropertyCouldNotBeResolved)
{
    /// <inheritdoc />
    public override ImmutableArray<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    /// <inheritdoc />
    public override bool DisableOnFailingImport => false;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var reference in context.File.Walk().OfType<ProjectReference>())
        {
            ReportUnresolved(context, reference, reference.Include);
        }

        foreach (var action in context.File.Walk().OfType<BuildAction>())
        {
            foreach (var include in action.Include)
            {
                ReportUnresolved(context, action, include);
            }
        }
    }

    private static void ReportUnresolved(ProjectFileAnalysisContext context, Node node, string? include)
    {
        if (include is not { Length: > 0 }) return;
        var (_, unresolved) = context.Substitute(node, include);
        foreach (var name in unresolved)
        {
            context.ReportDiagnostic(Rule.MsBuildPropertyCouldNotBeResolved, node, name, include);
        }
    }
}
