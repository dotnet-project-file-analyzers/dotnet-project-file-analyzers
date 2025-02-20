namespace Rules.MS_Build.Run_NuGet_security_audits_automatically;

public class Reports
{
    [Test]
    public void on_disabled_NuGet_audit()
       => new RunNuGetSecurityAuditsAutomatically()
       .ForProject("DisabledNugetAudit.cs")
       .HasIssue(
           Issue.WRN("Proj0004", "Run NuGet security audits automatically."));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    [TestCase("NoNugetAudit.cs")]
    public void Projects_without_issues(string project)
         => new RunNuGetSecurityAuditsAutomatically()
        .ForProject(project)
        .HasNoIssues();
}
