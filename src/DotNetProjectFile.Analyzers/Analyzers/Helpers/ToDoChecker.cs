using Microsoft.CodeAnalysis.Text;
using System.Text.RegularExpressions;

namespace DotNetProjectFile.Analyzers.Helpers;

internal sealed class ToDoChecker<TFile>(
    DiagnosticDescriptor descriptor,
    Func<XmlAnalysisNode, string?> getText)

    where TFile : class, ProjectFile, XmlAnalysisNode
{
    private readonly DiagnosticDescriptor Descriptor = descriptor;
    private readonly Func<XmlAnalysisNode, string?> GetText = getText;

    public void Check(
        XmlAnalysisNode node,
        SourceText text,
        ProjectFileAnalysisContext<TFile> context)
    {
        foreach (var comment in node.Element
            .DescendantNodes()
            .OfType<XComment>()
            .Select(x => new XmlComment(x, context.File)))
        {
            if (ToDoChecker.IsIssue(comment.Text) is { } issue)
            {
                context.ReportDiagnostic(Descriptor, comment.Project, comment.Positions.InnerSpan, issue);
            }
        }

        foreach (var child in node.Children())
        {
            Walk(child, text, context);
        }
    }

    private void Walk(XmlAnalysisNode node, SourceText text, ProjectFileAnalysisContext<TFile> context)
    {
        if (GetText(node) is { Length: >= 4 } txt && ToDoChecker.IsIssue(txt) is { } issue)
        {
            context.ReportDiagnostic(Descriptor, node.Project, node.Positions.InnerSpan, issue);
        }

        foreach (var child in node.Children())
        {
            Walk(child, text, context);
        }
    }
}

public static class ToDoChecker
{
    public static string? IsIssue(string text)
        => Issue.Match(text) is { Success: true } match
        ? match.Groups[nameof(Issue)].Value
        : null;

    private static readonly Regex Issue = new("(^|\\W)(?<Issue>(TO[- ]?DO('?S)?)|(FIX[- ]?ME))($|\\W)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture, TimeSpan.FromSeconds(2));
}
