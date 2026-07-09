namespace Rules.MS_Build.Project_reference_include_should_exist;

public class Reports
{
    [Test]
    public void empty_includes() => new ProjectReferenceChecker()
        .ForProject("ProjectReferenceMissingInclude.cs")
        .HasIssue(Issue
            .WRN("Proj0033", "The Include './foo.csproj' of <ProjectReference> does not exist")
            .WithSpan(07, 04, 07, 47));
}

public class Guards
{
    [Test]
    public void Projects_with_analyzers() => new ProjectReferenceChecker()
        .ForProject("CompliantCSharp.cs")
        .HasNoIssues();
}
