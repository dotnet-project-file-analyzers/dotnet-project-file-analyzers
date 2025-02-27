namespace DotNetProjectFile.Text.Globbing;

internal sealed class Group(IReadOnlyList<Segement> segments) : Segement
{
    public IReadOnlyList<Segement> Segments { get; } = segments;

    /// <inheritdoc />
    public override int MinLength => Segments.Sum(s => s.MinLength);

    /// <inheritdoc />
    public override int MaxLength => Segments.Sum(s => s.MaxLength);

    public override string ToString() => string.Concat(Segments);
}
