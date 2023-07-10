namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class DefineSingleTargetFramework : MsBuildProjectFileAnalyzer
{
    public DefineSingleTargetFramework() : base(Rule.DefineSingleTargetFramework) { }

    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var frameworks in context.Project.AncestorsAndSelf()
            .SelectMany(p => p.PropertyGroups)
            .SelectMany(p => p.TargetFrameworks)
            .Where(f => f.Values.Count <= 1))
        {
            context.ReportDiagnostic(Descriptor, frameworks.Location);
        }
    }
}
