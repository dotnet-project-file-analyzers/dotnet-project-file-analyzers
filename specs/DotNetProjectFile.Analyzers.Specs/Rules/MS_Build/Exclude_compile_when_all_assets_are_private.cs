namespace Rules.MS_Build.Exclude_compile_when_all_assets_are_private;

public class Reports
{
    [Test]
    public void on_version_and_prefix() => new ValidatePrivateAssets()
        .ForInlineCsproj(@"
<Project Sdk=""Microsoft.NET.Sdk"">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include=""DotNetProjectFile.Analyzers"" Version=""1.5.8"" PrivateAssets=""all"" />
  </ItemGroup>

</Project>")
        .HasIssue(
           Issue.WRN("Proj0037", @"ExcludeAssets should contain runtime when PrivateAssets=""all""")
           .WithSpan(07, 04, 07, 98));
}

public class Guards
{
    [Test]
    public void mulitple_excludes() => new ValidatePrivateAssets()
        .ForInlineCsproj(@"
<Project Sdk=""Microsoft.NET.Sdk"">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include=""DotNetProjectFile.Analyzers"" Version=""1.5.8"" PrivateAssets=""all"" ExcludeAssets=""compile;runtime"" />
  </ItemGroup>

</Project>")
        .HasNoIssues();

    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project) => new ValidatePrivateAssets()
        .ForProject(project)
        .HasNoIssues();
}
