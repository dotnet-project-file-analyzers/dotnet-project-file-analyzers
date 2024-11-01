using System.Resources;

namespace Rules.RESX.Escape_XML_nodes_resource_values;

public class Reports
{
    [Test]
    public void on_value_with_HTML() => new Resx.EscapeXmlNodesResourceValues()
        .ForProject("ResxWithHtmlContent.cs")
        .HasIssue(new Issue("Proj2005", "Escape the XML node in 'Html'.").WithSpan(12, 04, 12, 30));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project) => new Resx.EscapeXmlNodesResourceValues()
        .ForProject(project)
        .HasNoIssues();
}
