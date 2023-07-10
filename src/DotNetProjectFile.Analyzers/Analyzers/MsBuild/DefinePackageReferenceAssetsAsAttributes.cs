namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class DefinePackageReferenceAssetsAsAttributes : MsBuildProjectFileAnalyzer
{
    public DefinePackageReferenceAssetsAsAttributes() : base(Rule.DefinePackageReferenceAssetsAsAttributes) { }

    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var reference in context.Project.ItemGroups
            .SelectMany(g => g.PackageReferences)
            .Where(HasAssetsElement))
        {
            context.ReportDiagnostic(Descriptor, reference, reference.Include);
        }
    }

    private bool HasAssetsElement(PackageReference reference)
        => reference.Element.Elements()
        .Any(e => e.Name.LocalName.Contains("assets", StringComparison.OrdinalIgnoreCase));
}
