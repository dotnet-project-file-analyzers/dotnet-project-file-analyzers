namespace Microsoft.CodeAnalysis.Text;

public static class LinePositionExtensions
{
    extension(LinePosition span)
    {
        public LinePosition Expand(int right)
            => new(span.Line, span.Character + right);
    }
}
