namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Implements <see cref="Rule.DefinePackageReferenceVersion"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class DefinePackageReferenceVersion()
    : MsBuildProjectFileAnalyzer(Rule.DefinePackageReferenceVersion)
{
    /// <inheritdoc />
    public override ImmutableArray<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        var versions = context.ManagePackageVersionsCentrally
            ? PackageVersions(context)
            : [];

        foreach (var reference in context.EnabledItems<PackageReference>().Where(WithoutVersion))
        {
            context.ReportDiagnostic(Descriptor, reference, reference.IncludeOrUpdate);
        }

        bool WithoutVersion(PackageReference r)
            => r.IncludeOrUpdate is { Length: > 0 }
            && r.VersionOrVersionOverride is not { Length: > 0 }
            && !versions.Contains(r.IncludeOrUpdate);
    }

    private static ImmutableHashSet<string> PackageVersions(ProjectFileAnalysisContext context) =>
    [
        .. context.EnabledItems<PackageVersion>()
            .Where(r => r.Version is { Length: > 0 })
            .Where(r => r.IncludeOrUpdate is { Length: > 0 })
            .Select(r => r.IncludeOrUpdate)
    ];
}
