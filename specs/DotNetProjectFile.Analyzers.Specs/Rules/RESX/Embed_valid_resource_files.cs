namespace Rules.RESX.Embed_valid_resource_files;

public class Reports
{
    [Test]
    public void unsorted_data()
        => new Resx.EmbedValidResourceFiles()
        .ForProject("ResxNoXml.cs")
        .HasIssue(
            new Issue("Proj2000", "Invalid XML: Unexpected end of file has occurred. The following elements are not closed: root. Line 1, position 7."));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantVB.vb")]
    public void sorted_data(string project)
         => new Resx.EmbedValidResourceFiles()
        .ForProject(project)
        .HasNoIssues();
}
