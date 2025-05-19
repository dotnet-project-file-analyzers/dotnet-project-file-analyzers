namespace Rules.MS_Build.Publish_executables_only;

public class Reports
{
    [Test]
    public void on_implicit_library() => new PublishExeOnly()
        .ForInlineCsproj(@"
<Project Sdk=""Microsoft.NET.Sdk"">

  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <IsPublishable>true</IsPublishable>
  </PropertyGroup>

</Project>")
        .HasIssue(
            Issue.WRN("Proj0401", "Only executables should be publishable").WithSpan(04, 04, 04, 39));

    [Test]
    public void on_explicit_library() => new PublishExeOnly()
        .ForInlineCsproj(@"
<Project Sdk=""Microsoft.NET.Sdk"">

  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <IsPublishable>true</IsPublishable>
    <OutputType>Library</OutputType>
  </PropertyGroup>

</Project>")
    .HasIssue(
        Issue.WRN("Proj0401", "Only executables should be publishable").WithSpan(04, 04, 04, 39));

}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project) => new PublishExeOnly()
        .ForProject(project)
        .HasNoIssues();
}
