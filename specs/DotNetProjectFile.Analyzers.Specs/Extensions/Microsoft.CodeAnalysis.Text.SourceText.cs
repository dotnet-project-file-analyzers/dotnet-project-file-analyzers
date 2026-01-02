namespace Microsoft.CodeAnalysis.Text;

internal static class SourceTextExtensions
{
    extension(SourceText source)
    {
        public string ToString(LinePositionSpan span)
            => source.ToString(source.Lines.GetTextSpan(span));
    }
}
