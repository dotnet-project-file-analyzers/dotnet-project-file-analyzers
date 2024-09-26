using Microsoft.CodeAnalysis.Text;

namespace System.Xml;

internal static class XmlLineInfoExtensions
{
    public static LinePosition LinePosition(this IXmlLineInfo info)
        => new(info.LineNumber - 1, info.LinePosition - 1);
}
