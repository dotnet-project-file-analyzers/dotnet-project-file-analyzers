namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class OrderUsingDirectivesAlphabetically() : MsBuildProjectFileAnalyzer(Rule.OrderUsingDirectivesAlphabetically)
{
    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var group in context.File.ItemGroups
            .Select(g => g.Usings)
            .SelectMany(g => g.GroupBy(u => u.Type)))
        {
            AnalyzeGroup(context, group);
        }
    }

    private void AnalyzeGroup(ProjectFileAnalysisContext context, IEnumerable<Using> group)
        => group.CheckAlphabeticalOrder(r => r.Include, (expected, found) =>
        {
            context.ReportDiagnostic(Descriptor, expected, expected.Type.GetPrettyName(), expected.Include, found.Include);
        });
}
