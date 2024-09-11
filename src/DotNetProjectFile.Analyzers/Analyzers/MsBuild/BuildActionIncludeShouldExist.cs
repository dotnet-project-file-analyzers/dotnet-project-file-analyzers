namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class BuildActionIncludeShouldExist() : MsBuildProjectFileAnalyzer(Rule.BuildActionIncludeShouldExist)
{
    protected override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var project in context.Project.SelfAndImports())
        {
            Report(project, context.Project.Path.Directory, context);
        }
    }

    private void Report(MsBuildProject project, IODirectory root, ProjectFileAnalysisContext context)
    {
        foreach (var node in project.ItemGroups.SelectMany(group => group.BuildActions))
        {
            foreach (var include in node.Include.Where(i => root.Files(i)?.Any() == false))
            {
                context.ReportDiagnostic(Description, node, include, node.LocalName, Ending(include));
            }
        }
    }

    private static string Ending(string include)
        => include.Contains('?') || include.Contains('*')
            ? "match any files"
            : "exist";
}
