
namespace DotNetProjectFile.Text.Globbing;

internal sealed class Group(IReadOnlyList<Segment> segments) : Segment
{
    public IReadOnlyList<Segment> Segments { get; } = segments;

    /// <inheritdoc />
    public override int MinLength => Segments.Sum(s => s.MinLength);

    /// <inheritdoc />
    public override int MaxLength => Segments.Sum(s => s.MaxLength);

    /// <inheritdoc />
    [Pure]
    public override bool IsMatch(ReadOnlySpan<char> value, StringComparison comparison)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public override string ToString() => string.Concat(Segments);
}
