namespace Rules.SLNX.Track_TODO_tags;

public class Reports
{
    [Test]
    public void TODO_tags() => new Slnx.TrackToDoTags()
        .ForSdkProject("SolutionFile")
        .HasIssue(Issue.WRN("Proj3001", "Complete the task associated to this \"TODO\" comment")
            .WithSpan(02, 08, 02, 34)
            .WithPath("with-todo.slnx"));
}
