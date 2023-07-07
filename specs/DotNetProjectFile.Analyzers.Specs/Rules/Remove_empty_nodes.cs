namespace Specs.Rules.Remove_empty_nodes;

internal class Remove_empty_nodes
{
    public class Reports
    {
        [Test]
        public void empty_nodes()
            => new RemoveEmptyNodes()
            .ForProject("EmptyNodes.cs")
            .HasIssues(
                new Issue("Proj0007", "Remove empty PropertyGroup node.").WithSpan(08, 3, 08, 33),
                new Issue("Proj0007", "Remove empty PropertyGroup node.").WithSpan(10, 3, 10, 19),
                new Issue("Proj0007", "Remove empty PropertyGroup node.").WithSpan(12, 3, 12, 33),
                new Issue("Proj0007", "Remove empty PropertyGroup node.").WithSpan(14, 3, 17, 18),
                new Issue("Proj0007", "Remove empty ItemGroup node.").WithSpan(19, 3, 19, 15),
                new Issue("Proj0007", "Remove empty ItemGroup node.").WithSpan(21, 3, 22, 14),
                new Issue("Proj0007", "Remove empty ImportGroup node.").WithSpan(28,3, 28,17));
    }

    public class Guards
    {
        [TestCase("CompliantCSharp.cs")]
        [TestCase("CompliantVB.vb")]
        public void Projects_with_analyzers(string project)
             => new RemoveEmptyNodes()
            .ForProject(project)
            .HasNoIssues();
    }
}
