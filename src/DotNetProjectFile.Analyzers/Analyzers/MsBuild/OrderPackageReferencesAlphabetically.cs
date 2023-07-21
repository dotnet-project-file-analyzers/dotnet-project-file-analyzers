using DotNetProjectFile.IO;

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
        var expectedOrder = references.OrderBy(r => r.IncludeOrUpdate, FileSystem.PathCompare);
        var firstDifference = references
            .Zip(expectedOrder, (found, expected) => (found, expected))
            .FirstOrDefault(pair => pair.found != pair.expected);

        if (firstDifference != default)
        {
            var found = firstDifference.found;
            var expected = firstDifference.expected;
            var foundName = found.Include ?? found.Update ?? string.Empty;
            var expectedName = expected.Include ?? expected.Update ?? string.Empty;
            context.ReportDiagnostic(Descriptor, expected, expectedName, foundName);
        }
    }
}
