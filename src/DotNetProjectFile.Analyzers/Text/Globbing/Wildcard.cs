namespace DotNetProjectFile.Text.Globbing;

internal sealed class Wildcard : Segement
{
    public override int MinLength => 0;

    public override int MaxLength => int.MaxValue;

    public override string ToString() => "*";
}
