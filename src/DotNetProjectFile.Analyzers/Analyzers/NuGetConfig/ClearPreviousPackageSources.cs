using DotNetProjectFile.NuGet.Configuration;

namespace DotNetProjectFile.Analyzers.NuGetConfig;

/// <summary>Implements <see cref="Rule.NuGet.ClearPreviousPackageSources"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class ClearPreviousPackageSources() : NuGetConfigFileAnalyzer(Rule.NuGet.ClearPreviousPackageSources)
{
    /// <inheritdoc />
    protected override void Register(NuGetConfigFileAnalysisContext context)
    {
        if (context.File.PackageSources.Any(s => s.Children.OfType<Clear>().Any())) return;

        context.ReportDiagnostic(Descriptor, (XmlAnalysisNode?)context.File.PackageSources.FirstOrDefault() ?? context.File);
    }
}
