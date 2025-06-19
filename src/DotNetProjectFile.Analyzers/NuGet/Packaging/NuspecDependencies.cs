using System.Xml.Serialization;

namespace DotNetProjectFile.NuGet.Packaging;

public sealed record NuspecDependencies
{
    [XmlAttribute("targetFramework")]
    public string? TargetFramework { get; init; }

    [XmlElement("dependency")]
    public NuspecDependency[] Dependencies { get; init; } = [];
}
