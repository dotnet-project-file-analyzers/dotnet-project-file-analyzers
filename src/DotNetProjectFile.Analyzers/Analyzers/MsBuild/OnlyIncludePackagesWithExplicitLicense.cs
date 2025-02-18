
namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Implements <see cref="Rule.OnlyIncludePackagesWithExplicitLicense"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class OnlyIncludePackagesWithExplicitLicense() : MsBuildProjectFileAnalyzer(Rule.OnlyIncludePackagesWithExplicitLicense)
{
    /// <inheritdoc />
    public override bool DisableOnFailingImport => false;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext<MsBuildProject> context)
    {
        foreach(var reference in context.File.ItemGroups.SelectMany(g => g.Children)
            .OfType<PackageReferenceBase>()
            .Where(r => r.Version is { Length: > 0 } && r.Project == context.File)
            .Where(WithoutExplicitLicense))
        {
            context.ReportDiagnostic(Descriptor, reference, reference.IncludeOrUpdate);
        }
    }

    private static bool WithoutExplicitLicense(PackageReferenceBase reference)
        => NuGet.PackageCache.GetPackage(reference.IncludeOrUpdate, reference.Version) is { } package
        && package.LicenseExpression is not { Length: > 0 }
        && package.LicenseFile is not { Length: > 0 }
        && package.LicenseUrl is not { Length: > 0 };
}
