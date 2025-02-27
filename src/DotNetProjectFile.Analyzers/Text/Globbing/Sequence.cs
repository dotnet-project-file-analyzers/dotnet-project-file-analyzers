namespace DotNetProjectFile.Text.Globbing;

internal class Sequence(string options) : Segment
{
    public string Options { get; } = options;

    /// <inheritdoc />
    public override int MinLength => 1;

    /// <inheritdoc />
    public override int MaxLength => 1;

    /// <inheritdoc />
    [Pure]
    public override bool IsMatch(ReadOnlySpan<char> value, StringComparison comparison)
    {
        if (value.Length != 1) return false;

        var first = value[0];

        if (IsCaseSensitive(comparison))
        {
            return Options.Any(o => o == first);
        }
        else
        {
            first = char.ToUpperInvariant(first);
            return Options.Any(o => char.ToUpperInvariant(o) == first);
        }
    }

    private static bool IsCaseSensitive(StringComparison comparison) => comparison switch
    {
        StringComparison.OrdinalIgnoreCase or
        StringComparison.InvariantCultureIgnoreCase or
        StringComparison.CurrentCultureIgnoreCase => false,
        _ => true,
    };

    /// <inheritdoc />
    [Pure]
    public override string ToString() => $"[{Options}]";
}
