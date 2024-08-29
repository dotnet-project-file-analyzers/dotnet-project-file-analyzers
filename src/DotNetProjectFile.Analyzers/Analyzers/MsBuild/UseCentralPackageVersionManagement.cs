namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class UseCentralPackageVersionManagement() : MsBuildProjectFileAnalyzer(Rule.UseCentralPackageVersionManagement)
{
    protected override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (context.Project.ManagePackageVersionsCentrally() && context.Project.DirectoryPackagesProps is null)
        {
            context.ReportDiagnostic(Descriptor, context.Project.Positions.StartElement);
        }
    }
}
