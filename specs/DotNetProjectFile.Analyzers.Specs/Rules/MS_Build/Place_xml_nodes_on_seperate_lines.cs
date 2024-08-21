namespace Rules.MS_Build.Place_xml_nodes_on_seperate_lines;

public class Reports
{
    [Test]
    public void on_same_line()
       => new PlaceXmlNodesOnSeperateLines()
       .ForProject("MultipleXmlNodesOnSingleLine.cs")
       .HasIssues(
           new Issue("Proj7001", "Move XML node '<TargetFramework>net8.0</TargetFramework>' to a new line.").WithSpan(2, 18, 2, 19),
           new Issue("Proj7001", "Move XML node '<PackageReference Include=\"Microsoft.CodeAnalysis.Workspaces.Common\" Version=\"4.10.0\" />' to a new line.").WithSpan(4, 14, 4, 101),
           new Issue("Proj7001", "Move XML node '<PackageReference Include=\"Qowaiv.Analyzers.CSharp\" Version=\"2.0.0\" />' to a new line.").WithSpan(6, 5, 6, 6),
           new Issue("Proj7001", "Move XML node '<Compile Include=\"../common/Code.cs\" />' to a new line.").WithSpan(9, 5, 9, 6));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project)
         => new PlaceXmlNodesOnSeperateLines()
        .ForProject(project)
        .HasNoIssues();
}
