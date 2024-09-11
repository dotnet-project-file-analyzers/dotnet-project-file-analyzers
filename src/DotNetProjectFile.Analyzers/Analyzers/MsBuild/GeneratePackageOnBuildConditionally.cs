namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class GeneratePackageOnBuildConditionally()
    : MsBuildProjectFileAnalyzer(Rule.GeneratePackageOnBuildConditionally)
{
    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var generate in context.Project.PropertyGroups.SelectMany(p => p.GeneratePackageOnBuild)
            .Where(g => g.AncestorsAndSelf().All(n => n.Condition is not { Length: > 0 })))
        {
            context.ReportDiagnostic(Descriptor, generate);
        }
    }
}
