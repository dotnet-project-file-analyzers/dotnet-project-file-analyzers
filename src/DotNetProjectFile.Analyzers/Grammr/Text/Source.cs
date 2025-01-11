using Microsoft.CodeAnalysis.Text;

namespace Grammr.Text;

/// <summary>Wrapper around a <see cref="Microsoft.CodeAnalysis.Text.SourceText"/>.</summary>
/// <remarks>
/// stores the full text of the source text for performance reasons.
/// </remarks>
public readonly struct Source
{
    /// <summary>A match on source span.</summary>
    /// <param name="sourceSpan">
    /// The source span to match.
    /// </param>
    /// <returns>
    /// A text span indicating the match result.
    /// </returns>
    public delegate TextSpan? Match(SourceSpan sourceSpan);

    public readonly SourceText SourceText;
    public readonly TextSpan TextSpan;
    public readonly string Text;

    private Source(SourceText sourceText, TextSpan textSpan, string text)
    {
        SourceText = sourceText;
        TextSpan = textSpan;
        Text = text;
    }

    /// <summary>The length of the full source.</summary>
    public int Length => Text.Length;

    /// <summary>A character at the specified position of the source.</summary>
    public char this[int index] => Text[index];

    /// <inheritdoc cref="SourceText.Lines" />
    public TextLineCollection Lines => SourceText.Lines;

    /// <inheritdoc />
    public override string ToString() => Text;

    public static Source From(string text) => From(SourceText.From(text));

    public static Source From(SourceText sourceText)
        => new(sourceText, new TextSpan(0, sourceText.Length), sourceText.ToString());

    /// <summary>Implicitly casts from a <see cref="Microsoft.CodeAnalysis.Text.SourceText"/>.</summary>
    public static implicit operator Source(SourceText sourceText) => From(sourceText);

    /// <summary>Implicitly casts to a <see cref="SourceSpan"/>.</summary>
    public static implicit operator SourceSpan(Source source) => new(source);
}
