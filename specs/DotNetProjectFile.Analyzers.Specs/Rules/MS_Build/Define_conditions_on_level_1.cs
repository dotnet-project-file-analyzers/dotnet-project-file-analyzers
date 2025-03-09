namespace Rules.MS_Build.Define_conditions_on_level_1;

public class Reports
{
    [Test]
    public void conditions_on_other_levels() => new DefineConditionsOnLevel1()
        .ForInlineCsproj(@"
<Project Sdk=""Microsoft.NET.Sdk"">

  <PropertyGroup>
    <TargetFrameworks Condition=""'$(OS)' == 'Windows_NT'"">net9.0;net462</TargetFrameworks>
    <TargetFrameworks Condition=""'$(OS)' != 'Windows_NT'"">net9.0</TargetFrameworks>
  </PropertyGroup>
  
  <PropertyGroup Condition=""'1' == '1'"">
    <ProductName>Test project</ProductName>
  </PropertyGroup>

  <Choose>
    <When Condition=""'$(TargetFramework)'=='net9.0'"">
      <PropertyGroup>
        <IsPackable>true</IsPackable>
      </PropertyGroup>
      <ItemGroup>
        <Folder Include=""When/"" />
      </ItemGroup>
    </When>
    <Otherwise>
      <PropertyGroup>
        <IsPackable>false</IsPackable>
      </PropertyGroup>
      <ItemGroup>
        <Folder Include=""Otherwise/"" />
      </ItemGroup>
    </Otherwise>
  </Choose>

  <ItemGroup>
    <PackageReference Include=""Newtonsoft.Json"" Version=""13.0.3"" Condition=""'$(OS)' == 'Windows_NT'"" />
  </ItemGroup>

</Project>
")
        .HasIssue(Issue.WRN("Proj0028", "Move the condition to the parent <ItemGroup>")
        .WithSpan(31, 04, 31, 103));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project)
         => new DefineConditionsOnLevel1()
        .ForProject(project)
        .HasNoIssues();
}
