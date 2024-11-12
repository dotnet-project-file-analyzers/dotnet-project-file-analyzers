namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class RunNuGetSecurityAuditsAutomatically() : MsBuildProjectFileAnalyzer(Rule.RunNuGetSecurityAudit)
{
    protected override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (context.File.NuGetAuditEnabled() is not true)
        {
            context.ReportDiagnostic(Descriptor, context.File);
        }
    }
}
