using System.Xml.Serialization;

namespace DotNetProjectFile.NuGet.Packaging;

public sealed record Metadata
{
    [XmlElement("id")]
    public string? Id { get; init; }

    [XmlElement("version")]
    public string? Version { get; init; }

    [XmlElement("description")]
    public string? Description { get;init; }

    [XmlElement("authors")]
    public string? Authors { get; init; }

    [XmlElement("license")]
    public string? License { get; init; }
}
