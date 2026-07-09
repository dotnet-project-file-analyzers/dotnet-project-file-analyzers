namespace Rules.MS_Build.Run_NuGet_security_audits_automatically;

public class Reports
{
    [Test]
    public void on_disabled_NuGet_audit() => new RunNuGetSecurityAuditsAutomatically().ForInlineCsproj("""
            
           <Project Sdk="Microsoft.NET.Sdk">

             <PropertyGroup>
               <TargetFramework>net10.0</TargetFramework>
               <NuGetAudit>false</NuGetAudit>
             </PropertyGroup>

           </Project>
           """)
       .HasIssue(Issue.WRN("Proj0004", "Run NuGet security audits automatically"));
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
