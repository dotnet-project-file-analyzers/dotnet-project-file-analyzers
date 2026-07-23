namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Implements <see cref="Rule.AlignDirectoryBuildAndPackages"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class AlignDirectoryBuildAndPackages() : MsBuildProjectFileAnalyzer(Rule.AlignDirectoryBuildAndPackages)
{
    /// <inheritdoc />
    public override ImmutableArray<AnalyzerType> ApplicableTo => ProjectFileTypes.ProjectFile;

    /// <inheritdoc />
    public override bool DisableOnFailingImport => false;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (context.File.DirectoryBuildProps is { } build
            && context.File.DirectoryPackagesProps is { } packages
            && !build.Path.Directory.Equals(packages.Path.Directory))
        {
            context.ReportDiagnostic(Descriptor, context.File, build.Path, packages.Path);
        }
    }
}
