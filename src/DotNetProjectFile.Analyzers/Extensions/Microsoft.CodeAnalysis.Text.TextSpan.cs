namespace Microsoft.CodeAnalysis.Text;

public static class TextSpanExtensions
{
    /// <inheritdoc cref="TextLineCollection.GetTextSpan(LinePositionSpan)" />
    public static TextSpan TextSpan(this SourceText text, LinePositionSpan span)
        => text.Lines.GetTextSpan(span);
}
