namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class AvoidLicenseUrl() : MsBuildProjectFileAnalyzer(Rule.AvoidLicenseUrl)
{
    /// <inheritdoc />
    public override bool DisableOnFailingImport => false;
    
    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var lincensUrl in context.File.PropertyGroups.SelectMany(g => g.PackageLicenseUrl))
        {
            context.ReportDiagnostic(Descriptor, lincensUrl);
        }
    }
}
