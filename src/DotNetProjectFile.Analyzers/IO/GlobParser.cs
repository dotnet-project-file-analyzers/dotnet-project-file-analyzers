using DotNetProjectFile.IO.Segments;
using Grammr;
using Grammr.Lexers;
using static Grammr.Lexers.Shared;

namespace DotNetProjectFile.IO;

internal static class GlobParser
{
    private static readonly Lexer comma = ch(',');
    private static readonly Lexer any = ch('?');
    private static readonly Lexer wild = match(IsStar);
    private static readonly Lexer not = ch('!') | ch('^');
    private static readonly Lexer cho_str = ch('{');
    private static readonly Lexer cho_end = ch('}');
    private static readonly Lexer seq_str = ch('[');
    private static readonly Lexer seq_mid = match(IsSequence);
    private static readonly Lexer seq_end = ch(']');

    private static bool IsStar(char c, int _) => c is '*';

    private static bool IsSequence(char c, int _) => c is not '[' and not ']' and not '!';

    public static Segment? Parse(ref SourceReader reader)
        => Segement(ref reader) is { } segment
        ? segment
        : null;

    private static Segment? Segement(ref SourceReader reader)
    {
        var r = reader;
        var group = new List<Segment>();

        while (!r.EOS)
        {
            var segement = null
                ?? Any(/*......*/ref r)
                ?? Wild(/*.....*/ref r)
                ?? Choice(/*...*/ref r)
                ?? Sequence(/*.*/ref r)
                ?? Literal(/*..*/ref r);

            if (segement is null) break;
            group.Add(segement);
        }

        if (group.Count == 0) return null;

        reader = r;
        return group.Count == 1
            ? group[0]
            : Segment.Group([.. group]);
    }

    private static Segment? Any(ref SourceReader reader)
    {
        var r = reader;
        if (r.Cons(any))
        {
            reader = r;
            return Segment.AnyChar;
        }
        else return null;
    }

    private static Segment? Wild(ref SourceReader reader)
    {
        var r = reader;
        if (r.Take(wild) is { } token)
        {
            reader = r;
            return token.Length is 1
                ? Segment.Wildcard
                : Segment.RecursiveWildcard;
        }
        else return null;
    }

    private static Segment? Choice(ref SourceReader reader)
    {
        var choices = new List<Segment>();
        var r = reader;
        if (Chain
            && r.Cons(cho_str)
            && Choices(ref r, choices)
            && r.Cons(cho_end))
        {
            reader = r;
            return new Option(choices);
        }
        else return null;
    }

    private static bool Choices(ref SourceReader reader, List<Segment> segements)
    {
        var r = reader;

        while (!reader.EOS && Segement(ref r) is { } segement)
        {
            segements.Add(segement);

            if (!r.Cons(comma))
            {
                reader = r;
                return true;
            }
        }
        return false;
    }

    private static Segment? Sequence(ref SourceReader reader)
    {
        var r = reader;
        if (Chain
            && r.Cons(seq_str)
            && r.Take(not.optional) is { } negate
            && r.Take(seq_mid) is { } options
            && r.Cons(seq_end))
        {
            reader = r;
            return negate.Length is 1
                ? new NotSequence(options.Span.ToString())
                : new Sequence(options.Span.ToString());
        }
        else return null;
    }

    private static Literal? Literal(ref SourceReader reader)
    {
        Span<char> buffer = stackalloc char[reader.Length];
        var (len, ext, esc) = (0, 0, false);

        for (var pos = 0; pos < buffer.Length; pos++)
        {
            var c = reader[pos];

            if (c is '\r' or '\n' or '\t')
            {
                break;
            }
            else if (esc)
            {
                buffer[len++] = c;
                esc = false;
            }
            else if (Special(c))
            {
                break;
            }
            else if (c is '\\')
            {
                ext++;
                esc = true;
            }
            else
            {
                buffer[len++] = c;
            }
        }

        if (len > 0)
        {
            reader.Cons(len + ext);
            return new Literal(buffer[..len].ToString());
        }
        else return null;

        static bool Special(char c) => c is '?' or '*' or '[' or '{' or '}' or ',';
    }
}
