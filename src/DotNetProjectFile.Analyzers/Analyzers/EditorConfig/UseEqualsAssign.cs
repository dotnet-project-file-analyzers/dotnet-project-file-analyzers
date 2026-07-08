using DotNetProjectFile.Ini;

namespace DotNetProjectFile.Analyzers.EditorConfig;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class UseEqualsAssign() : IniFileAnalyzer(Rule.Ini.UseEqualsAssign)
{
    /// <inheritdoc />
    protected override void Register(IniFileAnalysisContext context)
    {
        if (!context.File.Path.Name.IsMatch(".editorconfig")) return;

        foreach (var colon in context.File.Tokens.Where(t => t.Kind == IniFileParser.Kind.ColonToken))
        {
            context.ReportDiagnostic(Descriptor, context.File, colon.LinePositionSpan);
        }
    }
}
