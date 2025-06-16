using DotNetProjectFile.Ini.Legacy;

namespace DotNetProjectFile.Analyzers.EditorConfig;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class UseEqualsAssign() : IniFileAnalyzer(Rule.Ini.UseEqualsAssign)
{
    protected override void Register(IniFileAnalysisContext context)
    {
        if (!context.File.Path.Name.IsMatch(".editorconfig")) return;

        foreach (var colon in context.File.Syntax.Tokens.Where(t => t.Kind == TokenKind.ColonToken))
        {
            context.ReportDiagnostic(Descriptor, context.File, colon.LinePositionSpan);
        }
    }
}
