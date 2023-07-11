using Microsoft.CodeAnalysis.Text;
using System.IO;
using System.Xml;

namespace DotNetProjectFile.CodeAnalysis;

public static class Locations
{
    public static Location FromXml(FileInfo file, SourceText text, IXmlLineInfo info)
    {
        var linePositionSpan = info.LinePositionSpan();
        var textSpan = text.TextSpan(linePositionSpan);
        return Location.Create(file.FullName, textSpan, linePositionSpan);
    }
}
