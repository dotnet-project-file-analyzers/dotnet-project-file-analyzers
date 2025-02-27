
namespace DotNetProjectFile.Text.Globbing;

internal sealed class Sequence(string options) : Segment
{
    public string Options { get; } = options;

    public override int MinLength => 1;

    public override int MaxLength => 1;

    public override string ToString() => $"[{Options}]";
}
