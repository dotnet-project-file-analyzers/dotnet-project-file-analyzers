namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class OverrideTargetFrameworksWithTargetFrameworks()
    : MsBuildProjectFileAnalyzer(Rule.OverrideTargetFrameworksWithTargetFrameworks)
{
    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var tfm in context.File.PropertyGroups.
            Children<TargetFramework>(tfm => context.File.PropertyGroups
                .Children<TargetFrameworks>()
                .Any(tfms => tfms.Condition is null || tfms.Condition == tfm.Condition)))
        {
            context.ReportDiagnostic(Descriptor, tfm);
        }
    }
}
