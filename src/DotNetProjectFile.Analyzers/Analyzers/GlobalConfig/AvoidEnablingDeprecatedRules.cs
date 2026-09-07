using DotNetProjectFile.GlobalConfig;
using DotNetProjectFile.Ini;

namespace DotNetProjectFile.Analyzers.GlobalConfig;

/// <summary>Implements <see cref="Rule.AvoidEnablingDeprecatedRules"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class AvoidEnablingDeprecatedRules() : IniFileAnalyzer(Rule.AvoidEnablingDeprecatedRules)
{
    /// <inheritdoc />
    public override ImmutableArray<AnalyzerType> ApplicableTo => IniFileTypes.EditorConfig_GlobalConfig;

    /// <inheritdoc />
    protected override void Register(IniFileAnalysisContext context)
    {
        foreach (var entry in context.File.AnalyzerDiagnosticSeverities.Where(IsEnabledOrDeprecated))
        {
            context.ReportDiagnostic(Descriptor, context.File, entry.Key.LinePositionSpan, entry.DiagnosticId);
        }
    }

    private static bool IsEnabledOrDeprecated(AnalyzerDiagnosticSeverity entry)
        => entry.Level >= DiagnosticSeverityLevel.silent && RoslynRules.Deprecated.Contains(entry.DiagnosticId);
}
