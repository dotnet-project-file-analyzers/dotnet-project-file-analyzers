namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class EnableCentralPackageManagementCentrally()
    : MsBuildProjectFileAnalyzer(Rule.EnableCentralPackageManagementCentrally)
{
    protected override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var enabled in context.Project.PropertyGroups
            .SelectMany(g => g.ManagePackageVersionsCentrally)
            .Where(p => p.Value == true))
        {
            context.ReportDiagnostic(Descriptor, enabled);
        }
    }
}
