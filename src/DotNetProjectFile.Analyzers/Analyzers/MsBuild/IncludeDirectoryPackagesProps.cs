namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class IncludeDirectoryPackagesProps() : MsBuildProjectFileAnalyzer(Rule.IncludeDirectoryPackagesProps)
{
    protected override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (context.Project.ManagePackageVersionsCentrally().GetValueOrDefault()
            && context.Project.DirectoryPackagesProps is null)
        {
            context.ReportDiagnostic(Descriptor, context.Project.Positions.StartElement);
        }
    }
}
