namespace Rules.MS_Build.Omit_XML_declaration;

public class Reports
{
    [Test]
    public void on_double_imports() => new OmitXmlDeclarations()
       .ForProject("XmlDeclaration.cs")
       .HasIssue(Issue.WRN("Proj1702", "Remove the XML declaration as it is redundant")
       .WithSpan(01, 00, 01, 32));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project) => new OmitXmlDeclarations()
        .ForProject(project)
        .HasNoIssues();
}
