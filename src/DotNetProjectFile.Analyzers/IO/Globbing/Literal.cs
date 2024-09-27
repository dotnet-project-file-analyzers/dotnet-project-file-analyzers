namespace DotNetProjectFile.IO.Globbing;

public class Literal(string value) : Segement
{
    public string Value { get; } = value;

    public override int MinLength => Value.Length;

    public override int MaxLength => Value.Length;

    public override string ToString() => Value;
}
