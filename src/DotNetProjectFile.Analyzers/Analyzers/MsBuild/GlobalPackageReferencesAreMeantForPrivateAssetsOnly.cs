using DotNetProjectFile.NuGet;

namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Implements <see cref="Rule.GlobalPackageReferencesAreMeantForPrivateAssetsOnly"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class GlobalPackageReferencesAreMeantForPrivateAssetsOnly()
    : MsBuildProjectFileAnalyzer(Rule.GlobalPackageReferencesAreMeantForPrivateAssetsOnly)
{
    /// <inheritdoc/>
    protected override void Register(ProjectFileAnalysisContext<MsBuildProject> context)
    {
        foreach (var reference in context.File.ItemGroups
            .SelectMany(g => g.GlobalPackageReferences)
            .Where(NoPrivateAsset))
        {
            context.ReportDiagnostic(Descriptor, reference, reference.Include);
        }
    }

    private bool NoPrivateAsset(GlobalPackageReference reference)
    {
        if (Packages.All.TryGet(reference.Include) is { } pkg)
        {
            return !pkg.IsPrivateAsset;
        }

        if (PackageCache.GetPackage(reference.Include, reference.Version) is { } cpkg)
        {
            return cpkg.HasRuntimeDll;
        }

        // No info.
        return false;
    }
}
