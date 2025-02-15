using System.IO;
using System.Xml.Serialization;

namespace DotNetProjectFile.NuGet.Packaging;

[XmlRoot(ElementName = "package", Namespace = "http://schemas.microsoft.com/packaging/2013/05/nuspec.xsd")]
public sealed record NuSpecFile
{
    [XmlElement("metadata")]
    public Metadata Metadata { get; init; } = new();

    [Pure]
    public static NuSpecFile Load(Stream stream)
        => (NuSpecFile)Serializer.Deserialize(stream);

    private static readonly XmlSerializer Serializer = new(typeof(NuSpecFile));
}
