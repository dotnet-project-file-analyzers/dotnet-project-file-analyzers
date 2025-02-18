using DotNetProjectFile.NuGet;

namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Implements <see cref="Rule.ExcludePrivateAssetDependencies"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class ExcludePrivateAssetDependencies() : MsBuildProjectFileAnalyzer(Rule.ExcludePrivateAssetDependencies)
{
    /// <inheritdoc />
    public override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    /// <inheritdoc />
    public override bool DisableOnFailingImport => false;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var reference in context.File.ItemGroups
            .SelectMany(p => p.PackageReferences)
            .Where(ShoudBePrivateAssets))
        {
            context.ReportDiagnostic(Descriptor, reference, reference.Include);
        }
    }

    private static bool ShoudBePrivateAssets(PackageReference reference)
    {
        if (reference.PrivateAssets.IsMatch("all"))
        {
            return false;
        }

        if (PackageCache.GetPackage(reference.IncludeOrUpdate, reference.ResolveVersion()) is { } package)
        {
            return !package.HasRuntimeDll || package.IsDevelopmentDependency is true;
        }

        // No info.
        return false;
    }
}
