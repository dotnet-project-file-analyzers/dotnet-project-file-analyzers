namespace DotNetProjectFile.Text.Globbing;

internal sealed class Option(IReadOnlyList<Segement> segments) : Segement
{
    public IReadOnlyList<Segement> Segments { get; } = segments;

    /// <inheritdoc />
    public override int MinLength => Segments.Min(s => s.MinLength);

    /// <inheritdoc />
    public override int MaxLength => Segments.Max(s => s.MaxLength);

    public override string ToString() => $"{{{string.Join(",", Segments)}}}";
}
