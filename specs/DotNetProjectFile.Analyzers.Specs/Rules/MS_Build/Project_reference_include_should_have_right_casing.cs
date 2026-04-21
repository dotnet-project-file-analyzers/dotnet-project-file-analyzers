namespace Rules.MS_Build.Project_reference_include_should_have_right_casing;

public class Reports
{
    [Test]
    public void case_mismatch() => new ProjectReferenceChecker()
        .ForProject("ProjectReferenceDifferentCasing.cs")
        .HasIssue(Issue
            .WRN("Proj0051", "The casing of './foo.csproj' of differs from the file 'FOO.csproj' on disk")
            .WithSpan(07, 04, 07, 47));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    public void Projects_with_analyzers(string project) => new ProjectReferenceChecker()
        .ForProject(project)
        .HasNoIssues();
}
