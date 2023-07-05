using Microsoft.CodeAnalysis.Text;
using System.Xml.Linq;

namespace System.Xml;

public static class XmlLineInfoExtensions
{
    public static LinePositionSpan LinePositionSpan(this IXmlLineInfo info)
    {
        var start = info.LinePositionStart();
        var end = info is XElement element && element.NextNode is IXmlLineInfo next
            ? new LinePosition(next.LineNumber - 1, next.LinePosition - 2)
            : new LinePosition(info.LineNumber - 1, info.LinePosition + 1);
        return new(start, end);
    }

    private static LinePosition LinePositionStart(this IXmlLineInfo info)
        => new(info.LineNumber - 1, info.LinePosition - 1);
}
