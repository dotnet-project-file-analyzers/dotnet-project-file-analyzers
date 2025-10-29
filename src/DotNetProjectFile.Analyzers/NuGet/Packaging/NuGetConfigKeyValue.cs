using System.Xml.Serialization;

namespace DotNetProjectFile.NuGet.Packaging;

public sealed record NuGetConfigKeyValue
{
    [XmlAttribute("key")]
    public string? Key { get; init; }

    [XmlAttribute("value")]
    public string? Value { get; init; }
}
