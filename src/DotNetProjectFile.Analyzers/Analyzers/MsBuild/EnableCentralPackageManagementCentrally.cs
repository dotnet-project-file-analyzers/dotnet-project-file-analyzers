namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class EnableCentralPackageManagementCentrally()
    : MsBuildProjectFileAnalyzer(Rule.EnableCentralPackageManagementCentrally)
{
    /// <inheritdoc />
    public override ImmutableArray<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var enabled in context.File.PropertyGroups
            .Children<ManagePackageVersionsCentrally>(p => p.Value is true))
        {
            context.ReportDiagnostic(Descriptor, enabled);
        }
    }
}
