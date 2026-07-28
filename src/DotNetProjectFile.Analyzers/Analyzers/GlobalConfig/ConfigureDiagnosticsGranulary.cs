using DotNetProjectFile.GlobalConfig;

namespace DotNetProjectFile.Analyzers.GlobalConfig;

/// <summary>
/// Implements
/// <see cref="Rule.Ini.AvoidGlobalDiagnosticSuppression"/> and
/// <see cref="Rule.Ini.AvoidGlobalDiagnosticSeverityConfiguration"/>.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class ConfigureDiagnosticsGranulary() : IniFileAnalyzer(
    Rule.Ini.AvoidGlobalDiagnosticSuppression,
    Rule.Ini.AvoidGlobalDiagnosticSeverityConfiguration)
{
    protected override void Register(IniFileAnalysisContext context)
    {
        foreach (var entry in context.File.AnalyzerDiagnosticSeverities
            .Where(e => e.DiagnosticId is not { Length: > 0 }))
        {
            if (entry.Level is DiagnosticSeverityLevel.none)
            {
                context.ReportDiagnostic(Rule.Ini.AvoidGlobalDiagnosticSuppression, context.File, entry.Entry.LinePositionSpan);
            }
            else if (entry.Level is not null)
            {
                context.ReportDiagnostic(Rule.Ini.AvoidGlobalDiagnosticSeverityConfiguration, context.File, entry.Entry.LinePositionSpan);
            }
        }
    }
}
