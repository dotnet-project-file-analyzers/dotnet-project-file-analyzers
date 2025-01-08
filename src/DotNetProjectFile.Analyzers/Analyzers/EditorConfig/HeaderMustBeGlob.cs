using DotNetProjectFile.Ini;

namespace DotNetProjectFile.Analyzers.EditorConfig;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class HeaderMustBeGlob() : IniFileAnalyzer(Rule.Ini.HeaderMustBeGlob)
{
    protected override void Register(IniFileAnalysisContext context)
    {
        if (!context.File.Path.Name.IsMatch(".editorconfig")) return;

        foreach (var header in context.File.Syntax.Sections
            .Select(s => s.Header)
            .OfType<HeaderSyntax>()
            .Where(h => h.Text is { Length: > 0 } && Glob.TryParse(h.Text) is null))
        {
            var span = header.Tokens.First(t => t.Kind == TokenKind.HeaderTextToken);
            context.ReportDiagnostic(Descriptor, context.File, span.LinePositionSpan, header.Text);
        }
    }
}
