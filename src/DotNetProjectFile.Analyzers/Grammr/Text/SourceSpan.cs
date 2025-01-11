using Microsoft.CodeAnalysis.Text;
using System.Text.RegularExpressions;

namespace Grammr.Text;

/// <summary>Represents a span of a source text.</summary>
/// <param name="source">
/// The full source text.
/// </param>
/// <param name="textSpan">
/// The (selected) span of the source text.
/// </param>
public readonly struct SourceSpan(Source source, TextSpan textSpan) : IEquatable<SourceSpan>
{
    /// <summary>Initializes a new instance of the <see cref="SourceSpan"/> struct.</summary>
    /// <param name="source">
    /// The full source.
    /// </param>
    public SourceSpan(Source source) : this(source, source.TextSpan) { }

    /// <remarks>Syntactic sugar that allows the use of NoMatch over null.</remarks>
    private static readonly TextSpan? NoMatch = null;

    /// <summary>The underlying source.</summary>
    public readonly Source Source = source;

    /// <summary>The (selected) span.</summary>
    public readonly TextSpan Span = textSpan;

    /// <summary>The (selected) source text.</summary>
    public string Text => Source.SourceText.ToString(Span);

    /// <summary>The First char in the span.</summary>
    public char First => this[0];

    /// <summary>Gets the n^th char of the source text.</summary>
    public char this[int index] => Source.Text[Start + index];

    /// <summary>The start position.</summary>
    public int Start => Span.Start;

    /// <summary>The end position.</summary>
    public int End => Span.End;

    /// <summary>The length of the span.</summary>
    public int Length => Span.Length;

    /// <summary>Indicates if the span is empty.</summary>
    public bool IsEmpty => Span.IsEmpty;

    /// <summary>Indicates if the span is not empty.</summary>
    public bool HasValue => !Span.IsEmpty;

    /// <summary>Matches until the end of line.</summary>
    /// <returns>
    /// The matching text span (can have a length of zero).
    /// </returns>
    [Pure]
    public SourceSpan Line()
    {
        var line = Source.Lines.GetLineFromPosition(Start);
        var span = line.Span;
        var text = new TextSpan(Start, span.Length - (Start - span.Start));
        return new(Source, text);
    }

    /// <summary>Trims the source span.</summary>
    /// <param name="span">
    /// The span to trim to.
    /// </param>
    /// <returns>
    /// A trimmed source span.
    /// </returns>
    public SourceSpan Trim(TextSpan span) => new(Source, span);

    /// <summary>Takes the number of specified characters from the start of this source span.</summary>
    [Pure]
    public SourceSpan Take(int count) => new(Source, new(Start, count));

    /// <summary>Trims the source span from the left.</summary>
    /// <param name="left">
    /// The number of characters to trim.
    /// </param>
    /// <returns>
    /// A trimmed source span.
    /// </returns>
    [Pure]
    public SourceSpan Skip(int left) => new(Source, new(Start + left, Length - left));

    /// <summary>Indicates that the text span starts with the specified character.</summary>
    /// <param name="ch">
    /// The character to match.
    /// </param>
    /// <returns>
    /// Null if no match, otherwise the matching text span.
    /// </returns>
    [Pure]
    public TextSpan? StartsWith(char ch)
    {
        var result = !Span.IsEmpty && Source.Text[Span.Start] == ch
            ? new TextSpan(Span.Start, 1)
            : NoMatch;

        return result;
    }

    /// <summary>Indicates that the text span starts with the specified string.</summary>
    /// <param name="str">
    /// The string to match.
    /// </param>
    /// <returns>
    /// Null if no match, otherwise the matching text span.
    /// </returns>
    [Pure]
    public TextSpan? StartsWith(string str)
    {
        if (Span.Length >= str.Length)
        {
            var pos = Span.Start;

            for (var i = 0; i < str.Length; i++)
            {
                if (Source[pos++] != str[i])
                {
                    return NoMatch;
                }
            }
            return new TextSpan(Span.Start, str.Length);
        }
        else
        {
            return NoMatch;
        }
    }

    /// <summary>Matches the predicate.</summary>
    /// <param name="match">
    /// The required match.
    /// </param>
    /// <returns>
    /// Null if no match, otherwise the matching text span.
    /// </returns>
    [Pure]
    public TextSpan? Predicate(Predicate<char> match)
    {
        if (IsEmpty)
        {
            return NoMatch;
        }

        var len = -1;
        var i = Start;

        while (++len < Length)
        {
            if (!match(Source[i++]))
            {
                return (len == 0)
                    ? NoMatch
                    : new(Span.Start, len);
            }
        }
        return Span;
    }

    /// <inheritdoc cref="Match(Regex)" />
    [Pure]
    public TextSpan? Match([StringSyntax(StringSyntaxAttribute.Regex)] string pattern) => Match(new Regex(pattern, RegexOptions.CultureInvariant));

    /// <summary>Reports a <see cref="TextSpan"/> indicating the position of the match.</summary>
    /// <remarks>
    /// Returns null if the character was not found.
    /// </remarks>
    [Pure]
    public TextSpan? Match(Regex pattern)
        => pattern.Match(Source.Text, Span.Start, Span.Length) is { Success: true } match
        ? new TextSpan(match.Index, match.Length)
        : NoMatch;

    /// <summary>Reports a <see cref="TextSpan"/> indicating the position of the character.</summary>
    /// <remarks>
    /// Returns null if the character was not found.
    /// </remarks>
    [Pure]
    public TextSpan? IndexOf(char ch)
    {
        var index = Source.Text.IndexOf(ch, Start, Length);
        return index == -1
            ? NoMatch
            : new(index, 1);
    }

    /// <inheritdoc />
    [Pure]
    public override string ToString() => Text;

    /// <inheritdoc />
    [Pure]
    public override bool Equals(object obj) => obj is SourceSpan other && Equals(other);

    /// <inheritdoc />
    [Pure]
    public bool Equals(SourceSpan other)
        => Span == other.Span
        && Source.Text == other.Text;

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => Span.GetHashCode();

    /// <summary>Returns true if left and right are equal.</summary>
    /// <param name="left">
    /// Left operator.
    /// </param>
    /// <param name="right">
    /// Right operator.
    /// </param>
    public static bool operator ==(SourceSpan left, SourceSpan right) => left.Equals(right);

    /// <summary>Returns true if left and right are different.</summary>
    /// <param name="left">
    /// Left operator.
    /// </param>
    /// <param name="right">
    /// Right operator.
    /// </param>
    public static bool operator !=(SourceSpan left, SourceSpan right) => !(left == right);

    public static SourceSpan operator ++(SourceSpan span) => span.Skip(1);
}
