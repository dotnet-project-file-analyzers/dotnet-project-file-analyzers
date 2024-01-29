namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class RunNuGetSecurityAuditsAutomatically() : MsBuildProjectFileAnalyzer(Rule.RunNuGetSecurityAudit)
{
    protected override bool ApplyToProps => false;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (context.Project.Property<bool?, NuGetAudit>(g => g.NuGetAudit, MsBuildDefaults.NuGetAudit) == false)
        {
            context.ReportDiagnostic(Descriptor, context.Project);
        }
    }
}
