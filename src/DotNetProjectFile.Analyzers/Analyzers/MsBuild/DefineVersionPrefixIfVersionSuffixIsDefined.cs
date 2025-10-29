namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class DefineVersionPrefixIfVersionSuffixIsDefined() : MsBuildProjectFileAnalyzer(Rule.DefineVersionPrefixIfVersionSuffixIsDefined)
{
    /// <inheritdoc />
    public override ImmutableArray<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (context.File.Property<VersionSuffix>() is { } suffix
         && context.File.Property<VersionPrefix>() is null)
        {
            context.ReportDiagnostic(Descriptor, suffix);
        }
    }
}
