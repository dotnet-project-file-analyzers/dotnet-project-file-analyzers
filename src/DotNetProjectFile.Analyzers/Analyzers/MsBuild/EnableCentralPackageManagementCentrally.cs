namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class EnableCentralPackageManagementCentrally()
    : MsBuildProjectFileAnalyzer(Rule.EnableCentralPackageManagementCentrally)
{
    /// <inheritdoc />
    public override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var enabled in context.File.PropertyGroups
            .SelectMany(g => g.ManagePackageVersionsCentrally)
            .Where(p => p.Value == true))
        {
            context.ReportDiagnostic(Descriptor, enabled);
        }
    }
}
