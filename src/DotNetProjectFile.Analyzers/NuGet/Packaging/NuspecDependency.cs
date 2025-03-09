using System.Xml.Serialization;

namespace DotNetProjectFile.NuGet.Packaging;

public sealed record NuspecDependency
{
    [XmlAttribute("id")]
    public string? Id { get; init; }

    [XmlAttribute("version")]
    public string? Version { get; init; }

    [XmlAttribute("exclude")]
    public string? Exclude { get; init; }

    public PackageVersionInfo Info => new(Id!, Version);
}
