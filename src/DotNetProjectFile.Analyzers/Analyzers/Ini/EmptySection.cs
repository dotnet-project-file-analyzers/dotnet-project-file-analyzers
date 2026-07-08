namespace DotNetProjectFile.Analyzers.Ini;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class EmptySection() : IniFileAnalyzer(Rule.Ini.EmptySection)
{
    /// <inheritdoc />
    protected override void Register(IniFileAnalysisContext context)
    {
        foreach (var header in context.File.Sections
            .Where(s => s.Entries.None())
            .Select(s => s.Header!))
        {
            context.ReportDiagnostic(Descriptor, context.File, header.LinePositionSpan, header.Text);
        }
    }
}
