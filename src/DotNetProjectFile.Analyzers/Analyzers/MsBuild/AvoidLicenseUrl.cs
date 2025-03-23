namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class AvoidLicenseUrl() : MsBuildProjectFileAnalyzer(Rule.AvoidLicenseUrl)
{
    /// <inheritdoc />
    public override bool DisableOnFailingImport => false;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var url in context.File.PropertyGroups.Children<PackageLicenseUrl>())
        {
            context.ReportDiagnostic(Descriptor, url);
        }
    }
}
