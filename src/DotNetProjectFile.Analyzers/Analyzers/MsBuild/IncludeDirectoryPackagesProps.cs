namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Implements <see cref="Rule.IncludeDirectoryPackagesProps"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class IncludeDirectoryPackagesProps() : MsBuildProjectFileAnalyzer(Rule.IncludeDirectoryPackagesProps)
{
    /// <inheritdoc />
    public override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (context.File.ManagePackageVersionsCentrally().GetValueOrDefault()
            && context.File.DirectoryPackagesProps is null)
        {
            context.ReportDiagnostic(Descriptor, context.File.Locations.StartElement);
        }
    }
}
