

namespace DotNetProjectFile.Text.Globbing;

internal sealed class NotSequence(string options) : Segment
{
    /// <inheritdoc />
    public string Options { get; } = options;

    /// <inheritdoc />
    public override int MinLength => 0;

    /// <inheritdoc />
    public override int MaxLength => 0;

    public override bool IsMatch(ReadOnlySpan<char> value, StringComparison comparison)
    {
        return false;
    }

    /// <inheritdoc />
    public override string ToString() => $"[!{Options}]";
}
