using DotNetProjectFile.Ini;

namespace DotNetProjectFile.Analyzers.GlobalConfig;

/// <summary>Implements <see cref="Rule.Ini.RemoveSectionHeader"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class RemoveSectionHeader()
    : IniFileAnalyzer(Rule.Ini.RemoveSectionHeader)
{
    /// <inheritdoc />
    public override ImmutableArray<AnalyzerType> ApplicableTo => IniFileTypes.GlobalConfig;

    /// <inheritdoc />
    protected override void Register(IniFileAnalysisContext context)
    {
        foreach (var header in context.File.Sections.Select(s => s.Header).OfType<IniHeader>())
        {
            context.ReportDiagnostic(Descriptor, context.File, header.LinePositionSpan);
        }
    }
}
