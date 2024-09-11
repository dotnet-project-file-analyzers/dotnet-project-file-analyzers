namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class DefineConditionsOnLevel1() : MsBuildProjectFileAnalyzer(Rule.DefineConditionsOnLevel1)
{
    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var node in context.Project
            .DescendantsAndSelf()
            .Where(IsConditionalOnLevel2OrHighger))
        {
            var parent = node.AncestorsAndSelf().Skip(1).First().LocalName;
            context.ReportDiagnostic(Description, node, parent);
        }
    }

    private static bool IsConditionalOnLevel2OrHighger(Node node)
        => node.Depth >= 2
        && node is not When
        && node is not Otherwise
        && node.Condition is { Length: > 0 };
}
