namespace Microsoft.CodeAnalysis.Text;

public static class TextSpanExtensions
{
    extension(SourceText sourceText)
    {
        /// <inheritdoc cref="TextLineCollection.GetTextSpan(LinePositionSpan)" />
        public TextSpan TextSpan(LinePositionSpan span)
            => sourceText.Lines.GetTextSpan(span);
    }
}
