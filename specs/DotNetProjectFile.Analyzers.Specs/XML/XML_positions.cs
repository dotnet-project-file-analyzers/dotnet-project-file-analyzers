using DotNetProjectFile.Xml;
using System.Xml.Linq;

namespace XML.XML_locations;

public class Locates
{
    private const LoadOptions Options = LoadOptions.PreserveWhitespace | LoadOptions.SetLineInfo;

    public class Element
    {
        [Test]
        public void empty()
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
        public void nested_empty()
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
        public void with_value()
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
        public void self_closing()
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
        public void with_line_end()
            => XmlPositions.New(XElement.Parse("<elm>1234</elm>\n", Options))
            .Should().Be(new XmlPositions
            {
                StartElement = new(new(00, 00), new(00, 04)),
                EndElement = new(new(00, 09), new(00, 15)),
            });

        /// <remarks>This behavior can be improved on.</remarks>
        [Test]
        public void with_attributes()
           => XmlPositions.New(XElement.Parse(@"<elm Id=""SomeId"" />", Options))
           .Should().Be(new XmlPositions
           {
               StartElement = new(new(00, 00), new(00, 05)),
               EndElement = new(new(00, 00), new(00, 05)),
           });

        /// <remarks>This behavior can be improved on.</remarks>
        [Test]
        public void with_child()
           => XmlPositions.New(XElement.Parse(
    @"<elm>
  <child>value</child>
</elm>", Options))
           .Should().Be(new XmlPositions
           {
               StartElement = new(new(00, 00), new(00, 04)),
               EndElement = new(new(02, 00), new(02, 06)),
           });

        [Test]
        public void self_closing_with_attributes()
            => XmlPositions.New(XElement.Parse(
                @"<Project>
  <Compile Include=""../common/Code.cs"" />
</Project>", Options).Elements().First())
            .Should().Be(new XmlPositions
            {
                StartElement = new(new(01, 02), new(01, 41)),
                EndElement = new(new(01, 02), new(01, 41)),
            });
    }

    public class Comment
    {
        [Test]
        public void level_1()
        {
            var comment = XElement.Parse("<elm><!-- LEVEL 1 --></elm>", Options)
                .DescendantNodes().OfType<XComment>().Single();

            var positions = XmlPositions.New(comment);
            positions.Should().Be(new XmlPositions
            {
                StartElement = new(new(00, 05), new(00, 09)),
                EndElement = new(new(00, 19), new(00, 22)),
            });
        }

        [Test]
        public void before_other()
        {
            var comment = XElement.Parse("<elm><!-- LEVEL 1 --><item /></elm>", Options)
                .DescendantNodes().OfType<XComment>().Single();

            var positions = XmlPositions.New(comment);
            positions.Should().Be(new XmlPositions
            {
                StartElement = new(new(00, 05), new(00, 09)),
                EndElement = new(new(00, 19), new(00, 22)),
            });
        }

        [Test]
        public void multi_line()
        {
            var comment = XElement.Parse(@"<elm>
  <!--
  // TODO: Should be regonized correctly.
  -->
  <item />
</elm>", Options)
                .DescendantNodes().OfType<XComment>().Single();

            var positions = XmlPositions.New(comment);
            positions.Should().Be(new XmlPositions
            {
                StartElement = new(new(01, 02), new(01, 06)),
                EndElement = new(new(03, 02), new(03, 05)),
            });
        }
    }
}
