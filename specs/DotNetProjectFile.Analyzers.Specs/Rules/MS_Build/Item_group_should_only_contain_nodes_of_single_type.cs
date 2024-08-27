namespace Rules.MS_Build.Item_group_should_only_contain_nodes_of_single_type;

public class Reports
{
    [Test]
    public void on_multiple_types()
       => new ItemGroupShouldBeUniform()
       .ForProject("MixedItemGroup.cs")
       .HasIssue(
           new Issue("Proj0020", @"<ItemGroup> should only contain nodes of a single type.").WithSpan(6, 2, 9, 14));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project)
         => new ItemGroupShouldBeUniform()
        .ForProject(project)
        .HasNoIssues();
}
