namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class BuildActionIncludeShouldExist() : MsBuildProjectFileAnalyzer(Rule.BuildActionIncludeShouldExist)
{
    protected override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        // Paths in imports are based in the location of the project file.
        foreach (var project in context.Project.SelfAndImports().Where(p => ProjectFileTypes.ProjectFile_Props.Contains(p.FileType)))
        {
            Report(project, context.Project.Path.Directory, context);
        }

        // Paths in directory.build.props are relative to the build.props themselves.
        if (context.Project.DirectoryBuildProps is { } build)
        {
            Report(build, build.Path.Directory, context);
        }
    }

    private void Report(MsBuildProject project, IODirectory root, ProjectFileAnalysisContext context)
    {
        foreach (var node in project.ItemGroups.SelectMany(group => group.BuildActions))
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
