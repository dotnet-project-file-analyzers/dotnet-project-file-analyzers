using DotNetProjectFile.Licensing;
using DotNetProjectFile.NuGet;

namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class ThirdPartyLicenseResolver() : MsBuildProjectFileAnalyzer(
    Rule.OnlyIncludePackagesWithExplicitLicense,
    Rule.PackageOnlyContainsDeprecatedLicenseUrl,
    Rule.PackageContainsIncompatibleLicense)
{
    /// <inheritdoc />
    public override bool DisableOnFailingImport => false;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        var allowed = (context.GetMsBuildProperty("AllowedLicenses") ?? string.Empty)
            .Split([',', ';'], StringSplitOptions.RemoveEmptyEntries)
            .Select(l => Licenses.FromExpression(l.Trim()))
            .ToArray();

        foreach (var reference in context.File.ItemGroups.SelectMany(g => g.Children)
            .OfType<PackageReferenceBase>()
            .Where(r => r.Version is { Length: > 0 }))
        {
            Report(reference, allowed, context);
        }
    }

    private static void Report(PackageReferenceBase reference, LicenseExpression[] allowed, ProjectFileAnalysisContext context)
    {
        if (reference.GetLicensedPackage() is not { } package)
        {
            context.ReportDiagnostic(Rule.OnlyIncludePackagesWithExplicitLicense, reference, reference.IncludeOrUpdate);
            return;
        }

        var expression = package.LicenseExpression();

        if (package.UrlOnly() && expression.IsUnknown)
        {
            context.ReportDiagnostic(Rule.PackageOnlyContainsDeprecatedLicenseUrl, reference, reference.IncludeOrUpdate);
        }
        else if (expression.IsKnown && allowed.None(l => l.CompatibleWith(expression)))
        {
            context.ReportDiagnostic(Rule.PackageContainsIncompatibleLicense, reference, reference.IncludeOrUpdate, expression);
        }
    }
}

file static class Extensions
{
    public static LicenseExpression LicenseExpression(this CachedPackage package)
    {
        var expression = Licenses.FromExpression(package.LicenseExpression);
        if (expression.IsUnknown) expression = Licenses.FromFile(package.LicenseFile);
        if (expression.IsUnknown) expression = Licenses.FromUrl(package.LicenseUrl);
        return expression;
    }

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
