namespace Rules.MS_Build.Build_actions_should_have_single_task;

public class Reports
{
    [Test]
    public void empty_nodes()
        => new BuildActionsShouldHaveSingleTask()
        .ForProject("MultipleBuildActionTasks.cs")
        .HasIssues(
            new Issue("Proj0021", "The Include 'DoesNotExist.cs' of <None> does not exist.").WithSpan(9, 5, 9, 38),
            new Issue("Proj0021", "The Include '*.vbproj' of <Content> does not contain any files.").WithSpan(10, 5, 10, 43),
            new Issue("Proj0021", "The Include '**/*.bin' of <EmbeddedResource> does not contain any files.").WithSpan(10, 5, 10, 43),
            new Issue("Proj0021", "The Include '*.unknown' of <AdditionalFiles> does not contain any files.").WithSpan(12, 5, 12, 43)
        );
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    public void Projects_with_analyzers(string project)
         => new BuildActionsShouldHaveSingleTask()
        .ForProject(project)
        .HasNoIssues();
}
