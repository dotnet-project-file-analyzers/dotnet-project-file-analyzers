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
            .Children<PackageReference>(ShoudBePrivateAssets))
        {
            context.ReportDiagnostic(Descriptor, reference, reference.IncludeOrUpdate);
        }
    }

    private static bool ShoudBePrivateAssets(PackageReference reference)
    {
        if (reference.PrivateAssets.IsMatch("all"))
        {
            return false;
        }

        if (reference.ResolvePackage() is { } package)
        {
            if (package.IsDevelopmentDependency is true)
            {
                // If explicitly marked as dev-dependency.
                return true;
            }
            else if (package.HasRuntimeDll)
            {
                // If not marked, and has runtime dll's.
                return false;
            }
            else if (package.HasAnalyzerDll)
            {
                // If not marked, and only has analyzer dll's.
                return true;
            }
            else if (package.HasDependencies)
            {
                // If not marked, no dll's, but has dependencies.
                return false;
            }
            else
            {
                // If no dependencies, no dependencies, then it should not have a runtime impact.
                return true;
            }
        }

        // No info.
        return false;
    }
}
