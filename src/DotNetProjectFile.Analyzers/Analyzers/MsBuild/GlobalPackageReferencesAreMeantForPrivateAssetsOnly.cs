using DotNetProjectFile.NuGet;

namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Implements <see cref="Rule.GlobalPackageReferencesAreMeantForPrivateAssetsOnly"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class GlobalPackageReferencesAreMeantForPrivateAssetsOnly()
    : MsBuildProjectFileAnalyzer(Rule.GlobalPackageReferencesAreMeantForPrivateAssetsOnly)
{
    /// <inheritdoc />
    public override bool DisableOnFailingImport => false;

    /// <inheritdoc/>
    protected override void Register(ProjectFileAnalysisContext<MsBuildProject> context)
    {
        foreach (var reference in context.File.ItemGroups
            .Children<GlobalPackageReference>(NoPrivateAsset))
        {
            context.ReportDiagnostic(Descriptor, reference, reference.IncludeOrUpdate);
        }
    }

    private bool NoPrivateAsset(GlobalPackageReference reference)
        => reference.ResolvePackage() is { } package
        && package.HasRuntimeDll
        && package.IsDevelopmentDependency is not true;
}
