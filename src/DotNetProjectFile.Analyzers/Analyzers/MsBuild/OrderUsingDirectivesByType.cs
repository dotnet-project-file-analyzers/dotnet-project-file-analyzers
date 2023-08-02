namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class OrderUsingDirectivesByType : MsBuildProjectFileAnalyzer
{
    public OrderUsingDirectivesByType() : base(Rule.OrderUsingDirectivesByType) { }

    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var directives in context.Project
            .ImportsAndSelf()
            .SelectMany(p => p.ItemGroups)
            .Select(g => g.Usings))
        {
            AnalyzeGroup(context, directives);
        }
    }

    private void AnalyzeGroup(ProjectFileAnalysisContext context, Nodes<Using> directives)
    {
        var foundGroups = directives.GroupBy(d => d.Type);
        var expectedGroups = foundGroups.OrderBy(found => found.Key);
        var zipped = expectedGroups.Zip(foundGroups, (f, e) => (f, e));

        if (zipped.HasDifference(out var expected, out var found))
        {
            var expectedType = expected.Key.GetPrettyName();
            var expectedFirst = expected.First();
            var foundType = found.Key.GetPrettyName();
            var foundFirst = found.First();
            context.ReportDiagnostic(Descriptor, expectedFirst, expectedType, expectedFirst.Include, foundType, foundFirst.Include);
        }
    }
}
