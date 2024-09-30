namespace DotNetProjectFile.IO.Globbing;

internal sealed class NotSequence(string options) : Segement
{
    public string Options { get; } = options;

    public override int MinLength => 0;

    public override int MaxLength => 0;

    public override string ToString() => $"[!{Options}]";
}
