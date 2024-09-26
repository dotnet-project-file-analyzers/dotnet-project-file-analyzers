namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class BuildActionIncludeShouldExist() : MsBuildProjectFileAnalyzer(Rule.BuildActionIncludeShouldExist)
{
    protected override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        var root = context.File.Path.Directory;

        foreach (var node in context.File.Walk().OfType<BuildAction>())
        {
            foreach (var include in node.Include.Where(i => root.Files(i)?.Any() == false))
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
