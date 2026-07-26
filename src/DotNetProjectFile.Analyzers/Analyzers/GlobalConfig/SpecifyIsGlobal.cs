using DotNetProjectFile.Ini;

namespace DotNetProjectFile.Analyzers.GlobalConfig;

/// <summary>Implements <see cref="Rule.Ini.SpecifyIsGlobal"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class SpecifyIsGlobal()
    : IniFileAnalyzer(Rule.Ini.SpecifyIsGlobal)
{
    /// <inheritdoc />
    public override ImmutableArray<AnalyzerType> ApplicableTo => IniFileTypes.GlobalConfig;

    /// <inheritdoc />
    protected override void Register(IniFileAnalysisContext context)
    {
        var any = false;
        foreach (var entry in context.File.Entries.Where(e => e.Key?.Text.IsMatch("is_global") is true))
        {
            any = true;

            _ = bool.TryParse(entry.Value?.Text, out var enabled);
            if (!enabled)
            {
                context.ReportDiagnostic(Descriptor, context.File, entry.Value?.LinePositionSpan ?? entry.LinePositionSpan, "Enable");
            }
        }

        if (!any)
        {
            context.ReportDiagnostic(Descriptor, context.File, context.File.Spans[context.File.Stream[0]], "Set");
        }
    }
}
