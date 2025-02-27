namespace DotNetProjectFile.Text.Globbing;

internal sealed class Literal(string text) : Segment
{
    /// <inheritdoc />
    public string Text { get; } = text;

    /// <inheritdoc />
    public override int MinLength => Text.Length;

    /// <inheritdoc />
    public override int MaxLength => Text.Length;

    /// <inheritdoc />
    [Pure]
    public override bool IsMatch(ReadOnlySpan<char> value, StringComparison comparison)
        => value.Equals(Text.AsSpan(), comparison);

    /// <inheritdoc />
    public override string ToString() => Text;
}
