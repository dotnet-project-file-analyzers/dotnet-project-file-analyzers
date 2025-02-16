using System.Xml.Serialization;

namespace DotNetProjectFile.NuGet.Packaging;

public sealed record Metadata
{
    [XmlElement("id")]
    public string? Id { get; init; }

    [XmlElement("version")]
    public string? Version { get; init; }

    [XmlElement("license")]
    public MetadataLicense? License { get; init; }

    [XmlElement("developmentDependency")]
    public bool? DevelopmentDependency { get; init; }
}
