namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class OrderPackageReferencesAlphabetically : MsBuildProjectFileAnalyzer
{
    public OrderPackageReferencesAlphabetically() : base(Rule.OrderPackageReferencesAlphabetically) { }

    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var references in context.Project
            .ImportsAndSelf()
            .SelectMany(p => p.ItemGroups)
            .Select(g => g.PackageReferences))
        {
            AnalyzeGroup(context, references);
        }
    }

    private void AnalyzeGroup(ProjectFileAnalysisContext context, Nodes<PackageReference> references)
    {
        var expectedOrder = references.OrderBy(r => r.Include);
        var firstDifference = references
            .Zip(expectedOrder, (found, expected) => (found, expected))
            .FirstOrDefault(pair => pair.found != pair.expected);

        if (firstDifference != default)
        {
            var reference = firstDifference.found;
            var name = reference.Include ?? reference.Update ?? string.Empty;
            context.ReportDiagnostic(Descriptor, reference, name);
        }
    }
}
