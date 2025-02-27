
namespace DotNetProjectFile.Text.Globbing;

internal sealed class AnyChar : Segment
{
    /// <inheritdoc />
    public override int MinLength => 1;

    /// <inheritdoc />
    public override int MaxLength => 1;

    /// <inheritdoc />
    [Pure]
    public override bool IsMatch(ReadOnlySpan<char> value, StringComparison comparison)
        => value.Length == 1;

    /// <inheritdoc />
    [Pure]
    public override string ToString() => "?";
}
