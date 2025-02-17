using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace DotNetProjectFile.NuGet.Packaging;

public sealed record MetadataLicense : IXmlSerializable
{
    [XmlAttribute("type")]
    public string? Type { get; set; }

    public string? Value { get; set; }

    public XmlSchema? GetSchema() => null;

    public void ReadXml(XmlReader reader)
    {
        Type = reader.GetAttribute("type");
        Value = reader.ReadElementString();

    }

    public void WriteXml(XmlWriter writer)
    {
        writer.WriteAttributeString("type", Type);
        writer.WriteValue(Value);
    }
}
