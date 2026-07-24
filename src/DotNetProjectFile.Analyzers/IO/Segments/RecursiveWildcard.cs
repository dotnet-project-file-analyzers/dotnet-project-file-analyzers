namespace DotNetProjectFile.IO.Segments;

internal sealed class RecursiveWildcard : Segment
{
    /// <inheritdoc />
    public override int MinLength => 0;

    /// <inheritdoc />
    public override int MaxLength => int.MaxValue;

    /// <inheritdoc />
    [Pure]
    public override bool IsMatch(Chars value, StringComparison comparison)
        => true;

    /// <inheritdoc />
    [Pure]
    public override string ToString() => "**";
}
