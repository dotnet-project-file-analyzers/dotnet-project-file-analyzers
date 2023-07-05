namespace Rules.Run_NuGet_security_audits_automatically;

public class Reports
{
    [Test]
    public void on_disabled_NuGet_audit()
       => new RunNuGetSecurityAuditsAutomatically()
       .ForProject("DisabledNugetAudit.cs")
       .HasIssue(
           new Issue("Proj0004", "Run NuGet security audits automatically.").WithSpan(0, 1, 0, 2));

    [Test]
    public void on_not_configured_NuGet_audit()
        => new RunNuGetSecurityAuditsAutomatically()
        .ForProject("NoNugetAudit.cs")
        .HasIssue(
            new Issue("Proj0004", "Run NuGet security audits automatically.").WithSpan(0, 1, 0, 2));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantVB.vb")]
    public void Projects_without_issues(string project)
         => new RunNuGetSecurityAuditsAutomatically()
        .ForProject(project)
        .HasNoIssues();
}
