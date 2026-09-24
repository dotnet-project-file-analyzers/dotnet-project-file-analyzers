using DotNetProjectFile.GlobalJson;
using DotNetProjectFile.Json;

namespace DotNetProjectFile.Analyzers.GlobalJson;

/// <summary>Implements <see cref="Rule.Json.SpecifySdkVersion"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class SpecifySdkVersion() : JsonFileAnalyzer(Rule.Json.SpecifySdkVersion)
{
    /// <inheritdoc />
    public override ImmutableArray<AnalyzerType> ApplicableTo => JsonFileTypes.GlobalJson;

    /// <inheritdoc />
    protected override void Register(JsonFileAnalysisContext context)
    {
        if (context.File.SdkNode?.Version is not { } node)
        {
            context.ReportDiagnostic(Descriptor, context.File, context.File.SdkNode?.Span ?? context.File.Spans[context.File.TextSpan]);
        }
        else if (node is not JsonString value || SemVer.TryParse(value.Text, trim: false) is null)
        {
            context.ReportDiagnostic(Descriptor, context.File, node);
        }
    }
}
