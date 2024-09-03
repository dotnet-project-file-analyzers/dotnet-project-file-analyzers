namespace Rules.MS_Build.Ommit_XML_declaration;

public class Reports
{
    [Test]
    public void on_double_imports()
       => new OmitXmlDeclarations()
       .ForProject("XmlDeclaration.cs")
       .HasIssue(new Issue("Proj1702", "Remove the XML declaration as it is redundant.")
       .WithSpan(00, 00, 01, 00));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project)
         => new OmitXmlDeclarations()
        .ForProject(project)
        .HasNoIssues();
}
