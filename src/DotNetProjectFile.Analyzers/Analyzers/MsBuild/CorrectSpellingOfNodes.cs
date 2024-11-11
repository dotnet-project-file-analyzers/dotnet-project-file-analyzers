using DotNetProjectFile.Text;

namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class CorrectSpellingOfNodes() : MsBuildProjectFileAnalyzer(Rule.CorrectSpellingOfNodes)
{
    private readonly ImmutableArray<string> Knowns =
    [
        .. NodeFactory.KnownNodes.Select(x => x.ToUpperInvariant()),
        .. Known.NodeNames.Select(x => x.ToUpperInvariant()),
    ];

    protected override void Register(ProjectFileAnalysisContext<MsBuildProject> context)
    {
        Walk(context.File, context);
    }

    private void Walk(Node node, ProjectFileAnalysisContext<MsBuildProject> context)
    {
        if (node is Unknown
            && !Known.NodeNames.Contains(node.LocalName)
            && Suggestion(node) is { Length: > 0 } suggestion)
        {
            context.ReportDiagnostic(Descriptor, node, node.LocalName, suggestion);
        }

        foreach (var child in node.Children)
        {
            Walk(child, context);
        }
    }

    private string? Suggestion(Node node)
    {
        var name = node.LocalName.ToUpperInvariant();
        string? suggestion = null;
        var best = name.Length / 2;

        var word = new Levenshtein(name);

        foreach (var known in Knowns)
        {
            var test = word.DistanceFrom(known);
            if (test < best)
            {
                best = test;
                suggestion = known;
            }
        }
        return suggestion;
    }
}
