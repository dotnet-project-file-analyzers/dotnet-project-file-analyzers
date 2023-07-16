namespace Rules.RESX.Embed_valid_resource_files;

public class Reports
{
    [Test]
    public void no_XML()
        => new Resx.EmbedValidResourceFiles()
        .ForProject("ResxNoXml.cs")
        .HasIssue(
            new Issue("Proj2000", "Resource file contains no XML."));

    [Test]
    public void missing_headers()
        => new Resx.EmbedValidResourceFiles()
        .ForProject("ResxNoHeaders.cs")
        .HasIssues(
            new Issue("Proj2000", @"Resource file misses <resheader name=""resmimetype""> with value ""text/microsoft-resx""."),
            new Issue("Proj2000", @"Resource file misses <resheader name=""reader""> with value ""System.Resources.ResXResourceReader""."),
            new Issue("Proj2000", @"Resource file misses <resheader name=""writer""> with value ""System.Resources.ResXResourceWriter""."));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    public void sorted_data(string project)
         => new Resx.EmbedValidResourceFiles()
        .ForProject(project)
        .HasNoIssues();
}
