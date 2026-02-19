namespace Rules.MS_Build.Remove_IncludeAssets_when_redundant;

public class Reports
{
    [Test]
    public void redundant_includes() => new ValidatePrivateAssets()
       .ForProject("IncludeAssets.cs")
       .HasIssues(
           Issue.WRN("Proj0026", "Remove <IncludeAssets> as it is redundant when all assets are private" /*.*/).WithSpan(20, 04, 23, 23),
           Issue.WRN("Proj0026", "Remove IncludeAssets as it is redundant when all assets are private" /*...*/).WithSpan(24, 04, 24, 187));


    [Test]
    public void previously_reported_NuGet_Protocol() => new ValidatePrivateAssets()
       .ForInlineCsproj(@"
<Project Sdk=""Microsoft.NET.Sdk"">

  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Update=""SonarAnalyzer.CSharp"">
      <PrivateAssets>all</PrivateAssets>
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    </PackageReference>
  </ItemGroup>

</Project>")
       .HasIssues(
           Issue.WRN("Proj0026", "Remove <IncludeAssets> as it is redundant when all assets are private").WithSpan(07, 04, 10, 23),
           Issue.WRN("Proj0037", "ExcludeAssets should contain runtime when PrivateAssets=\"all\"" /*.*/).WithSpan(07, 04, 10, 23));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project) => new ValidatePrivateAssets()
        .ForProject(project)
        .HasNoIssues();
}
