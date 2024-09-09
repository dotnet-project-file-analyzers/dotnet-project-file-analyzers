namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class DefineConditionsOnLevel1() : MsBuildProjectFileAnalyzer(Rule.DefineConditionsOnLevel1)
{
    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var node in context.Project
            .DescendantsAndSelf()
            .Where(n => n.Depth > 1 && n.Condition is { Length: > 0 } && n is not When))
        {
            var parent = node.AncestorsAndSelf().Skip(1).First().LocalName;
            context.ReportDiagnostic(Descriptor, node, parent);
        }
    }
}
