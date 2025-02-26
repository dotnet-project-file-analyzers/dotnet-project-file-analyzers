namespace Rules.MS_Build.Remove_None_when_redundant;

public class Reports
{
    [Test]
    public void on_not_alphabetical_order() => new RemoveNoneWhenRedundant()
       .ForInlineCsproj(@"
<Project Sdk=""Microsoft.NET.Sdk"">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
  </PropertyGroup>

  <ItemGroup>
    <None Include=""*"" />
    <None Remove=""License.md"" />
  </ItemGroup>

</Project>")
       .HasIssue(
           Issue.WRN("Proj0036", "Remove <None> as it is redundant").WithSpan(8, 04, 8, 32));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project) => new RemoveNoneWhenRedundant()
        .ForProject(project)
        .HasNoIssues();
}
