namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Implements <see cref="Rule.AllignDirectoryBuildAndPackages"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class AllignDirectoryBuildAndPackages() : MsBuildProjectFileAnalyzer(Rule.AllignDirectoryBuildAndPackages)
{
    /// <inheritdoc />
    public override ImmutableArray<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    /// <inheritdoc />
    public override bool DisableOnFailingImport => false;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext<MsBuildProject> context)
    {
        if (context.File.DirectoryBuildProps is { } build
            && context.File.DirectoryPackagesProps is { } packages
            && !build.Path.Directory.Equals(packages.Path.Directory))
        {
            context.ReportDiagnostic(Descriptor, context.File, build.Path, packages.Path);
        }
    }
}
