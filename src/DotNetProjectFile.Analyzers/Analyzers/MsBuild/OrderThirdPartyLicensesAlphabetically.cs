namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Implements <see cref="Rule.OrderThirdPartyLicensesAlphabetically"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class OrderThirdPartyLicensesAlphabetically() : MsBuildProjectFileAnalyzer(Rule.OrderThirdPartyLicensesAlphabetically)
{
    /// <inheritdoc />
    public override bool DisableOnFailingImport => false;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var references in context.File.ItemGroups.Select(g => g.ThirdPartyLicenses))
        {
            AnalyzeGroup(context, references);
        }
    }

    private void AnalyzeGroup(ProjectFileAnalysisContext context, Nodes<ThirdPartyLicense> references)
        => references.CheckAlphabeticalOrder(r => r.Include, (expected, found) =>
        {
            context.ReportDiagnostic(Descriptor, expected, expected.Include, found.Include);
        });
}
