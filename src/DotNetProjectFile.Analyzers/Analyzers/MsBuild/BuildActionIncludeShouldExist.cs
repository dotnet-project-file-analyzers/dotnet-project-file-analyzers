namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class BuildActionIncludeShouldExist() : MsBuildProjectFileAnalyzer(Rule.BuildActionIncludeShouldExist)
{
    /// <inheritdoc />
    public override bool DisableOnFailingImport => false;

    /// <inheritdoc />
    public override ImmutableArray<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var node in context.File.Walk().OfType<BuildAction>())
        {
            foreach (var include in node.Include.Where(i => context.Files(node, i)?.Any() == false))
            {
                context.ReportDiagnostic(Descriptor, node, include, node.LocalName, Ending(include));
            }
        }
    }

    private static string Ending(string include)
        => include.Contains('?') || include.Contains('*')
            ? "match any files"
            : "exist";
}
