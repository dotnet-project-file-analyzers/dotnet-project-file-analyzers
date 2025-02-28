using DotNetProjectFile.Licensing;
using DotNetProjectFile.NuGet;

namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class ThirdPartyLicenseResolver() : MsBuildProjectFileAnalyzer(
    Rule.OnlyIncludePackagesWithExplicitLicense,
    Rule.PackageOnlyContainsDeprecatedLicenseUrl,
    Rule.PackageIncompatibleWithProjectLicense,
    Rule.CustomPackageLicenseUnknown,
    Rule.CustomPackageLicenseHasChanged)
{
    /// <inheritdoc />
    public override bool DisableOnFailingImport => false;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        var projectLicense = Licenses.FromExpression(context.GetMsBuildProperty("PackageLicenseExpression"));

        var licenses = context.File.WalkBackward()
            .OfType<ThirdPartyLicense>()
            .Where(p => p.Include is { Length: > 0 })
            .ToArray();

        foreach (var reference in context.File.ItemGroups.SelectMany(g => g.Children)
            .OfType<PackageReferenceBase>()
            .Where(r => r.Version is { Length: > 0 }))
        {
            Report(reference, projectLicense, licenses, context);
        }
    }

    private static void Report(PackageReferenceBase reference, LicenseExpression projectLicense, IReadOnlyCollection<ThirdPartyLicense> licenses, ProjectFileAnalysisContext context)
    {
        if (reference.GetLicensedPackage() is not { } package)
        {
            context.ReportDiagnostic(Rule.OnlyIncludePackagesWithExplicitLicense, reference, reference.IncludeOrUpdate);
            return;
        }

        var packageLicense = package.License;

        if (package.UrlOnly() && packageLicense.IsUnknown)
        {
            context.ReportDiagnostic(Rule.PackageOnlyContainsDeprecatedLicenseUrl, reference, reference.IncludeOrUpdate);
        }
        else if (packageLicense is CustomLicense customLicense)
        {
            if (licenses.FirstOrDefault(l => l.IsMatch(reference)) is not { } license)
            {
                context.ReportDiagnostic(Rule.CustomPackageLicenseUnknown, reference, reference.Include, customLicense.Hash);
            }
            else if (license.Hash != customLicense.Hash)
            {
                context.ReportDiagnostic(Rule.CustomPackageLicenseHasChanged, license, reference.Include, customLicense.Hash);
            }
        }
        else if (!packageLicense.CompatibleWith(projectLicense))
        {
            context.ReportDiagnostic(Rule.PackageIncompatibleWithProjectLicense, reference, reference.IncludeOrUpdate, packageLicense, projectLicense);
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
