using DotNetProjectFile.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace DotNetProjectFile.IO.Globbing;

public static class GlobParser
{
    public static Segement? TryParse(string str)
    {
        var span = new SourceSpan(SourceText.From(str));

        return Global(span);
    }

    private static Segement? Global(SourceSpan span)
    {
        var group = new List<Segement>();

        while (!span.IsEmpty)
        {
            if (span.StartsWith('?') is { } any)
            {
                group.Add(Segement.AnyChar);
                span = span.TrimLeft(any.Length);
            }
            else if (span.Matches(c => c == '*') is { } wildcards)
            {
                group.Add(wildcards.Length == 1 ? Segement.Wildcard : Segement.RecursiveWildcard);
                span = span.TrimLeft(wildcards.Length);
            }
            else if (span.Segment() is { } segment)
            {
                var text = span.SourceText.ToString(segment);
                group.Add(text[0] == '!' ? new NotSequence(text[1..]) : new Sequence(text));
                span = span.TrimLeft(segment.Length + 2);
            }
            else if (span.Literal() is { } literal)
            {
                group.Add(new Literal(span.SourceText.ToString(literal)));
                span = span.TrimLeft(literal.Length);
            }
        }
        return group.Count == 1
            ? group[0]
            : Segement.Group(group);
    }

    private static TextSpan? Segment(this SourceSpan span)
    {
        if (span.StartsWith('[') is { })
        {
            span = span.TrimLeft(1);

            if (span.Matches(c => c != ']') is { } match &&
                span.TrimLeft(match.Length).StartsWith(']') is { })
            {
                return match;
            }
            else
            {
                throw new InvalidPattern("] Missing.");
            }
        }
        else
        {
            return null;
        }
    }

    private static TextSpan? Literal(this SourceSpan span)
        => span.Matches(c => c != '?' && c != '*' && c != '[');
}
