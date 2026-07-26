using DotNetProjectFile.GlobalConfig;
using DotNetProjectFile.Ini;

namespace DotNetProjectFile.Analyzers.Ini;

/// <summary>Implements <see cref="Rule.Ini.EmptySection"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class UseGlobalConfigForRoslynDiagnostics()
    : IniFileAnalyzer(Rule.Ini.UseGlobalConfigForRoslynDiagnostics)
{
    /// <inheritdoc />
    public override ImmutableArray<AnalyzerType> ApplicableTo => IniFileTypes.EditorConfig;

    /// <inheritdoc />
    protected override void Register(IniFileAnalysisContext context)
    {
        foreach (var key in context.File.AnalyzerDiagnosticSeverities.Select(e => e.Key))
        {
            context.ReportDiagnostic(Descriptor, context.File, key.LinePositionSpan, key.Text);
        }
    }
}
