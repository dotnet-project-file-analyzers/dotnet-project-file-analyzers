namespace Rules.MS_Build.Update_should_change_state;

public class Reports
{
    [Test]
    public void on_update_with_single_attribute() => new UpdateShouldChangeState()
        .ForInlineCsproj(@"
<Project Sdk=""Microsoft.NET.Sdk"">

  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Update=""SonarAnalyzer.CSharp"" />
  </ItemGroup>

</Project>")
        .HasIssue(
            Issue.WRN("Proj0046", "Update statement does not change anything").WithSpan(7, 04, 7, 54));
}
public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void compliant(string project) => new UpdateShouldChangeState()
        .ForProject(project)
        .HasNoIssues();

    [Test]
    public void updates_via_child_elements() => new UpdateShouldChangeState()
        .ForInlineCsproj(@"
<Project Sdk=""Microsoft.NET.Sdk"">

  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
  </PropertyGroup>

 <ItemGroup>
    <Content Update=""*.md"">
     <CopyToOutputDirectory>Always</CopyToOutputDirectory>
    </Content>
  </ItemGroup>

</Project>")
        .HasNoIssues();

    [Test]
    public void updates_via_attributes() => new UpdateShouldChangeState()
        .ForInlineCsproj(@"
<Project Sdk=""Microsoft.NET.Sdk"">

  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
  </PropertyGroup>

 <ItemGroup>
    <PackageReference Update=""SonarAnalyzer.CSharp"" Version=""10.12"" />
  </ItemGroup>

</Project>")
        .HasNoIssues();
}
