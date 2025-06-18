namespace Rules.SLNX.Remove_commented_out_code;

public class Reports
{
    [Test]
    public void Faulty_indented() => new Slnx.RemoveCommentedOutCode()
        .ForSDkProject("SolutionFile")
        .HasIssue(Issue.WRN("Proj3002", "Remove the commented-out code")
            .WithSpan(03, 06, 03, 47)
            .WithPath("with-comment.slnx"));
}
