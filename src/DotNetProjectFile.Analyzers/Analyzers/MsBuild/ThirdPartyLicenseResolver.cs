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

        var queue = new Queue<Dependency>();
        var done = new HashSet<PackageVersionInfo>();

        foreach (var reference in context.File.ItemGroups.SelectMany(g => g.Children)
            .OfType<PackageReferenceBase>()
            .Where(r => r is { Info.Version.Length: > 0 } && done.Add(r.Info)))
        {
            queue.Enqueue(new(reference, reference.Info));
        }

        while (queue.Any())
        {
            var dependency = queue.Dequeue();

            if (Report(dependency, projectLicense, licenses, context) is { } package)
            {
                foreach (var transitive in package.TransativeDepedencies())
                {
                    if (done.Add(transitive))
                    {
                        queue.Enqueue(new(dependency.Node, transitive));
                    }
                }
            }
        }
    }

    private static Package? Report(Dependency dependency, LicenseExpression projectLicense, IReadOnlyCollection<ThirdPartyLicense> licenses, ProjectFileAnalysisContext context)
    {
        if (PackageCache.GetPackage(dependency.Info) is not { } package)
        {
            return null;
        }

        if (package.LicenseExpression is not { Length: > 0 } &&
            package.LicenseFile is not { Length: > 0 } &&
            package.LicenseUrl is not { Length: > 0 })
        {
            context.ReportDiagnostic(
                Rule.OnlyIncludePackagesWithExplicitLicense,
                dependency.Node,
                dependency.Info.Name,
                dependency.Info.Version,
                dependency.Format);

            return null;
        }

        if (package.UrlOnly() && package.License.IsUnknown)
        {
            context.ReportDiagnostic(
                Rule.PackageOnlyContainsDeprecatedLicenseUrl,
                dependency.Node,
                dependency.Info.Name,
                dependency.Info.Version,
                dependency.Format,
                package.LicenseUrl);
        }
        else if (package.License is CustomLicense customLicense)
        {
            if (licenses.FirstOrDefault(l => l.IsMatch(dependency.Node)) is not { } license)
            {
                context.ReportDiagnostic(
                    Rule.CustomPackageLicenseUnknown,
                    dependency.Node,
                    dependency.Info.Name,
                    customLicense.Hash);
            }
            else if (license.Hash != customLicense.Hash)
            {
                context.ReportDiagnostic(
                    Rule.CustomPackageLicenseHasChanged,
                    license,
                    dependency.Info.Name,
                    customLicense.Hash);
            }
        }
        else if (!package.License.CompatibleWith(projectLicense))
        {
            context.ReportDiagnostic(
                Rule.PackageIncompatibleWithProjectLicense,
                dependency.Node,
                dependency.Info.Name,
                dependency.Info.Version,
                dependency.Format,
                package.License,
                projectLicense);
        }

        return package;
    }

    private readonly record struct Dependency(PackageReferenceBase Node, PackageVersionInfo Info)
    {
        public bool IsTransitive => !Node.Info.Name.IsMatch(Info.Name);

        public string Format => IsTransitive ? "transitive " : string.Empty;
    }
}

file static class Extensions
{
    public static bool UrlOnly(this Package package)
        => package.LicenseExpression is not { Length: > 0 }
        && package.LicenseFile is not { Length: > 0 };

    public static IEnumerable<PackageVersionInfo> TransativeDepedencies(this Package package)
        => package.NuSpec?.Metadata?.Depedencies?
            .SelectMany(d => d.Dependencies ?? [])
            .Select(d => d.Info)
        ?? [];
}
