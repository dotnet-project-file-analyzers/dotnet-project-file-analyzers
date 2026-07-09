using System.Text.RegularExpressions;

namespace Grammr;

/// <summary>Represents a source that could be parsed.</summary>
public sealed class Source(string text)
{
    /// <summary>Gets the length of the source.</summary>
    private readonly string Text = text;

    /// <summary>Gets the length of the source.</summary>
    public int Length => Text.Length;

    /// <summary>Represents the source as a <see cref="ReadOnlySpan{T}" />.</summary>
    /// <param name="start">
    /// The start position of the span.
    /// </param>
    /// <param name="length">
    /// The length of the span.
    /// </param>
    [Pure]
    public ReadOnlySpan<char> AsSpan(int start, int length)
        => Text.AsSpan(start, length);

    /// <summary>Tries to match the regular expression.</summary>
    /// <param name="pattern">
    /// The pattern to match.
    /// </param>
    /// <param name="startat">
    /// The start position.
    /// </param>
    /// <remarks>
    /// This method allows <see cref="SourceReader"/> to use regular expressions
    /// without extra string allocations.
    /// </remarks>
    [Pure]
    public Match Match(Regex pattern, int startat) => pattern.Match(Text, startat, Length - startat);

    /// <summary>Implicitly casts a <see cref="string"/> to a <see cref="Source"/>.</summary>
    public static implicit operator Source(string text) => new(text);

    /// <summary>Implicitly casts a <see cref="Microsoft.CodeAnalysis.Text.SourceText"/> to a <see cref="Source"/>.</summary>
    public static implicit operator Source(Microsoft.CodeAnalysis.Text.SourceText text)
        => new(text.ToString());
}
