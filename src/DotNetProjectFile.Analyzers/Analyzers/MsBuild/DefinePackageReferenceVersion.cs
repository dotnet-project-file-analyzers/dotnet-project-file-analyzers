namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class DefinePackageReferenceVersion()
    : MsBuildProjectFileAnalyzer(Rule.DefinePackageReferenceVersion)
{
    /// <inheritdoc />
    public override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        var versions = context.File.ManagePackageVersionsCentrally() is true
            ? PackageVersions(context.File)
            : [];

        var references = context.File.ItemGroups
            .SelectMany(g => g.PackageReferences)
            .Where(r => r.IncludeOrUpdate is { Length: > 0 })
            .Where(r => r.VersionOrVersionOverride is not { Length: > 0 });

        foreach (var reference in references)
        {
            if (!versions.Contains(reference.IncludeOrUpdate))
            {
                context.ReportDiagnostic(Descriptor, reference, reference.IncludeOrUpdate);
            }
        }
    }

    private static ImmutableHashSet<string> PackageVersions(MsBuildProject project) => project
        .Walk()
        .OfType<PackageVersion>()
        .Where(r => r.Version is { Length: > 0 })
        .Where(r => r.IncludeOrUpdate is { Length: > 0 })
        .Select(r => r.IncludeOrUpdate)
        .ToImmutableHashSet();
}
