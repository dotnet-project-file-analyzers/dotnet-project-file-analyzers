namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class RunNuGetSecurityAuditsAutomatically : MsBuildProjectFileAnalyzer
{
    public RunNuGetSecurityAuditsAutomatically() : base(Rule.RunNuGetSecurityAudit) { }

    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (context.Project.IsProject)
        {
            var audits = context.Project
                .AncestorsAndSelf()
                .SelectMany(p => p.PropertyGroups)
                .SelectMany(g => g.NuGetAudit);

            if (audits.None() || audits.Any(a => a.Value != true))
            {
                context.ReportDiagnostic(Descriptor, context.Project);
            }
        }
    }
}
