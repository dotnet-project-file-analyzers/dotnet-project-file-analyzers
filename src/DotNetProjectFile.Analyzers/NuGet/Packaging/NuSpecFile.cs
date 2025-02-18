using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace DotNetProjectFile.NuGet.Packaging;

[XmlRoot(ElementName = "package")]
public sealed record NuSpecFile
{
    [XmlElement("metadata")]
    public Metadata Metadata { get; init; } = new();

    [Pure]
    public static NuSpecFile Load(Stream stream)
        => (NuSpecFile)Serializer.Deserialize(new NamespaceIgnorantReader(stream));

    private static readonly XmlSerializer Serializer = new(typeof(NuSpecFile));

    private sealed class NamespaceIgnorantReader : XmlTextReader
    {
        public NamespaceIgnorantReader(Stream stream) : base(stream) { }

        public override string NamespaceURI => string.Empty;
    }
}
