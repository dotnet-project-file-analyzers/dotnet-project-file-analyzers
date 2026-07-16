namespace DotNetProjectFile.IO.Segments;

internal sealed class Literal(string text) : Segment
{
    public string Text { get; } = text;

    /// <inheritdoc />
    public override int MinLength => Text.Length;

    /// <inheritdoc />
    public override int MaxLength => Text.Length;

    /// <inheritdoc />
    [Pure]
    public override bool IsMatch(Chars value, StringComparison comparison)
        => value.Equals(Text.AsSpan(), comparison);

    /// <inheritdoc />
    public override string ToString() => Text;
}
