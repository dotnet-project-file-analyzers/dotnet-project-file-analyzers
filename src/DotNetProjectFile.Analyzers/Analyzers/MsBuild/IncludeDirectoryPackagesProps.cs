namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class IncludeDirectoryPackagesProps() : MsBuildProjectFileAnalyzer(Rule.IncludeDirectoryPackagesProps)
{
    /// <inheritdoc />
    public override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (context.File.ManagePackageVersionsCentrally().GetValueOrDefault()
            && context.File.DirectoryPackagesProps is null)
        {
            context.ReportDiagnostic(Descriptor, context.File, context.File.Locations.StartElement);
        }
    }
}
