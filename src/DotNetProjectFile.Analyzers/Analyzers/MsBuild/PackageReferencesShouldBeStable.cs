namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class PackageReferencesShouldBeStable() : MsBuildProjectFileAnalyzer(Rule.PackageReferencesShouldBeStable)
{
    /// <inheritdoc />
    public override bool DisableOnFailingImport => false;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var package in context.File.ItemGroups.SelectMany(i => i.PackageReferences))
        {
            if (package.ResolveVersionVerbose() is not { } resolved)
            {
                continue;
            }

            if (IsUnstable(package, resolved.Version))
            {
                context.ReportDiagnostic(Descriptor, resolved.Node, package.IncludeOrUpdate, resolved.Version);
            }
        }
    }

    private static bool IsUnstable(PackageReference package, string version)
        => version.Contains('-')
        && !package.PrivateAssets.IsMatch("all");
}
