namespace Rules.MS_Build.Run_NuGet_security_audits_on_all;

public class Reports
{
    [Test]
    public void on_unspecified_NuGet_audit_mode()
       => new RunNuGetSecurityAuditsOnAll()
       .ForInlineCsproj(@"
<Project Sdk=""Microsoft.NET.Sdk"">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
  </PropertyGroup>

</Project>")
       .HasIssue(Issue.WRN("Proj0040", "Set <NugetAuditMode> to all").WithSpan(00, 00, 00, 32));

    [Test]
    public void on_NuGet_audit_direct_mode()
       => new RunNuGetSecurityAuditsOnAll()
       .ForInlineCsproj(@"
<Project Sdk=""Microsoft.NET.Sdk"">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <NuGetAuditMode>direct</NuGetAuditMode>
  </PropertyGroup>

</Project>")
       .HasIssue(Issue.WRN("Proj0040", "Set <NugetAuditMode> to all").WithSpan(04, 04, 04, 43));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project)
         => new RunNuGetSecurityAuditsOnAll()
        .ForProject(project)
        .HasNoIssues();
}
