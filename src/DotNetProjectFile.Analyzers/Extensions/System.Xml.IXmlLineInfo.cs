using Microsoft.CodeAnalysis.Text;

namespace System.Xml;

internal static class XmlLineInfoExtensions
{
    extension(IXmlLineInfo info)
    {
        public LinePosition LinePosition()
            => new(info.LineNumber - 1, info.LinePosition - 1);
    }
}
