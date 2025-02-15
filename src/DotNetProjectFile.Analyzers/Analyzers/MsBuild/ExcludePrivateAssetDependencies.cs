using DotNetProjectFile.NuGet;

namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class ExcludePrivateAssetDependencies() : MsBuildProjectFileAnalyzer(Rule.ExcludePrivateAssetDependencies)
{
    /// <inheritdoc />
    public override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

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

        if (NuGet.Packages.All.TryGet(reference.Include) is { } pkg)
        {
            return pkg.IsPrivateAsset;
        }

        if (PackageCache.GetPackage(reference.IncludeOrUpdate, reference.ResolveVersion()) is { } cpkg)
        {
            return !cpkg.HasRuntimeDll;
        }

        return false;
    }
}
