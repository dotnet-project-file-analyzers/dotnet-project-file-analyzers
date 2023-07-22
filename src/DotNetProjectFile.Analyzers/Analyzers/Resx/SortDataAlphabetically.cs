namespace DotNetProjectFile.Analyzers.Resx;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class SortDataAlphabetically : ResourceFileAnalyzer
{
    public SortDataAlphabetically() : base(Rule.SortDataAlphabetically) { }

    protected override void Register(ResourceFileAnalysisContext context)
    {
        var expectedOrder = context.Resource.Data.OrderBy(r => r.Name);
        var firstDifference = context.Resource.Data
            .Zip(expectedOrder, (found, expected) => (found, expected))
            .FirstOrDefault(pair => pair.found != pair.expected);

        if (firstDifference != default)
        {
            var found = firstDifference.found;
            var expected = firstDifference.expected;
            var foundName = found.Name ?? string.Empty;
            var expectedName = expected.Name ?? string.Empty;
            context.ReportDiagnostic(Descriptor, expected, expectedName, foundName);
        }
    }
}
