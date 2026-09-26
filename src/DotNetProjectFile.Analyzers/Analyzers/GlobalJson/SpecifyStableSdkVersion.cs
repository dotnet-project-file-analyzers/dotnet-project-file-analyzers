using DotNetProjectFile.GlobalJson;
using DotNetProjectFile.Json;

namespace DotNetProjectFile.Analyzers.GlobalJson;

/// <summary>Implements <see cref="Rule.Json.SpecifyStableSdkVersion"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class SpecifyStableSdkVersion() : JsonFileAnalyzer(Rule.Json.SpecifyStableSdkVersion)
{
    /// <inheritdoc />
    public override ImmutableArray<AnalyzerType> ApplicableTo => JsonFileTypes.GlobalJson;

    /// <inheritdoc />
    protected override void Register(JsonFileAnalysisContext context)
    {
        if (context.File.SdkNode?.Version is JsonString version
            && SemVer.TryParse(version.Text, trim: false) is { PreRelease.Length: > 0 })
        {
            context.ReportDiagnostic(Descriptor, context.File, version);
        }
        if (context.File.SdkNode?.AllowPrerelease is JsonTrue prerelease)
        {
            context.ReportDiagnostic(Descriptor, context.File, prerelease);
        }
    }
}
