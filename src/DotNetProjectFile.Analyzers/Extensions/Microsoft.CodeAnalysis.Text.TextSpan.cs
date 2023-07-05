namespace Microsoft.CodeAnalysis.Text;

public static class TextSpanExtensions
{
    public static TextSpan TextSpan(this SourceText text, LinePositionSpan span)
    {
        var start = text.Lines[span.Start.Line];
        var end = text.Lines[span.End.Line];
        return new(start.Start + span.Start.Character, end.Start + span.End.Character);
    }
}
