namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Implements <see cref="Rule.PackablesShouldBeLibraries"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class PackablesShouldBeLibraries() : MsBuildProjectFileAnalyzer(Rule.PackablesShouldBeLibraries)
{
    /// <inheritdoc />
    public override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext<MsBuildProject> context)
    {
        if (context.File.IsPackable() && context.File.GetOutputType() != OutputType.Kind.Library)
        {
            context.ReportDiagnostic(Descriptor, context.File);
        }
    }
}
