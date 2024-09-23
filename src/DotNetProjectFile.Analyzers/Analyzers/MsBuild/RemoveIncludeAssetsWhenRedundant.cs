namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class RemoveIncludeAssetsWhenRedundant()
    : MsBuildProjectFileAnalyzer(Rule.RemoveIncludeAssetsWhenRedundant)
{
    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var reference in context.File.ItemGroups
            .SelectMany(g => g.PackageReferences)
            .Where(r => r.PrivateAssets.IsMatch("all")))
        {
            if (reference.Element.Attribute(IncludeAssets) is { Value.Length: > 0 })
            {
                context.ReportDiagnostic(Descriptor, reference, IncludeAssets);
            }
            else if (reference.Element.Element(IncludeAssets) is { Value.Length: > 0 })
            {
                context.ReportDiagnostic(Descriptor, reference, $"<{IncludeAssets}>");
            }
        }
    }

    private const string IncludeAssets = nameof(IncludeAssets);
}
