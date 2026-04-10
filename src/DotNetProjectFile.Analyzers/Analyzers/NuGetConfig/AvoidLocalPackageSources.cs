using DotNetProjectFile.NuGet.Configuration;

namespace DotNetProjectFile.Analyzers.NuGetConfig;

/// <summary>Implements <see cref="Rule.NuGet.AvoidLocalPackageSources"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class AvoidLocalPackageSources() : NuGetConfigFileAnalyzer(Rule.NuGet.AvoidLocalPackageSources)
{
    /// <inheritdoc />
    protected override void Register(NuGetConfigFileAnalysisContext context)
    {
        foreach (var add in context.File.PackageSources.Children<Add>(IsLocal))
        {
            context.ReportDiagnostic(Descriptor, add);
        }
    }

    private static bool IsLocal(Add add)
        => add.Value is { Length: > 0 } value
        && !value.StartsWith("http");
}
