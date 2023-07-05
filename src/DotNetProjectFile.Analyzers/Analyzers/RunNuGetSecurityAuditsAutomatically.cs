namespace DotNetProjectFile.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class RunNuGetSecurityAuditsAutomatically : ProjectFileAnalyzer
{
    public RunNuGetSecurityAuditsAutomatically() : base(Rule.RunNuGetSecurityAudit) { }

    protected override void Register(ProjectFileAnalysisContext context)
    {
        var audits = context.Project.AncestorsAndSelf()
          .SelectMany(p => p.PropertyGroups)
          .SelectMany(g => g.NuGetAudits);

        if (audits.None() || audits.Any(a => a.Value != true))
        {
            context.ReportDiagnostic(Descriptor, context.Project.Location);
        }
    }
}
