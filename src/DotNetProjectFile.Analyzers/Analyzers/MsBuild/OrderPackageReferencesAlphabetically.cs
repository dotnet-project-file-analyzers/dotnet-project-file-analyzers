namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Implements <see cref="Rule.OrderPackageReferencesAlphabetically"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class OrderPackageReferencesAlphabetically() : MsBuildProjectFileAnalyzer(Rule.OrderPackageReferencesAlphabetically)
{
    /// <inheritdoc />
    public override bool DisableOnFailingImport => false;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var references in context.File.ItemGroups.Select(g => g.PackageReferences))
        {
            AnalyzeGroup(context, references);
        }
    }

    private void AnalyzeGroup(ProjectFileAnalysisContext context, Nodes<PackageReference> references)
        => references.CheckAlphabeticalOrder(r => r.IncludeOrUpdate, (expected, found) =>
        {
            context.ReportDiagnostic(Descriptor, expected, expected.IncludeOrUpdate, found.IncludeOrUpdate);
        });
}
