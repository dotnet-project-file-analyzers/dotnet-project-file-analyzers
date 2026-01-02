using Microsoft.CodeAnalysis.Text;
using System.Xml;

namespace DotNetProjectFile.Xml;

public sealed record AttributesPositions
{
    public LinePositionSpan FullSpan { get; init; }

    public LinePositionSpan Name { get; init; }

    public LinePositionSpan Assignment { get; init; }

    public LinePositionSpan Value { get; init; }

    public static AttributesPositions New(XAttribute attribute, SourceText text)
    {
        var info = attribute.LinePosition();
        var line = info.Line;
        var offset = info.Character;
        LinePositionSpan? name = null;
        LinePositionSpan? assignment = null;
        LinePosition? start = null;
        LinePosition? end = null;

        var str = text.Lines[line].ToString()[offset..];

        var p = -1;

        while (++p < str.Length && end is null)
        {
            var ch = str[p];

            if (name is null && char.IsWhiteSpace(ch))
            {
                name = new(info, new(line, offset + p));
            }
            else if (assignment is null && ch is '=')
            {
                name ??= new(info, new(line, offset + p));
                assignment = new(new(line, offset + p), new(line, offset + p + 1));
            }
            else if (start is null && ch is '"')
            {
                start = new(line, offset + p + 1);
            }
            else if (end is null && ch is '"')
            {
                end = new(line, offset + p);
            }
        }

        return new()
        {
            Name = name.GetValueOrDefault(),
            Assignment = assignment.GetValueOrDefault(),
            Value = new(start.GetValueOrDefault(), end.GetValueOrDefault()),
            FullSpan = new(info, end.GetValueOrDefault().Expand(1)),
        };
    }
}
