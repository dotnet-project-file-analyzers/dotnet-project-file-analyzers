namespace Rules.MS_Build.Specify_NoWarn_rule_IDs_fully;

public class Reports
{
    [Test]
    public void IDs_specified_as_ints() => new SpecifyNoWarnRuleIdsFully()
        .ForInlineCsproj(@"
<Project Sdk=""Microsoft.NET.Sdk"">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <NoWarn>$(NoWarn);1825;NuGet1700;42</NoWarn>
  </PropertyGroup>

</Project>")
        .HasIssues(
            Issue.WRN("Proj0038", "Specify rule ID '1825' fully by adding its prefix").WithSpan(04, 04, 04, 48),
            Issue.WRN("Proj0038", "Specify rule ID '42' fully by adding its prefix").WithSpan(04, 04, 04, 48));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void project_files_as_additional(string project) => new SpecifyNoWarnRuleIdsFully()
        .ForProject(project)
        .HasNoIssues();
}

