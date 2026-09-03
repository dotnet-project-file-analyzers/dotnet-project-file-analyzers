using DotNetProjectFile.GlobalConfig;
using DotNetProjectFile.Ini;

namespace DotNetProjectFile.Analyzers.GlobalConfig;

/// <summary>
/// Implements
/// <see cref="Rule.RemoveConfigurationNotConfigurableRule"/>
/// <see cref="Rule.RemoveDroppedRuleConfiguration"/>
/// .</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class RemoveIneffectiveRuleConfiguration() : IniFileAnalyzer(
    Rule.RemoveConfigurationNotConfigurableRule,
    Rule.RemoveDroppedRuleConfiguration)
{
    /// <inheritdoc />
    public override ImmutableArray<AnalyzerType> ApplicableTo => IniFileTypes.EditorConfig_GlobalConfig;

    /// <inheritdoc />
    protected override void Register(IniFileAnalysisContext context)
    {
        foreach (var entry in context.File.AnalyzerDiagnosticSeverities.Where(e => e.DiagnosticId is { Length: > 0 }))
        {
            if (RoslynRules.NotConfigurables.Contains(entry.DiagnosticId))
            {
                context.ReportDiagnostic(Rule.RemoveConfigurationNotConfigurableRule, context.File, entry.Entry.Key!.LinePositionSpan, entry.DiagnosticId);
            }
            else if (RoslynRules.Dropped.Contains(entry.DiagnosticId))
            {
                context.ReportDiagnostic(Rule.RemoveDroppedRuleConfiguration, context.File, entry.Entry.Key!.LinePositionSpan, entry.DiagnosticId);
            }
        }
    }
}
