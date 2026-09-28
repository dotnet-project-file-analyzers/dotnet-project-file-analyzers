using DotNetProjectFile.GlobalJson;
using DotNetProjectFile.Json;

namespace DotNetProjectFile.Analyzers.GlobalJson;

/// <summary>Implements <see cref="Rule.Json.SpecifySdkRollForwardPolicy"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class SpecifySdkRollForwardPolicy() : JsonFileAnalyzer(
    Rule.Json.SpecifySdkRollForwardPolicy,
    Rule.Json.DisableSdkRollForwardWhenLocked)
{
    /// <inheritdoc />
    public override ImmutableArray<AnalyzerType> ApplicableTo => JsonFileTypes.GlobalJson;

    /// <inheritdoc />
    protected override void Register(JsonFileAnalysisContext context)
    {
        if (context.File.SdkNode?.RollForward is not { } node)
        {
            context.ReportDiagnostic(Rule.Json.SpecifySdkRollForwardPolicy, context.File, context.File.SdkNode?.Span ?? context.File.Spans[context.File.TextSpan]);
        }
        else if (context.File.SdkNode?.RollForwardPolicy is null or RollForwardPolicy.None)
        {
            context.ReportDiagnostic(Rule.Json.SpecifySdkRollForwardPolicy, context.File, node);
        }
        else if (context.File.SdkNode?.RollForwardPolicy is not RollForwardPolicy.Disable && context.Props.RestorePackagesWithLockFile is true)
        {
            context.ReportDiagnostic(Rule.Json.DisableSdkRollForwardWhenLocked, context.File, node);
        }
    }
}
