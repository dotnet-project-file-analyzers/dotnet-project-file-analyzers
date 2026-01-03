using Microsoft.CodeAnalysis.Text;
using System.Xml;

namespace DotNetProjectFile.Xml;

/// <summary>
/// Represents positions of the parts that make an XML attribute.
/// </summary>
public sealed record AttributesPositions
{
    /// <summary>THe full span.</summary>
    public LinePositionSpan FullSpan { get; init; }

    /// <summary>The span containing the attribute name.</summary>
    /// <remarks>
    /// This includes the namespace if specified.
    /// </remarks>
    public LinePositionSpan Name { get; init; }

    /// <summary>The span containing the <c>=</c> sign.</summary>
    public LinePositionSpan Assignment { get; init; }

    /// <summary>The value of the attribute.</summary>
    /// <remarks>
    /// The surrounding (double) qoutes are excluded.
    /// </remarks>
    public LinePositionSpan Value { get; init; }

    /// <summary>Creates new <see cref="AttributesPositions"/>.</summary>
    [Pure]
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
