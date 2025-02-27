namespace DotNetProjectFile.Text.Globbing;

internal sealed class AnyChar : Segment
{
    /// <inheritdoc />
    public override int MinLength => 1;

    /// <inheritdoc />
    public override int MaxLength => 1;

    public override string ToString() => "?";
}
