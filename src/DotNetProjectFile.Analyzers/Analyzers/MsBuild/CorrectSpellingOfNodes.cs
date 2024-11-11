using DotNetProjectFile.Text;

namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class CorrectSpellingOfNodes() : MsBuildProjectFileAnalyzer(Rule.CorrectSpellingOfNodes)
{
    private const string ConfigKey = "dotnet_diagnostic.Proj0031.KnownNodes";

    private readonly ImmutableArray<Suggestion> Knowns =
    [
        .. Node.Factory.KnownNodes.Select(Suggestion.New),
        .. Known.NodeNames.Select(Suggestion.New),
    ];

    protected override void Register(ProjectFileAnalysisContext context)
    {
        var configured = GetConfigured(context);
        Walk(context.File, context, configured);
    }

    private static HashSet<string> GetConfigured(ProjectFileAnalysisContext context)
    {
        var configured = new HashSet<string>();

        var options = context.File.AdditionalText is { } additional
            ? context.Options.AnalyzerConfigOptionsProvider.GetOptions(additional)
            : context.Options.AnalyzerConfigOptionsProvider.GlobalOptions;

        if (options.TryGetValue(ConfigKey, out var names))
        {
            foreach (var name in names.Split([';'], StringSplitOptions.RemoveEmptyEntries))
            {
                configured.Add(name.Trim());
            }
        }
        return configured;
    }

    private void Walk(Node node, ProjectFileAnalysisContext context, HashSet<string> configured)
    {
        if (node is Unknown && GetSuggestion(node.LocalName, configured) is { Length: > 0 } suggestion)
        {
            context.ReportDiagnostic(Descriptor, node, node.LocalName, suggestion);
        }

        foreach (var child in node.Children)
        {
            Walk(child, context, configured);
        }
    }

    /// <summary>Gets a suggestion based on the provided name.</summary>
    [Pure]
    public string? GetSuggestion(string name, HashSet<string> configured, double factor = 0.34)
    {
        if (Known.NodeNames.Contains(name)) { return null; }

        string? suggestion = null;
        var best = (int)(name.Length * factor);

        var word = new Levenshtein(name.ToUpperInvariant());

        foreach (var known in configured.Select(Suggestion.New).Concat(Knowns))
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
