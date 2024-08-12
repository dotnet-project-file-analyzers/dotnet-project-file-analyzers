using Microsoft.CodeAnalysis.Text;
using System.Xml;

namespace DotNetProjectFile.CodeAnalysis;

public static class Locations
{
    public static Location FromXml(IOFile file, SourceText text, IXmlLineInfo info)
    {
        var linePositionSpan = info.LinePositionSpan();
        var textSpan = text.TextSpan(linePositionSpan);
        return Location.Create(file.ToString(), textSpan, linePositionSpan);
    }
}
