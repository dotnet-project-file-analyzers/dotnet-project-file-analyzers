using DotNetProjectFile.GlobalConfig;
using DotNetProjectFile.Ini;

namespace DotNetProjectFile.Analyzers.GlobalConfig;

/// <summary>Implements <see cref="Rule.AvoidEnablingObsoleteRules"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class AvoidEnablingObsoleteRules() : IniFileAnalyzer(Rule.AvoidEnablingObsoleteRules)
{
    /// <inheritdoc />
    public override ImmutableArray<AnalyzerType> ApplicableTo => IniFileTypes.EditorConfig_GlobalConfig;

    /// <inheritdoc />
    protected override void Register(IniFileAnalysisContext context)
    {
        foreach (var entry in context.File.AnalyzerDiagnosticSeverities.Where(IsEnabledObsolete))
        {
            context.ReportDiagnostic(Descriptor, context.File, entry.Key.LinePositionSpan, entry.DiagnosticId);
        }
    }

    private static bool IsEnabledObsolete(AnalyzerDiagnosticSeverity entry)
        => entry.Level >= DiagnosticSeverityLevel.silent && RoslynRules.Obsolete.Contains(entry.DiagnosticId);
}
