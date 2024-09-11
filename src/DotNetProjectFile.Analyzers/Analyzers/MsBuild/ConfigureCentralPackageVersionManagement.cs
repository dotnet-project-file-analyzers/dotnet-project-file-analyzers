namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class ConfigureCentralPackageVersionManagement() : MsBuildProjectFileAnalyzer(Rule.ConfigureCentralPackageVersionManagement)
{
    protected override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile_DirectoryPackages;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (context.Project.ManagePackageVersionsCentrally() is null)
        {
            context.ReportDiagnostic(Description, context.Project.Positions.StartElement);
        }
    }
}
