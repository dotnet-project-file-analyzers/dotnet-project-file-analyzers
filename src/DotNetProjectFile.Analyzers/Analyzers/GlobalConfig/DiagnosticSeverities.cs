using DotNetProjectFile.GlobalConfig;
using DotNetProjectFile.Ini;

namespace DotNetProjectFile.Analyzers.GlobalConfig;

/// <summary>Implements <see cref="Rule.Ini.SpecifyIsGlobal"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class DiagnosticSeverities() : IniFileAnalyzer(
    Rule.Ini.UseValidSeverityLevel,
    Rule.Ini.UseExplicitSeverityLevel)
{
    /// <inheritdoc />
    public override ImmutableArray<AnalyzerType> ApplicableTo => IniFileTypes.EditorConfig_GlobalConfig;

    /// <inheritdoc />
    protected override void Register(IniFileAnalysisContext context)
    {
        foreach (var entry in context.File.AnalyzerDiagnosticSeverities
            .Where(e => e.Value is { Length: > 0 }))
        {
            if (entry.Level is DiagnosticSeverityLevel.@default)
            {
                context.ReportDiagnostic(Rule.Ini.UseExplicitSeverityLevel, context.File, entry.Entry.Value!.LinePositionSpan);
            }
            else if (entry.Level is null)
            {
                context.ReportDiagnostic(Rule.Ini.UseValidSeverityLevel, context.File, entry.Entry.Value!.LinePositionSpan, entry.Value);
            }
        }
    }
}
