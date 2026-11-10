using DotNetProjectFile.Licensing;
using DotNetProjectFile.NuGet;

namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Resolves third party licenses and validates them.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class ThirdPartyLicenseResolver() : MsBuildProjectFileAnalyzer(
    Rule.OnlyIncludePackagesWithExplicitLicense,
    Rule.PackageOnlyContainsDeprecatedLicenseUrl,
    Rule.PackageIncompatibleWithProjectLicense,
    Rule.CustomPackageLicenseUnknown,
    Rule.CustomPackageLicenseHasChanged,
    Rule.PackageCacheCouldNotBeResolved)
{
    /// <inheritdoc />
    public override ImmutableArray<AnalyzerType> ApplicableTo => ProjectFileTypes.ProjectFile;

    /// <inheritdoc />
    public override bool DisableOnFailingImport => false;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (PackageCache.GetDirectory().SubDirectories()?.None() is not false)
        {
            context.ReportDiagnostic(Rule.PackageCacheCouldNotBeResolved, context.File, PackageCache.GetDirectory());
            return;
        }

        var projectLicense = Licenses.FromExpression(context.Props.PackageLicenseExpression);
        var targetFrameworks = context.TargetFrameworks;
        var cpm = context.ManagePackageVersionsCentrally;

        var licenses = context.File.WalkBackward()
            .OfType<ThirdPartyLicense>()
            .Where(p => p.Include is { Length: > 0 })
            .ToArray();

        var requested = new HashSet<PackageVersionInfo>();
        var resolved = new HashSet<PackageVersionInfo>();
        var queue = new Queue<Dependency>(References(context.File, cpm).Where(r => requested.Add(r.Info)));

        while (queue.Count > 0)
        {
            var dependency = queue.Dequeue();

            // Different versions (ranges) can resolve to the same package: handle it only once.
            if (PackageCache.GetPackage(dependency.Info) is not { } package
                || !resolved.Add(new(package.Name, package.Version)))
            {
                continue;
            }

            if (Report(dependency, package, projectLicense, licenses, context))
            {
                foreach (var transitive in package.TransitiveDependencies(targetFrameworks).Where(requested.Add))
                {
                    queue.Enqueue(new(dependency.Node, transitive));
                }
            }
        }
    }

    /// <summary>Only the packages referenced by the project; not all &lt;PackageVersion&gt;s defined for CPM.</summary>
    /// <remarks>
    /// Reports on the reference, not on the &lt;PackageVersion&gt; that defines the version: that one is
    /// shared by all projects, so it would get a (duplicate) diagnostic per project (and target framework).
    /// </remarks>
    private static IEnumerable<Dependency> References(MsBuildProject project, bool cpm)
    {
        var lookup = new PackageVersionLookup();

        foreach (var reference in project.WalkBackward().OfType<PackageReferenceBase>().Where(r => r is not PackageVersion))
        {
            if (reference.ResolveVersion(cpm, lookup) is { } version && reference.IncludeOrUpdate is { Length: > 0 } name)
            {
                yield return new(reference, new(name, version));
            }
        }
    }

    /// <returns>True if the transitive dependencies of the package should be analyzed too.</returns>
    private static bool Report(Dependency dependency, Package package, LicenseExpression projectLicense, IReadOnlyCollection<ThirdPartyLicense> licenses, ProjectFileAnalysisContext context)
    {
        if (package.LicenseExpression is not { Length: > 0 } &&
            package.LicenseFile is not { Length: > 0 } &&
            package.LicenseUrl is not { Length: > 0 })
        {
            context.ReportDiagnostic(
                Rule.OnlyIncludePackagesWithExplicitLicense,
                dependency.Node,
                dependency.Info.Name,
                dependency.Info.Version,
                dependency.Format,
                dependency.Parent);

            return false;
        }

        if (package.UrlOnly() && package.License.IsUnknown)
        {
            context.ReportDiagnostic(
                Rule.PackageOnlyContainsDeprecatedLicenseUrl,
                dependency.Node,
                dependency.Info.Name,
                dependency.Info.Version,
                dependency.Format,
                package.LicenseUrl,
                dependency.Parent);
        }
        else if (package.License is CustomLicense customLicense)
        {
            if (licenses.FirstOrDefault(l => l.IsMatch(dependency.Info)) is not { } license)
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
                projectLicense,
                dependency.Parent);
        }

        return true;
    }

    private readonly record struct Dependency(PackageReferenceBase Node, PackageVersionInfo Info)
    {
        public bool IsTransitive => !Node.Info.Name.IsMatch(Info.Name);

        public string Format => IsTransitive ? "transitive " : string.Empty;

        public string Parent => IsTransitive ? $" in {Node.Info.Name}" : string.Empty;
    }
}

file static class Extensions
{
    public static bool UrlOnly(this Package package)
        => package.LicenseExpression is not { Length: > 0 }
        && package.LicenseFile is not { Length: > 0 };

    public static IEnumerable<PackageVersionInfo> TransitiveDependencies(this Package package, IReadOnlyCollection<string> targetFrameworks) =>
    [
        .. package.NuSpec?.Metadata?.Dependencies?.For(targetFrameworks).Where(d => d.Id is { Length: > 0 }).Select(d => d.Info) ?? [],
    ];
}
