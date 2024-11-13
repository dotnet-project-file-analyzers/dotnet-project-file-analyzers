namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class VersionOverrideShouldChangeVersion()
    : MsBuildProjectFileAnalyzer(Rule.VersionOverrideShouldChangeVersion)
{
    public override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile_Props;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (context.File.ManagePackageVersionsCentrally() is not true) return;

        foreach (var @override in context.File.ItemGroups
            .SelectMany(g => g.PackageReferences)
            .Where(r => r.VersionOverride is { Length: > 0 }))
        {
            if (CpmVersion(context, @override) is { } cpm
                && cpm.Version == @override.VersionOverride)
            {
                context.ReportDiagnostic(Descriptor, @override);
            }
        }
    }

    private static PackageVersion CpmVersion(ProjectFileAnalysisContext context, PackageReference @override)
        => context.File
            .WalkBackward()
            .OfType<PackageVersion>()
            .Where(v => v.Include == @override.IncludeOrUpdate)
            .FirstOrDefault(v => v.Version is { Length: > 0 });
}
