namespace Rules.MS_Build.Fully_specify_NoWarn_rule_IDs;

public class Reports
{
    [Test]
    public void IDs_specified_as_ints() => new FullySpecifyNoWarnRuleIds()
        .ForInlineCsproj(@"
<Project Sdk=""Microsoft.NET.Sdk"">

  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <NoWarn>$(NoWarn);1825;NuGet1700;42</NoWarn>
  </PropertyGroup>

</Project>")
        .HasIssues(
            Issue.WRN("Proj0038", "Fully specify rule ID '1825' by adding its prefix").WithSpan(04, 04, 04, 48),
            Issue.WRN("Proj0038", "Fully specify rule ID '42' by adding its prefix").WithSpan(04, 04, 04, 48));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void project_files_as_additional(string project) => new FullySpecifyNoWarnRuleIds()
        .ForProject(project)
        .HasNoIssues();
}

