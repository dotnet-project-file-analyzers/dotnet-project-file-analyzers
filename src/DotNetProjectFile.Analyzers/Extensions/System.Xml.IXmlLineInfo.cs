using Microsoft.CodeAnalysis.Text;
using System.Xml.Linq;

namespace System.Xml;

public static class XmlLineInfoExtensions
{
    public static LinePositionSpan LinePositionSpan(this IXmlLineInfo info)
    {
        var start = info.LinePositionStart();
        var end = info is XElement element && element.NextNode is IXmlLineInfo next
            ? next.LinePositionStart()
            : start.Expand(right: +1);

        return new(start, end);
    }

    private static LinePosition LinePositionStart(this IXmlLineInfo info)
        => new(info.LineNumber - 1, info.LinePosition - 1);
}
