namespace Rules.MS_Build.Build_action_include_should_exist;

public class Reports
{
    [Test]
    public void empty_includes()
        => new BuildActionIncludeShouldExist()
        .ForProject("EmptyInclude.cs")
        .HasIssues(
            new Issue("Proj0022", "The Include '*.txt' of <None> does not match any files.").WithSpan(8, 5, 8, 28),
            new Issue("Proj0022", "The Include 'DoesNotExist.cs' of <None> does not exist.").WithSpan(9, 5, 9, 38),
            new Issue("Proj0022", "The Include '*.vbproj' of <Content> does not match any files.").WithSpan(10, 5, 10, 43),
            new Issue("Proj0022", "The Include '**/*.bin' of <EmbeddedResource> does not match any files.").WithSpan(11, 5, 11, 43),
            new Issue("Proj0022", "The Include '*.unknown' of <AdditionalFiles> does not match any files.").WithSpan(12, 5, 12, 43)
        );
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    public void Projects_with_analyzers(string project)
         => new BuildActionIncludeShouldExist()
        .ForProject(project)
        .HasNoIssues();
}
