using DotNetProjectFile.Ini;
using Microsoft.CodeAnalysis.Text;
using static DotNetProjectFile.Ini.IniFileParser;

namespace DotNetProjectFile.Analyzers.Ini;

/// <summary>Implements <see cref="Rule.Ini.RemoveTrailingComma"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class RemoveTrailingComma() : IniFileAnalyzer(Rule.Ini.RemoveTrailingComma)
{
    /// <inheritdoc />
    public override ImmutableArray<AnalyzerType> ApplicableTo => IniFileTypes.All;

    /// <inheritdoc />
    protected override void Register(IniFileAnalysisContext context)
    {
        foreach (var value in context.File.Entries.Select(entry => entry.Value).OfType<IniValue>().Where(EndsWithComma))
        {
            if (value.Tokens.FirstOrNone(token => token.Kind is Kind.ValueToken) is { } valueToken)
            {
                context.ReportDiagnostic(Descriptor, context.File, context.File.Spans[new TextSpan(valueToken.End - 1, 1)]);
            }
        }
    }

    private static bool EndsWithComma(IniValue value)
        => value.Text is { Length: > 1 } text && text[^1] is ',';
}
