namespace Rules.MS_Build.Build_actions_should_have_single_task;

public class Reports
{
    [Test]
    public void empty_nodes()
        => new BuildActionsShouldHaveSingleTask()
        .ForProject("MultipleBuildActionTasks.cs")
        .HasIssues(
            new Issue("Proj0021", "The <Content> defines multiple tasks.").WithSpan(12, 5, 12, 52),
            new Issue("Proj0021", "The <None> defines multiple tasks.").WithSpan(16, 5, 16, 50),
            new Issue("Proj0021", "The <AdditionalFiles> defines multiple tasks.").WithSpan(20, 5, 20, 67)
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
