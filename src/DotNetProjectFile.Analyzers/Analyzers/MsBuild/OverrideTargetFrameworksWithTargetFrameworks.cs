namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class OverrideTargetFrameworksWithTargetFrameworks()
    : MsBuildProjectFileAnalyzer(Rule.OverrideTargetFrameworksWithTargetFrameworks)
{
    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var tfm in context.File.PropertyGroups.
            SelectMany(g => g.TargetFramework)
            .Where(tfm => context.File.PropertyGroups
                .SelectMany(g => g.TargetFrameworks)
                .Any(tmfs => tmfs.Condition is null || tmfs.Condition == tfm.Condition)))
        {
            context.ReportDiagnostic(Descriptor, tfm);
        }
    }
}
