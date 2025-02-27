namespace DotNetProjectFile.Text.Globbing;

internal sealed class NotSequence(string options) : Sequence(options)
{
    /// <inheritdoc />
    [Pure]
    public override bool IsMatch(ReadOnlySpan<char> value, StringComparison comparison)
        => value.Length == 1
        && !base.IsMatch(value, comparison);

    /// <inheritdoc />
    public override string ToString() => $"[!{Options}]";
}
