namespace Rules.MS_Build.Project_reference_include_should_have_right_casing;

public class Reports
{
    [Test]
    public void case_mismatch() => new ProjectReferenceChecker()
        .ForProject("ProjectReferenceDifferentCasing.cs")
        .HasIssue(Issue
            .WRN("Proj0051", "The Include './foo.csproj' of <ProjectReference> does not exist")
            .WithSpan(08, 04, 08, 47));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    public void Projects_with_analyzers(string project) => new ProjectReferenceChecker()
        .ForProject(project)
        .HasNoIssues();
}
