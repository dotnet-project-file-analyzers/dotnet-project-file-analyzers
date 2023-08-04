namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class PrivateAsset : MsBuildProjectFileAnalyzer
{
    public PrivateAsset() : base(Rule.SomeRule) { }

    protected override bool ApplyToProps => false;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var reference in context.Project.ItemGroups.SelectMany(p => p.PackageReferences).Where(ShoudBePrivateAssets))
        {
            context.ReportDiagnostic(Descriptor, reference);
        }
    }

    private static bool ShoudBePrivateAssets(PackageReference reference)
        => reference.PrivateAssets != "all"
        && NuGet.Packages.All.TryGet(reference.Include) is { IsPrivateAsset: true };
}
