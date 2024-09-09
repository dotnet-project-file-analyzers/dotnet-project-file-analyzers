namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class UpdateTargetFrameworksWithTargetFrameworks()
    : MsBuildProjectFileAnalyzer(Rule.UpdateTargetFrameworksWithTargetFrameworks)
{
    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (context.Project.SelfAndImports().SelectMany(p => p.PropertyGroups).SelectMany(g => g.TargetFrameworks).None()) return;

        foreach (var tfm in context.Project.PropertyGroups.SelectMany(g => g.TargetFramework))
        {
            context.ReportDiagnostic(Descriptor, tfm);
        }
    }
}
