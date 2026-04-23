using System.Text.RegularExpressions;

namespace DotNetProjectFile.Analyzers.Helpers;

internal sealed class CommentChecker<TFile>(
    DiagnosticDescriptor descriptor,
    Func<string, string?> isIssue)

    where TFile : class, ProjectFile, XmlAnalysisNode
{
    private readonly DiagnosticDescriptor Descriptor = descriptor;
    private readonly Func<string, string?> IsIssue = isIssue;

    public void Check(
        XmlAnalysisNode node,
        ProjectFileAnalysisContext<TFile> context)
    {
        foreach (var comment in node.Element
            .DescendantNodes()
            .OfType<XComment>()
            .Select(x => new XmlComment(x, context.File)))
        {
            if (IsIssue(comment.Text) is { } issue)
            {
                context.ReportDiagnostic(Descriptor, comment.Locations.InnerSpan, issue);
            }
        }
    }
}

public static class CommentChecker
{
    public static string? ContainsXml(string comment) => comment switch
    {
        not { Length: > 0 } => null,
        _ when SingleWord.IsMatch(comment) => null,
        _ when Tag.IsMatch(comment)
            || Tag.IsMatch($"<{comment.Trim()}>") => comment,
        _ => null,
    };

    private static readonly Regex SingleWord = new(@"^\s+[A-Z]+\s+$", Options, TimeSpan.FromSeconds(1));

    private static readonly Regex Tag = new(
        @"<(?<Tag>[A-Z]\w+)(\s+(?<Attr>[A-Z]\w+)\s*=\s*"".*"")*\s*\/?>",
        Options,
        TimeSpan.FromSeconds(10));

    private const RegexOptions Options = RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Singleline | RegexOptions.Compiled;
}
