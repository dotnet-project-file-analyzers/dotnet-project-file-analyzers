namespace DotNetProjectFile.Analyzers.Ini;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class IniSyntaxAnalyzer() : IniFileAnalyzer(
    Rule.Ini.Invalid,
    Rule.Ini.InvalidHeader,
    Rule.Ini.InvalidKeyValuePair)
{
    protected override void Register(IniFileAnalysisContext context)
    {
        foreach (var diagnostic in context.File.Syntax.GetDiagnostics())
        {
            context.ReportDiagnostic(diagnostic);
        }
    }
}
