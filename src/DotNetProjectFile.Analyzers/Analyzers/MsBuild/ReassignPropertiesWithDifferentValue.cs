namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class ReassignPropertiesWithDifferentValue : MsBuildProjectFileAnalyzer
{
    public ReassignPropertiesWithDifferentValue() : base(Rule.ReassignPropertiesWithDifferentValue) { }

    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (context.Project.Imports.None()) { return; }

        foreach (var prop in context.Project.PropertyGroups.SelectMany(p => p.Children()))
        {
            if (EarlierAssignement(prop) is { } previous
                && Equals(prop.Val, previous.Val))
            {
                context.ReportDiagnostic(Descriptor, prop, prop.LocalName);
            }
        }
    }

    private static Node? EarlierAssignement(Node node)
    {
        foreach (var import in node.Project.SelfAndImports().Skip(1))
        {
            if (import.PropertyGroups
                .SelectMany(p => p.Children())
                .FirstOrDefault(n => Same(node, n)) is { } previous)
            {
                return previous;
            }
        }
        return null;
    }

    private static bool Same(Node l, Node r)
        => l.LocalName == r.LocalName
        && Enumerable.SequenceEqual(l.Conditions(), r.Conditions());
}
