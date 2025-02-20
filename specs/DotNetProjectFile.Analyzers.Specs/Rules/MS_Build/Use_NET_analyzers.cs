namespace Rules.MS_Build.Use_NET_analyzers;

public class Reports
{
    [TestCase("DisableNetAnalyzers.cs")]
    [TestCase("NoEnableNetAnalyzers.cs")]
    public void on_not_enabled_NET_analyzers(string project)
       => new UseDotNetAnalyzers()
       .ForProject(project)
       .HasIssue(
            Issue.WRN("Proj1002", "Use Microsoft's .NET analyzers by setting <EnableNETAnalyzers> to true."));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    public void Projects_without_issues(string project)
         => new UseDotNetAnalyzers()
        .ForProject(project)
        .HasNoIssues();
}
