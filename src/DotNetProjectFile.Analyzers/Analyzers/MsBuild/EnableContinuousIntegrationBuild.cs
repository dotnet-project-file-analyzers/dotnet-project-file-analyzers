namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class EnableContinuousIntegrationBuild() : MsBuildProjectFileAnalyzer(Rule.EnableContinuousIntegrationBuild)
{
    /// <inheritdoc />
    public override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        context.ReportDiagnostic(Descriptor, context.File);
    }
}
