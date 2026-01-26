using DotNetProjectFile.Xml;
using Microsoft.CodeAnalysis.Text;
using System.Xml.Linq;

namespace XML.Attribute_locations;

public class Locates
{
    private const LoadOptions Options = LoadOptions.PreserveWhitespace | LoadOptions.SetLineInfo;

    public class Attribute
    {
        [Test]
        public void Without_whitespace()
        {
            var text = """<elm Include="TestValue" />""";
            var source = SourceText.From(text);
            var element = XElement.Parse(text, Options);
            var positions = AttributesPositions.New(element.Attribute("Include")!, source);

            source.ToString(positions.Name).Should().Be("Include");
            source.ToString(positions.Assignment).Should().Be("=");
            source.ToString(positions.Value).Should().Be("TestValue");
            source.ToString(positions.FullSpan).Should().Be("""
                Include="TestValue"
                """);

            positions.Should().Be(new AttributesPositions
            {
                Name = new(new(00, 05), new(00, 12)),
                Assignment = new(new(00, 12), new(00, 13)),
                Value = new(new(00, 14), new(00, 23)),
                FullSpan = new(new(00, 05), new(00, 24)),
            });
        }

        [Test]
        public void With_whitespace()
        {
            var text = """<elm Include = "TestValue" />""";
            var source = SourceText.From(text);
            var element = XElement.Parse(text, Options);
            var positions = AttributesPositions.New(element.Attribute("Include")!, source);

            source.ToString(positions.Name).Should().Be("Include");
            source.ToString(positions.Assignment).Should().Be("=");
            source.ToString(positions.Value).Should().Be("TestValue");
            source.ToString(positions.FullSpan).Should().Be("""
                Include = "TestValue"
                """);

            positions.Should().Be(new AttributesPositions
            {
                Name = new(new(00, 05), new(00, 12)),
                Assignment = new(new(00, 13), new(00, 14)),
                Value = new(new(00, 16), new(00, 25)),
                FullSpan = new(new(00, 05), new(00, 26)),
            });
        }
    }
}
