using DotNetProjectFile.Ini;

namespace DotNetProjectFile.Analyzers.Ini;

/// <summary>Implements <see cref="Rule.Ini.RemoveGlobalConfigHeader"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class RemoveGlobalConfigHeader()
    : IniFileAnalyzer(Rule.Ini.RemoveGlobalConfigHeader)
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
