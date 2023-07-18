using ProjectReference = DotNetProjectFile.MsBuild.ProjectReference;

namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class OrderProjectReferencesAlphabetically : MsBuildProjectFileAnalyzer
{
    public OrderProjectReferencesAlphabetically() : base(Rule.OrderProjectReferencesAlphabetically) { }

    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var references in context.Project
            .ImportsAndSelf()
            .SelectMany(p => p.ItemGroups)
            .Select(g => g.ProjectReferences))
        {
            AnalyzeGroup(context, references);
        }
    }

    private void AnalyzeGroup(ProjectFileAnalysisContext context, Nodes<ProjectReference> references)
    {
        var expectedOrder = references.OrderBy(r => r.Include ?? string.Empty);
        var firstDifference = references
            .Zip(expectedOrder, (found, expected) => (found, expected))
            .FirstOrDefault(pair => pair.found != pair.expected);

        if (firstDifference != default)
        {
            var found = firstDifference.found;
            var expected = firstDifference.expected;
            var foundName = found.Include ?? string.Empty;
            var expectedName = expected.Include ?? string.Empty;
            context.ReportDiagnostic(Descriptor, expected, expectedName, foundName);
        }
    }
}
