namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class ValidatePrivateAssets() : MsBuildProjectFileAnalyzer(
    Rule.RemoveIncludeAssetsWhenRedundant,
    Rule.ExcludeCompileWhenAllAssetsArePrivate)
{
    /// <inheritdoc />
    public override bool DisableOnFailingImport => false;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var reference in context.File.ItemGroups
            .SelectMany(g => g.PackageReferences)
            .Where(r => r.PrivateAssets.IsMatch("all")))
        {
            if (reference.Element.Attribute(IncludeAssets) is { Value.Length: > 0 })
            {
                context.ReportDiagnostic(Rule.RemoveIncludeAssetsWhenRedundant, reference, IncludeAssets);
            }
            else if (reference.Element.Element(IncludeAssets) is { Value.Length: > 0 })
            {
                context.ReportDiagnostic(Rule.RemoveIncludeAssetsWhenRedundant, reference, $"<{IncludeAssets}>");
            }

            if (reference.Element.Attribute(ExcludeAssets)?.Value.Contains("compile") is not true)
            {
                context.ReportDiagnostic(Rule.ExcludeCompileWhenAllAssetsArePrivate, reference);
            }
        }
    }

    private const string IncludeAssets = nameof(IncludeAssets);

    private const string ExcludeAssets = nameof(ExcludeAssets);
}
