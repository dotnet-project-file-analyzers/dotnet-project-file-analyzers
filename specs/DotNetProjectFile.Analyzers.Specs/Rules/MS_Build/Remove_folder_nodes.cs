namespace Rules.MS_Build.Remove_folder_nodes;

public class Reports
{
    [Test]
    public void folder_nodes()
        => new RemoveFolderNodes()
        .ForProject("FolderNodes.cs")
        .HasIssues(
            Issue.WRN("Proj0008", "Remove folder node 'SomeFolder'" /*..*/).WithSpan(08, 4, 08, 36),
            Issue.WRN("Proj0008", "Remove folder node 'OtherFolder'" /*.*/).WithSpan(12, 4, 12, 37));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_with_analyzers(string project)
         => new RemoveFolderNodes()
        .ForProject(project)
        .HasNoIssues();
}
