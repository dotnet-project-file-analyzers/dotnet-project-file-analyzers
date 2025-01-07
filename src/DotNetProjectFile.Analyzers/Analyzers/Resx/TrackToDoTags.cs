using DotNetProjectFile.Resx;

namespace DotNetProjectFile.Analyzers.Resx;

/// <summary>Implements <see cref="Rule.TrackToDoTags"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class TrackToDoTags() : ResourceFileAnalyzer(Rule.TrackToDoTags)
{
    private readonly ToDoChecker<Resource> Checker = new(Rule.TrackToDoTags, GetText);

    /// <inheritdoc />
    protected override void Register(ResourceFileAnalysisContext context)
        => Checker.Check(context.File, context.File.Text, context);

    private static string? GetText(XmlAnalysisNode node) => node switch
    {
        Comment c => c.Text,
        _ => null,
    };
}
