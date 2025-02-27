namespace DotNetProjectFile.Text.Globbing;

internal sealed class Wildcard : Segment
{
    /// <inheritdoc />
    public override int MinLength => 0;
    
    /// <inheritdoc />
    public override int MaxLength => int.MaxValue;

    /// <inheritdoc />
    [Pure]
    public override bool IsMatch(ReadOnlySpan<char> value, StringComparison comparison)
    {
        foreach (var ch in value)
        {
            if (ch is '/' or '\\') return false;
        }
        return true;
    }

    /// <inheritdoc />
    [Pure]
    public override string ToString() => "*";
}
