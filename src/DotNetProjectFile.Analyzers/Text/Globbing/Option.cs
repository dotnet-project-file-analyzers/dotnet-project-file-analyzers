
namespace DotNetProjectFile.Text.Globbing;

internal sealed class Option(IReadOnlyList<Segment> segments) : Segment
{
    public IReadOnlyList<Segment> Segments { get; } = segments;

    /// <inheritdoc />
    public override int MinLength => Segments.Min(s => s.MinLength);

    /// <inheritdoc />
    public override int MaxLength => Segments.Max(s => s.MaxLength);

    /// <inheritdoc />
    [Pure]
    public override bool IsMatch(ReadOnlySpan<char> value, StringComparison comparison)
    {
        foreach (var segment in Segments)
        {
            if (segment.IsMatch(value, comparison)) return true;
        }
        return false;
    }

    /// <inheritdoc />
    public override string ToString() => $"{{{string.Join(",", Segments)}}}";
}
