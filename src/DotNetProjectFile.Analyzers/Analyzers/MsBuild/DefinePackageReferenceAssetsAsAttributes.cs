namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class DefinePackageReferenceAssetsAsAttributes() : MsBuildProjectFileAnalyzer(Rule.DefinePackageReferenceAssetsAsAttributes)
{
    /// <inheritdoc />
    public override bool DisableOnFailingImport => false;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var reference in context.File.ItemGroups
            .Children<PackageReference>(HasAssetsElement))
        {
            context.ReportDiagnostic(Descriptor, reference, reference.Include);
        }
    }

    private static bool HasAssetsElement(PackageReference reference)
        => reference.Element.Elements()
        .Any(e => e.Name.LocalName.Contains("assets", StringComparison.OrdinalIgnoreCase));
}
