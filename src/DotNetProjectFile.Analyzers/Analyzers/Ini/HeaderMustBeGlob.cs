using DotNetProjectFile.Ini;
using Grammr;

namespace DotNetProjectFile.Analyzers.EditorConfig;

/// <summary>Implements <see cref="Rule.Ini.HeaderMustBeGlob"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class HeaderMustBeGlob() : IniFileAnalyzer(Rule.Ini.HeaderMustBeGlob)
{
    /// <inheritdoc />
    public override ImmutableArray<AnalyzerType> ApplicableTo => IniFileTypes.EditorConfig;

    /// <inheritdoc />
    protected override void Register(IniFileAnalysisContext context)
    {
        foreach (var header in context.File.Sections
            .Select(s => s.Header)
            .OfType<IniHeader>()
            .Where(h => h.Text is { Length: > 0 } && Glob.TryParse(h.Text) is null))
        {
            var span = header.Tokens.OfKind(IniFileParser.Kind.HeaderText);
            context.ReportDiagnostic(Descriptor, context.File, header.Spans[span], header.Text);
        }
    }
}
