namespace Microsoft.CodeAnalysis.Text;

public static class LinePositionExtensions
{
    public static LinePosition Expand(this LinePosition span, int right)
        => new(span.Line, span.Character + right);
}
