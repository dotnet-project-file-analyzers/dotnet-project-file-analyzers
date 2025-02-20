namespace Rules.MS_Build.Project_reference_include_should_exist;

public class Reports
{
    [Test]
    public void empty_includes()
        => new ProjectReferenceIncludeShouldExist()
        .ForProject("ProjectReferenceMissingInclude.cs")
        .HasIssue(Issue.WRN("Proj0033", "The Include './foo.csproj' of <ProjectReference> does not exist.").WithSpan(08, 04, 08, 47));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    public void Projects_with_analyzers(string project)
         => new ProjectReferenceIncludeShouldExist()
        .ForProject(project)
        .HasNoIssues();
}
