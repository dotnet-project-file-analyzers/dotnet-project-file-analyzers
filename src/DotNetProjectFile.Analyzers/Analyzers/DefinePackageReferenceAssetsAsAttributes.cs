namespace DotNetProjectFile.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class DefinePackageReferenceAssetsAsAttributes : ProjectFileAnalyzer
{
    public DefinePackageReferenceAssetsAsAttributes() : base(Rule.DefinePackageReferenceAssetsAsAttributes) { }

    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var reference in context.Project.AncestorsAndSelf()
            .SelectMany(p => p.ItemGroups)
            .SelectMany(g => g.PackageReferences)
            .Where(HasAssetsElement))
        {
            context.ReportDiagnostic(Descriptor, reference.Location, reference.Include);
        }
    }

    private bool HasAssetsElement(PackageReference reference)
        => reference.Element.Elements()
        .Any(e => e.Name.LocalName.Contains("assets", StringComparison.OrdinalIgnoreCase));
}
