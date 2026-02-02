using DotNetProjectFile.NuGet.Configuration;

namespace DotNetProjectFile.Analyzers.NuGetConfig;

/// <summary>Implements
/// <see cref="Rule.NuGet.DefineMappingForMultipleSources"/>,
/// <see cref="Rule.NuGet.PackageSourceMappingsShouldBeUnique"/>,
/// <see cref="Rule.NuGet.LastMappingCatchesAll"/>.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class HandlePackageSourceMappings() : NuGetConfigFileAnalyzer(
    Rule.NuGet.DefineMappingForMultipleSources,
    Rule.NuGet.PackageSourceMappingsShouldBeUnique,
    Rule.NuGet.LastMappingCatchesAll)
{
    /// <inheritdoc />
    protected override void Register(NuGetConfigFileAnalysisContext context)
    {
        var keys = new Dictionary<string, Add>();
        var patterns = new HashSet<string>();
        NuGet.Configuration.Node last = context.File;

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
                if (package.Pattern is { Length: > 0 } pattern && !patterns.Add(pattern))
                {
                    context.ReportDiagnostic(Rule.NuGet.PackageSourceMappingsShouldBeUnique, package, pattern);
                }
                last = package;
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

        if (LacksCatchAllMapping(keysCount, last))
        {
            context.ReportDiagnostic(Rule.NuGet.LastMappingCatchesAll, last);
        }
    }

    private static bool LacksCatchAllMapping(int keysCount, NuGet.Configuration.Node last)
    {
        // no package specified and single (or none) package sources specfied.
        if (keysCount <= 1 && last is NuGetConfigFile) return false;

        // Given that ther is a Package source it should contain 1 and it should be *.
        return !(last.Parent is PackageSource package
            && package.Packages.Count is 1
            && last is Package { Pattern: "*" });
    }
}
