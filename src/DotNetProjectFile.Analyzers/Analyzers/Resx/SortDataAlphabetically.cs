namespace DotNetProjectFile.Analyzers.Resx;

/// <summary>Implements <see cref="Rule.RESX.SortDataAlphabetically"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class SortDataAlphabetically() : ResourceFileAnalyzer(Rule.RESX.SortDataAlphabetically)
{
    /// <inheritdoc />
    protected override void Register(ResourceFileAnalysisContext context)
        => context.File.Data.CheckAlphabeticalOrder(r => r.Name, (expected, found) =>
        {
            context.ReportDiagnostic(Descriptor, expected, expected.Name, found.Name);
        });
}
