namespace DotNetProjectFile.IO.Globbing;

internal sealed class Group(IReadOnlyList<Segement> segments) : Segement
{
    public IReadOnlyList<Segement> Segments { get; } = segments;

    /// <inheritdoc />
    public override int MinLength => Segments.Sum(s => s.MinLength);

    /// <inheritdoc />
    public override int MaxLength => Segments.Sum(s => s.MaxLength);

    /// <inheritdoc />
    public override bool IsParseble => Segments.All(s => s.IsParseble);

    public override string ToString() => string.Concat(Segments);
}
