namespace DotNetProjectFile.IO.Globbing;

internal sealed class AnyChar : Segement
{
    public override int MinLength => 1;

    public override int MaxLength => 1;

    public override string ToString() => "?";
}
