namespace DotNetProjectFile.IO.Globbing;

internal sealed class Group(IReadOnlyList<Segement> segments) : Segement
{
    public IReadOnlyList<Segement> Segments { get; } = segments;

    public override int MinLength => Segments.Sum(s => s.MinLength);

    public override int MaxLength => Segments.Sum(s => s.MaxLength);

    public override string ToString() => string.Concat(Segments);
}
