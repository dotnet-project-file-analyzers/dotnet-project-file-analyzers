using DotNetProjectFile.NuGet.Configuration;

namespace DotNetProjectFile.Analyzers.NuGetConfig;

/// <summary>Implements <see cref="Rule.NuGet.DefineMappingForMultipleSources"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class DefineMappingForMultipleSources() : NuGetConfigFileAnalyzer(Rule.NuGet.DefineMappingForMultipleSources)
{
    /// <inheritdoc />
    protected override void Register(NuGetConfigFileAnalysisContext context)
    {
        var keys = new Dictionary<string, Add>();

        foreach (var add in context.File.PackageSources.Children<Add>())
        {
            if (add.Key is { Length: > 0 } key)
            {
                keys[key] = add;
            }
        }

        if (keys.Count <= 1) return;

        foreach (var source in context.File.PackageSourceMappings.Children<PackageSource>().Where(WithDefinedMappings))
        {
            keys.Remove(source.Key!);
        }

        foreach (var kvp in keys)
        {
            context.ReportDiagnostic(Descriptor, kvp.Value, kvp.Key);
        }
    }

    private static bool WithDefinedMappings(PackageSource source) => source.Key is { Length: > 0 } && source.Packages.Any();
}
