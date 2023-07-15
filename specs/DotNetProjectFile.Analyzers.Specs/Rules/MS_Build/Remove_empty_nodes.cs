namespace Rules.MS_Build.Remove_empty_nodes;

public class Reports
{
    [Test]
    public void empty_nodes()
        => new RemoveEmptyNodes()
        .ForProject("EmptyNodes.cs")
        .HasIssues(
            new Issue("Proj0007", "Remove empty PropertyGroup node.").WithSpan(10, 3, 10, 33),
            new Issue("Proj0007", "Remove empty PropertyGroup node.").WithSpan(12, 3, 12, 19),
            new Issue("Proj0007", "Remove empty PropertyGroup node.").WithSpan(14, 3, 14, 33),
            new Issue("Proj0007", "Remove empty PropertyGroup node.").WithSpan(16, 3, 19, 18),
            new Issue("Proj0007", "Remove empty ItemGroup node.").WithSpan(21, 3, 21, 15),
            new Issue("Proj0007", "Remove empty ItemGroup node.").WithSpan(23, 3, 24, 14),
            new Issue("Proj0007", "Remove empty ImportGroup node.").WithSpan(30, 3, 30, 17));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    [TestCase("CompliantVB.vb")]
    [TestCase("CompliantVBPackage.vb")]
    public void Projects_with_analyzers(string project)
         => new RemoveEmptyNodes()
        .ForProject(project)
        .HasNoIssues();
}
