namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Implements <see cref="Rule.GlobalPackageReferencesAreMeantForPrivateAssetsOnly"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class GlobalPackageReferencesAreMeantForPrivateAssetsOnly()
    : MsBuildProjectFileAnalyzer(Rule.GlobalPackageReferencesAreMeantForPrivateAssetsOnly)
{
    /// <inheritdoc />
    public override bool DisableOnFailingImport => false;

    /// <inheritdoc/>
    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var reference in context.File.ItemGroups
            .Children<GlobalPackageReference>(r => NoPrivateAsset(r, context.ManagePackageVersionsCentrally)))
        {
            context.ReportDiagnostic(Descriptor, reference, reference.IncludeOrUpdate);
        }
    }

    private static bool NoPrivateAsset(GlobalPackageReference reference, bool cpmEnabled)
        => reference.ResolvePackage(cpmEnabled) is { } package
        && package.HasRuntimeDll
        && package.IsDevelopmentDependency is not true;
}
