using DotNetProjectFile.Ini;

namespace DotNetProjectFile.Analyzers.EditorConfig;

/// <summary>Implements <see cref="Rule.Ini.UseEqualsAssign"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class UseEqualsAssign() : IniFileAnalyzer(Rule.Ini.UseEqualsAssign)
{
    /// <inheritdoc />
    public override ImmutableArray<AnalyzerType> ApplicableTo => IniFileTypes.EditorConfig_GlobalConfig;

    /// <inheritdoc />
    protected override void Register(IniFileAnalysisContext context)
    {
        foreach (var colon in context.File.Tokens.Where(t => t.Kind == IniFileParser.Kind.ColonToken))
        {
            context.ReportDiagnostic(Descriptor, context.File, context.File.Spans[colon]);
        }
    }
}
