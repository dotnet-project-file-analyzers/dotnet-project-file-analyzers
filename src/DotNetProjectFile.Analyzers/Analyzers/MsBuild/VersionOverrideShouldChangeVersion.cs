namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class VersionOverrideShouldChangeVersion()
    : MsBuildProjectFileAnalyzer(Rule.VersionOverrideShouldChangeVersion)
{
    protected override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile_Props;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (context.Project.ManagePackageVersionsCentrally() is not true) return;

        foreach (var @override in context.Project.ItemGroups
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
        => context.Project
            .ImportsAndSelf()
            .SelectMany(p => p.ItemGroups)
            .SelectMany(g => g.PackageVersions)
            .Where(v => v.Include == @override.IncludeOrUpdate)
            .LastOrDefault(v => v.Version is { Length: > 0 });
}
