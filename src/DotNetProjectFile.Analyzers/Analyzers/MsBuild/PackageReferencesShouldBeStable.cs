namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class PackageReferencesShouldBeStable() : MsBuildProjectFileAnalyzer(Rule.PackageReferencesShouldBeStable)
{
    /// <inheritdoc />
    public override bool DisableOnFailingImport => false;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        var cpm = context.ManagePackageVersionsCentrally;
        var lookup = new PackageVersionLookup();

        foreach (var package in context.File.ItemGroups.Children<PackageReferenceBase>())
        {
            if (package.ResolveVersionVerbose(cpm, lookup) is { } resolved
                && resolved.Version.Contains('-'))
            {
                context.ReportDiagnostic(Descriptor, resolved.Node, package.IncludeOrUpdate, resolved.Version);
            }
        }
    }
}
