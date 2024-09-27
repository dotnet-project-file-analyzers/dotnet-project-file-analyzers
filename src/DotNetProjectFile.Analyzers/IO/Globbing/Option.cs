namespace DotNetProjectFile.IO.Globbing;

internal class Option(IReadOnlyList<Segement> values) : Segement
{
    public IReadOnlyList<Segement> Values { get; } = values;

    public override int MinLength => Values.Min(s => s.MinLength);

    public override int MaxLength => Values.Max(s => s.MaxLength);

    public override string ToString() => $"{{{string.Join(",", Values)}}}";
}
