namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class OrderPackageVersionsAlphabetically() : MsBuildProjectFileAnalyzer(Rule.OrderPackageVersionsAlphabetically)
{
    /// <inheritdoc />
    public override bool DisableOnFailingImport => false;
    
    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var references in context.File.ItemGroups.Select(g => g.PackageVersions))
        {
            AnalyzeGroup(context, references);
        }
    }

    private void AnalyzeGroup(ProjectFileAnalysisContext context, Nodes<PackageVersion> references)
        => references.CheckAlphabeticalOrder(r => r.IncludeOrUpdate, (expected, found) =>
        {
            context.ReportDiagnostic(Descriptor, expected, expected.IncludeOrUpdate, found.IncludeOrUpdate);
        });
}
