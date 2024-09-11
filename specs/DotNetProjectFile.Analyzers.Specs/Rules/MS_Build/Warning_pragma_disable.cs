namespace Rules.MS_Build.Warning_pragma_disable;

public class Guard
{
    [Test]
    public void disabled_issues()
        => new RemoveFolderNodes()
        .ForProject("SuppressIssues.cs")
        .Add(new IncludeProjectReferencesOnce())
        .HasIssue(new Issue("Proj0008", @"Remove folder node 'Third'.").WithSpan(20, 04, 20, 30));
}
