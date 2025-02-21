using System.IO;
using System.Xml.Serialization;

namespace DotNetProjectFile.NuGet.Packaging;

[XmlRoot(ElementName = "configuration")]
public sealed record NuGetConfigFile
{
    [XmlElement("config")]
    public NuGetConfigSection[] Configs { get; init; } = [];

    [Pure]
    public static NuGetConfigFile Load(Stream stream)
       => (NuGetConfigFile)Serializer.Deserialize(stream);

    private static readonly XmlSerializer Serializer = new(typeof(NuGetConfigFile));
}
