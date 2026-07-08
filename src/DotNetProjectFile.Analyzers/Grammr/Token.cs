using Microsoft.CodeAnalysis.Text;

namespace Grammr;

[DebuggerDisplay("{Formatter.Format(ToString())}")]
public readonly struct Token(int start, int length, Source source, string? kind)
{
    private readonly Source Source = source;

    public string? Kind { get; } = kind;

    public int Start { get; } = start;

    public int Length { get; } = length;

    public int End => Start + Length;

    public ReadOnlySpan<char> Span => Source.AsSpan(Start, Length);

    public LinePositionSpan LinePositionSpan { get; }

    /// <inheritdoc />
    [Pure]
    public override string ToString() => Span.ToString();
}
