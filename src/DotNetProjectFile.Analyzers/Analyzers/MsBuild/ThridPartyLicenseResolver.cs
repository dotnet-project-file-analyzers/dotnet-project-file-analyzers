
using DotNetProjectFile.Licensing;
using DotNetProjectFile.NuGet;

namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Implements <see cref="Rule.OnlyIncludePackagesWithExplicitLicense"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class ThridPartyLicenseResolver() : MsBuildProjectFileAnalyzer(
    Rule.OnlyIncludePackagesWithExplicitLicense,
    Rule.PackageOnlyContainsDeprecatedLicenseUrl)
{
    /// <inheritdoc />
    public override bool DisableOnFailingImport => false;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext<MsBuildProject> context)
    {
        foreach (var reference in context.File.ItemGroups.SelectMany(g => g.Children)
            .OfType<PackageReferenceBase>()
            .Where(r => r.Version is { Length: > 0 }))
        {
            if (reference.GetLicensedPackage() is not { } package)
            {
                context.ReportDiagnostic(Rule.OnlyIncludePackagesWithExplicitLicense, reference, reference.IncludeOrUpdate);
            }
            else if (package.UrlOnly() && Licenses.FromUrl(package.LicenseUrl) == Licenses.Unknown)
            {
                context.ReportDiagnostic(Rule.PackageOnlyContainsDeprecatedLicenseUrl, reference, reference.IncludeOrUpdate);
            }
        }
    }
}

file static class Extensions
{
    public static CachedPackage? GetLicensedPackage(this PackageReferenceBase reference)
       => NuGet.PackageCache.GetPackage(reference.IncludeOrUpdate, reference.Version) is { } package
       && (package.LicenseExpression is { Length: > 0 }
       || package.LicenseFile is { Length: > 0 }
       || package.LicenseUrl is { Length: > 0 })
       ? package
       : null;

    public static bool UrlOnly(this CachedPackage package)
        => package.LicenseExpression is not { Length: > 0 }
        && package.LicenseFile is not { Length: > 0 };
}
