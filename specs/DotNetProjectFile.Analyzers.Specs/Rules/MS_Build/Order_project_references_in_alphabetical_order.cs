namespace Rules.MS_Build.Order_project_references_in_alphabetical_order;

public class Reports
{
    [Test]
    public void on_not_alphabetical_order()
       => new OrderProjectReferencesAlphabetically()
       .ForProject("NonAlphabeticalProjectReferences.cs")
       .HasIssue(
           Issue.WRN("Proj0016", @"Project '../EmptyNodes/EmptyNodes.csproj' is not ordered alphabetically and should appear before '../FolderNodes/FolderNodes.csproj'").WithSpan(8, 04, 8, 66));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project)
         => new OrderProjectReferencesAlphabetically()
        .ForProject(project)
        .HasNoIssues();
}
