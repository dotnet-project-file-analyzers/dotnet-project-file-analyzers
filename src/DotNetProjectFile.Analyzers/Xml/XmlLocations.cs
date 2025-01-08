using Microsoft.CodeAnalysis.Text;

namespace DotNetProjectFile.Xml;

public sealed record XmlLocations
{
    public Location FullSpan => File.GetLocation(Positions.FullSpan);

    public Location InnerSpan => File.GetLocation(Positions.InnerSpan);

    public Location StartElement => File.GetLocation(Positions.StartElement);

    public Location EndElement => File.GetLocation(Positions.EndElement);

    public required ProjectFile File { get; init; }

    public required XmlPositions Positions { get; init; }
}
