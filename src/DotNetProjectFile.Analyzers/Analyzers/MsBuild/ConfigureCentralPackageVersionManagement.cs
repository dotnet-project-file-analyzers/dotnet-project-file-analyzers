namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class ConfigureCentralPackageVersionManagement() : MsBuildProjectFileAnalyzer(Rule.ConfigureCentralPackageVersionManagement)
{
    public override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile_DirectoryPackages;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (context.File.ManagePackageVersionsCentrally() is null)
        {
            context.ReportDiagnostic(Descriptor, context.File.Positions.StartElement);
        }
    }
}
