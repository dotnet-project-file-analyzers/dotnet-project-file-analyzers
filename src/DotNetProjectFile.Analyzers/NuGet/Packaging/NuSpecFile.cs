using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace DotNetProjectFile.NuGet.Packaging;

[XmlRoot(ElementName = "package")]
public sealed record NuSpecFile
{
    [XmlElement("metadata")]
    public Metadata Metadata { get; init; } = new();

    public override string ToString()
    {
        using var stream = new MemoryStream();
        Serializer.Serialize(stream, this);
        return Encoding.UTF8.GetString(stream.ToArray());
    }

    [Pure]
    public static NuSpecFile Load(Stream stream)
        => (NuSpecFile)Serializer.Deserialize(new NamespaceIgnorantReader(stream));

    private static readonly XmlSerializer Serializer = new(typeof(NuSpecFile));

    private sealed class NamespaceIgnorantReader(Stream stream) : XmlTextReader(stream)
    {
        public override string NamespaceURI => string.Empty;
    }
}
