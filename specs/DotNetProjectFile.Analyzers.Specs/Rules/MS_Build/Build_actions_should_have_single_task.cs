namespace Rules.MS_Build.Build_actions_should_have_single_task;

public class Reports
{
    [Test]
    public void empty_nodes()
        => new BuildActionsShouldHaveSingleTask()
        .ForProject("MultipleBuildActionTasks.cs")
        .HasIssues(
            Issue.WRN("Proj0021", "The <Content> defines multiple tasks." /*...*/).WithSpan(12, 04, 12, 52),
            Issue.WRN("Proj0021", "The <None> defines multiple tasks." /*......*/).WithSpan(16, 04, 16, 50),
            Issue.WRN("Proj0021", "The <AdditionalFiles> defines multiple tasks.").WithSpan(20, 04, 20, 67)
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
