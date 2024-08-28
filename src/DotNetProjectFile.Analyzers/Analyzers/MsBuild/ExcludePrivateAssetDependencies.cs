namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class ExcludePrivateAssetDependencies() : MsBuildProjectFileAnalyzer(Rule.ExcludePrivateAssetDependencies)
{
    protected override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var reference in context.Project.ItemGroups
            .SelectMany(p => p.PackageReferences)
            .Where(ShoudBePrivateAssets))
        {
            context.ReportDiagnostic(Descriptor, reference, reference.Include);
        }
    }

    private static bool ShoudBePrivateAssets(PackageReference reference)
        => !string.Equals(reference.PrivateAssets, "all", StringComparison.OrdinalIgnoreCase)
        && NuGet.Packages.All.TryGet(reference.Include) is { IsPrivateAsset: true };
}
