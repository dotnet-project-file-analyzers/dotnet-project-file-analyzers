namespace Rules.MS_Build.Remove_empty_nodes;

public class Reports
{
    [Test]
    public void empty_nodes()
        => new RemoveEmptyNodes()
        .ForProject("EmptyNodes.cs")
        .HasIssues(
            Issue.WRN("Proj0007", "Remove empty PropertyGroup node." /*.*/).WithSpan(10, 2, 10, 33),
            Issue.WRN("Proj0007", "Remove empty PropertyGroup node." /*.*/).WithSpan(12, 2, 12, 19),
            Issue.WRN("Proj0007", "Remove empty PropertyGroup node." /*.*/).WithSpan(14, 2, 14, 33),
            Issue.WRN("Proj0007", "Remove empty PropertyGroup node." /*.*/).WithSpan(16, 2, 19, 18),
            Issue.WRN("Proj0007", "Remove empty ItemGroup node." /*.....*/).WithSpan(21, 2, 21, 15),
            Issue.WRN("Proj0007", "Remove empty ItemGroup node."  /*....*/).WithSpan(23, 2, 24, 14),
            Issue.WRN("Proj0007", "Remove empty ImportGroup node." /*...*/).WithSpan(30, 2, 30, 17));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_with_analyzers(string project)
         => new RemoveEmptyNodes()
        .ForProject(project)
        .HasNoIssues();
}
