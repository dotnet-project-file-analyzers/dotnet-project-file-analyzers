namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class RunNuGetSecurityAuditsAutomatically() : MsBuildProjectFileAnalyzer(Rule.RunNuGetSecurityAudit)
{
    protected override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (context.Project.Property<bool?, NuGetAudit>(g => g.NuGetAudit, MsBuildDefaults.NuGetAudit) == false)
        {
            context.ReportDiagnostic(Description, context.Project);
        }
    }
}
