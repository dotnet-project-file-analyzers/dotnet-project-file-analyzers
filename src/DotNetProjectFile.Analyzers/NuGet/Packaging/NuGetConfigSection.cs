using System.Xml.Serialization;

namespace DotNetProjectFile.NuGet.Packaging;

public sealed record NuGetConfigSection
{
    [XmlElement("add")]
    public NuGetConfigKeyValue[] KeyValues { get; init; } = [];
}
