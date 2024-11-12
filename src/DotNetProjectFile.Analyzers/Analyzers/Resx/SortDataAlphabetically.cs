namespace DotNetProjectFile.Analyzers.Resx;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class SortDataAlphabetically() : ResourceFileAnalyzer(Rule.SortDataAlphabetically)
{
    protected override void Register(ResourceFileAnalysisContext context)
        => context.File.Data.CheckAlphabeticalOrder(r => r.Name, (expected, found) =>
        {
            context.ReportDiagnostic(Descriptor, expected, expected.Name, found.Name);
        });
}
