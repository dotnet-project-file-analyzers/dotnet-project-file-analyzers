namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class PackageReferencesShouldBeStable() : MsBuildProjectFileAnalyzer(Rule.PackageReferencesShouldBeStable)
{
    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var package in context.Project.ItemGroups
            .SelectMany(i => i.PackageReferences)
            .Where(IsUnstable))
        {
            context.ReportDiagnostic(Descriptor, package, package.IncludeOrUpdate, package.ResolveVersion());
        }
    }

    private static bool IsUnstable(PackageReference package)
        => package.ResolveVersion() is { Length: > 0 } version
        && version.Contains('-')
        && !"ALL".Equals(package.PrivateAssets, StringComparison.OrdinalIgnoreCase);
}
