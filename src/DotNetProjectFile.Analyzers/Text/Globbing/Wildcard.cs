namespace DotNetProjectFile.Text.Globbing;

internal sealed class Wildcard : Segment
{
    /// <inheritdoc />
    public override int MinLength => 0;
    
    /// <inheritdoc />
    public override int MaxLength => int.MaxValue;
    
    /// <inheritdoc />
    public override string ToString() => "*";
}
