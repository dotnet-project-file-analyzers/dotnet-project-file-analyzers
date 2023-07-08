namespace Specs.Rules.Remove_folder_nodes;

public class Reports
{
    [Test]
    public void folder_nodes()
        => new RemoveFolderNodes()
        .ForProject("FolderNodes.cs")
        .HasIssues(
            new Issue("Proj0008", "Remove folder node 'SomeFolder'.").WithSpan(08, 5, 08, 36),
            new Issue("Proj0008", "Remove folder node 'OtherFolder'.").WithSpan(12, 5, 12, 37));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantVB.vb")]
    public void Projects_with_analyzers(string project)
         => new RemoveFolderNodes()
        .ForProject(project)
        .HasNoIssues();
}
