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
            context.ReportDiagnostic(Descriptor, context.File, context.File.SdkNode?.Span ?? context.File.Spans[context.File.TextSpan]);
        }
        else if (node is not JsonString value
            || !value.Text.All(char.IsLetter)
            || !Enum.TryParse<RollForwardPolicy>(value.Text, ignoreCase: true, out var policy)
            || policy is RollForwardPolicy.None)
        {
            context.ReportDiagnostic(Descriptor, context.File, node);
        }
        else if (policy is not RollForwardPolicy.Disable && context.Props.RestorePackagesWithLockFile is true)
        {
            context.ReportDiagnostic(Rule.Json.DisableSdkRollForwardWhenLocked, context.File, node);
        }
    }
}
