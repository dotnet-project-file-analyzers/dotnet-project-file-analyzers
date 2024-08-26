using DotNetProjectFile.Xml;
using System.Xml.Linq;

namespace XML.XML_locations;

public class Locates
{
    private const LoadOptions Options = LoadOptions.PreserveWhitespace | LoadOptions.SetLineInfo;

    [Test]
    public void Empty_element()
    {
        var element = XElement.Parse("<elm></elm>", Options);
        var positions = XmlPositions.New(element);
        positions.Should().Be(new XmlPositions
        {
            StartElement = new(new(00, 00), new(00, 04)),
            EndElement = new(new(00, 05), new(00, 11)),
        });
    }

    [Test]
    public void Nested_empty_element()
    {
        var element = XElement.Parse("<parent><elm></elm></parent>", Options);

        var positions = XmlPositions.New(element.Elements().First());
        positions.Should().Be(new XmlPositions
        {
            StartElement = new(new(00, 08), new(00, 12)),
            EndElement = new(new(00, 13), new(00, 19)),
        });
    }

    [Test]
    public void element_with_value()
    {
        var element = XElement.Parse("<elm>1234</elm>", Options);

        var positions = XmlPositions.New(element);
        positions.Should().Be(new XmlPositions
        {
            StartElement = new(new(00, 00), new(00, 04)),
            EndElement = new(new(00, 09), new(00, 15)),
        });
    }

    [Test]
    public void self_closing_element()
    {
        var element = XElement.Parse("<elm/>", Options);

        var positions = XmlPositions.New(element);
        positions.Should().Be(new XmlPositions
        {
            StartElement = new(new(00, 00), new(00, 05)),
            EndElement = new(new(00, 00), new(00, 05)),
        });
    }


    [Test]
    public void element_with_line_end() 
        => XmlPositions.New(XElement.Parse("<elm>1234</elm>\n", Options))
        .Should().Be(new XmlPositions
        {
            StartElement = new(new(00, 00), new(00, 04)),
            EndElement = new(new(00, 09), new(00, 15)),
        });
}
