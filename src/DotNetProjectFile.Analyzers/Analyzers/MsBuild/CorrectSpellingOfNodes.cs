using DotNetProjectFile.Text;

namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class CorrectSpellingOfNodes() : MsBuildProjectFileAnalyzer(Rule.CorrectSpellingOfNodes)
{
    private readonly ImmutableArray<Suggestion> Knowns =
    [
        .. Node.Factory.KnownNodes.Select(Suggestion.New),
        .. Known.NodeNames.Select(Suggestion.New),
    ];

    protected override void Register(ProjectFileAnalysisContext<MsBuildProject> context)
    {
        Walk(context.File, context);
    }

    private void Walk(Node node, ProjectFileAnalysisContext<MsBuildProject> context)
    {
        if (node is Unknown && GetSuggestion(node.LocalName) is { Length: > 0 } suggestion)
        {
            context.ReportDiagnostic(Descriptor, node, node.LocalName, suggestion);
        }

        foreach (var child in node.Children)
        {
            Walk(child, context);
        }
    }

    /// <summary>Gets a suggestion based on the provided name.</summary>
    [Pure]
    public string? GetSuggestion(string name, double factor = 0.34)
    {
        if (Known.NodeNames.Contains(name)) { return null; }

        string? suggestion = null;
        var best = (int)(name.Length * factor);

        var word = new Levenshtein(name.ToUpperInvariant());

        foreach (var known in Knowns)
        {
            var test = word.DistanceFrom(known.Upper);
            if (test < best)
            {
                best = test;
                suggestion = known.Name;
            }
        }
        return suggestion;
    }

    private readonly record struct Suggestion(string Name, string Upper)
    {
        public static Suggestion New(string name) => new(name, name.ToUpperInvariant());
    }
}
