namespace DotNetProjectFile.Text.Globbing;

internal sealed class Option(IReadOnlyList<Segment> segments) : Segment
{
    public IReadOnlyList<Segment> Segments { get; } = segments;

    /// <inheritdoc />
    public override int MinLength => Segments.Min(s => s.MinLength);

    /// <inheritdoc />
    public override int MaxLength => Segments.Max(s => s.MaxLength);

    /// <inheritdoc />
    public override string ToString() => $"{{{string.Join(",", Segments)}}}";
}
