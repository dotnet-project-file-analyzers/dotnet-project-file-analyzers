namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class RunNuGetSecurityAuditsAutomatically() : MsBuildProjectFileAnalyzer(Rule.RunNuGetSecurityAudit)
{
    /// <inheritdoc />
    public override ImmutableArray<AnalyzerType> ApplicableTo => ProjectFileTypes.ProjectFile;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (context.File.NuGetAuditEnabled() is not true)
        {
            context.ReportDiagnostic(Descriptor, context.File);
        }
    }
}
