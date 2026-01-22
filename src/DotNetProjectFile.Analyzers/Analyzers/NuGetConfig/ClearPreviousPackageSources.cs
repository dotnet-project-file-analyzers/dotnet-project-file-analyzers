using DotNetProjectFile.NuGet.Configuration;
using Node = DotNetProjectFile.NuGet.Configuration.Node;

namespace DotNetProjectFile.Analyzers.NuGetConfig;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class ClearPreviousPackageSources() : NuGetConfigFileAnalyzer(Rule.NuGet.ClearPreviousPackageSources)
{
    protected override void Register(NuGetConfigFileAnalysisContext context)
    {
        if (context.File.PackageSources.Any(s => s.Children.OfType<Clear>().Any())) return;

        context.ReportDiagnostic(Descriptor, (Node?)context.File.PackageSources.FirstOrDefault() ?? context.File);
    }
}
