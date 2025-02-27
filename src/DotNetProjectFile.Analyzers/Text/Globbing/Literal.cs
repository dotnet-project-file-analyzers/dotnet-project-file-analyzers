

namespace DotNetProjectFile.Text.Globbing;

internal sealed class Literal(string text) : Segment
{
    /// <inheritdoc />
    public string Text { get; } = text;

    /// <inheritdoc />
    public override int MinLength => Text.Length;

    /// <inheritdoc />
    public override int MaxLength => Text.Length;
    
    /// <inheritdoc />
    public override string ToString() => Text;
}
