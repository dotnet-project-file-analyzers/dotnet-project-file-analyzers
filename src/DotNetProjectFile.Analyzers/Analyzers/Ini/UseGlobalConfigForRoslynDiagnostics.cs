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
        foreach (var entry in context.File.Sections
            .SelectMany(e => e.Entries)
            .Select(e => AnalyzerDiagnosticSeverity.Create(e)?.Key)
            .OfType<IniKey>())
        {
            context.ReportDiagnostic(Descriptor, context.File, entry.LinePositionSpan, entry.Text);
        }
    }
}
