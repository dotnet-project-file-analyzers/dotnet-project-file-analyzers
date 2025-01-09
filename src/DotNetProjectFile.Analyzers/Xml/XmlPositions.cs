using Microsoft.CodeAnalysis.Text;
using System.Xml;

namespace DotNetProjectFile.Xml;

public sealed record XmlPositions
{
    public LinePositionSpan FullSpan => new(StartElement.Start, EndElement.End);

    public LinePositionSpan InnerSpan => new(StartElement.End, EndElement.Start);

    public LinePositionSpan StartElement { get; init; }

    public LinePositionSpan EndElement { get; init; }

    public bool IsSelfClosing => StartElement == EndElement;

    [Pure]
    public XmlLocations Locations(ProjectFile file) => new() { File = file, Positions = this };

    public static XmlPositions New(XElement element)
    {
        LinePosition? start = null;
        LinePosition? next = null;
        LinePosition? end = null;

        using var reader = element.CreateReader();

        var info = (IXmlLineInfo)reader;

        reader.MoveToContent();

        var depth = reader.Depth;

        do
        {
            if (reader.NodeType == XmlNodeType.Element && depth == reader.Depth && start is null)
            {
                start = info.LinePosition().Expand(-1);
                if (reader.IsEmptyElement)
                {
                    end = start;
                }
            }
            else if (reader.NodeType == XmlNodeType.EndElement && depth == reader.Depth)
            {
                end ??= info.LinePosition().Expand(-2);
                next ??= end.Value.Expand(-1);
            }
            else if (reader.NodeType != XmlNodeType.Element && reader.NodeType != XmlNodeType.None)
            {
                next ??= info.LinePosition().Expand(-1);
            }
        }
        while (reader.Read());

        var final = element.NextNode?.LinePosition()
            ?? info.LinePosition().Expand(element.Name.LocalName.Length + 1);

        return new()
        {
            StartElement = new(start.GetValueOrDefault(), next ?? final),
            EndElement = new(end.GetValueOrDefault(), final),
        };
    }

    public static XmlPositions New(XComment comment)
    {
        LinePosition start = comment.LinePosition();
        LinePosition end = default;
        using var reader = comment.Parent.CreateReader();

        var info = (IXmlLineInfo)reader;

        reader.MoveToContent();

        do
        {
            if (reader.NodeType != XmlNodeType.Comment)
            {
                var test = info.LinePosition();
                if (test > start)
                {
                    if (reader.NodeType == XmlNodeType.EndElement)
                    {
                        test = test.Expand(-1);
                    }
                    end = test;
                }
            }
        }
        while (reader.Read() && end == default);

        if (end == default)
        {
            end = start.Expand(comment.Value.Length + 3);
        }

        return new()
        {
            // Length of '<!--' is 4 and should be subtracted.
            StartElement = new(start.Expand(-4), start),
            // Length of '-->' is 3 and should be subtracted.
            EndElement = new(end.Expand(-3), end),
        };
    }
}
