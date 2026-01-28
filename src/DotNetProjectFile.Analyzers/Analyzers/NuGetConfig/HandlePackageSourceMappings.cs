using DotNetProjectFile.NuGet.Configuration;

namespace DotNetProjectFile.Analyzers.NuGetConfig;

/// <summary>Implements
/// <see cref="Rule.NuGet.DefineMappingForMultipleSources"/>,
/// <see cref="Rule.NuGet.PackageSourceMappingsShouldBeUnique"/>.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class HandlePackageSourceMappings() : NuGetConfigFileAnalyzer(
    Rule.NuGet.DefineMappingForMultipleSources,
    Rule.NuGet.PackageSourceMappingsShouldBeUnique)
{
    /// <inheritdoc />
    protected override void Register(NuGetConfigFileAnalysisContext context)
    {
        var keys = new Dictionary<string, Add>();
        var patterns = new HashSet<string>();

        foreach (var add in context.File.PackageSources.Children<Add>())
        {
            if (add.Key is { Length: > 0 } key)
            {
                keys[key] = add;
            }
        }

        var keysCount = keys.Count;

        foreach (var source in context.File.PackageSourceMappings.Children<PackageSource>())
        {
            foreach (var package in source.Packages)
            {
                if (package.Pattern is { Length: > 0} pattern && !patterns.Add(pattern))
                {
                    context.ReportDiagnostic(Rule.NuGet.PackageSourceMappingsShouldBeUnique, package, pattern);
                }
            }
            if (source.Key is { Length: > 0 } key)
            {
                keys.Remove(key);
            }
        }

        if (keysCount >= 2)
        {
            foreach (var kvp in keys)
            {
                context.ReportDiagnostic(Rule.NuGet.DefineMappingForMultipleSources, kvp.Value, kvp.Key);
            }
        }
    }
}
