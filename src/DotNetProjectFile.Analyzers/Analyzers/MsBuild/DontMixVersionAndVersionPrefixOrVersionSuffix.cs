namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class DontMixVersionAndVersionPrefixOrVersionSuffix() : MsBuildProjectFileAnalyzer(Rule.DontMixVersionAndVersionPrefixOrVersionSuffix)
{
    /// <inheritdoc />
    public override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (context.File.Property<DotNetProjectFile.MsBuild.Version>() is not { } version)
        {
            return;
        }

        var prefix = context.File.Property<VersionPrefix>();
        var suffix = context.File.Property<VersionSuffix>();

        if (GetErrorArgument(prefix, suffix) is { } error)
        {
            context.ReportDiagnostic(Descriptor, version, error);
        }
    }

    private static string? GetErrorArgument(VersionPrefix? prefix, VersionSuffix? suffix)
        => (prefix, suffix) switch
        {
            ({ }, { }) => "<VersionPrefix> and <VersionSuffix> nodes",
            ({ }, null) => "<VersionPrefix> node",
            (null, { }) => "<VersionSuffix> node",
            _ => null,
        };
}
