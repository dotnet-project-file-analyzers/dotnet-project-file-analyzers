using DotNetProjectFile.GlobalConfig;
using DotNetProjectFile.Ini;

namespace DotNetProjectFile.Analyzers.GlobalConfig;

/// <summary>Implements <see cref="Rule.RemoveIneffectiveRuleConfiguration"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class RemoveIneffectiveRuleConfiguration() : IniFileAnalyzer(Rule.RemoveIneffectiveRuleConfiguration)
{
    /// <inheritdoc />
    public override ImmutableArray<AnalyzerType> ApplicableTo => IniFileTypes.EditorConfig_GlobalConfig;

    /// <inheritdoc />
    protected override void Register(IniFileAnalysisContext context)
    {
        foreach (var entry in context.File.AnalyzerDiagnosticSeverities
            .Where(e => e.DiagnosticId is { Length: > 0 } id && RoslynRules.NotConfigurables.Contains(id)))
        {
            context.ReportDiagnostic(Descriptor, context.File, entry.Entry.Key!.LinePositionSpan, entry.DiagnosticId);
        }
    }
}
