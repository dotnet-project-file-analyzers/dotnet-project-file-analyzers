namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class OverrideTargetFrameworksWithTargetFrameworks()
    : MsBuildProjectFileAnalyzer(Rule.OverrideTargetFrameworksWithTargetFrameworks)
{
    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (context.File.Property<TargetFrameworks>() is null) return;

        foreach (var tfm in context.File.PropertyGroups.SelectMany(g => g.TargetFramework))
        {
            context.ReportDiagnostic(Descriptor, tfm);
        }
    }
}
