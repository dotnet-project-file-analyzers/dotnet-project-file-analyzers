using DotNetProjectFile.NuGet.Configuration;

namespace DotNetProjectFile.Analyzers.NuGetConfig;

/// <summary>Implements <see cref="Rule.NuGet.PreferHttpsSources"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class PreferHttpsSources() : NuGetConfigFileAnalyzer(Rule.NuGet.PreferHttpsSources)
{
    /// <inheritdoc />
    protected override void Register(NuGetConfigFileAnalysisContext context)
    {
        foreach (var add in context.File.PackageSources.Children<Add>(StartsWithHttp))
        {
            context.ReportDiagnostic(Descriptor, add);
        }
    }

    private static bool StartsWithHttp(Add add) => (add.Value ?? string.Empty).StartsWith("http://");
}
