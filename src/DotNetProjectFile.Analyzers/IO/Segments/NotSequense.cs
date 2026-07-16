namespace DotNetProjectFile.IO.Segments;

internal sealed class NotSequence(string options) : Sequence(options)
{
    /// <inheritdoc />
    [Pure]
    public override bool IsMatch(Chars value, StringComparison comparison)
        => value.Length is 1
        && !base.IsMatch(value, comparison);

    /// <inheritdoc />
    public override string ToString() => $"[!{Options}]";
}
