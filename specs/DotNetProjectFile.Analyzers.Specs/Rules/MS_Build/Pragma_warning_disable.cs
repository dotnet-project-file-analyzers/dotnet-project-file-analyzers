namespace Rules.MS_Build.Pragma_warning_disable;

public class Guard
{
    [Test]
    public void disabled()
        => new RemoveFolderNodes()
        .ForProject("SuppressIssues.cs")
        .HasIssue(new Issue("Proj0008", @"Remove folder node 'Third'").WithSpan(19, 05, 19, 30));
}
