namespace Rules.Generic.Remove_commented_out_code;

public class Reports
{
    [Test]
    public void on_XML_commented_out_in_MS_Build() => new RemoveCommentedOutCode()
       .ForInlineCsproj("""
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <!-- ImplicitUsings>enable</ImplicitUsings -->
  </PropertyGroup>

  <ItemGroup>
    <!-- Reconsider adding this
    <GlobalPackageReference Include="DotNetProjectFile.Analyzers" Version="1.5.8" />
    -->
  </ItemGroup>

</Project>
""")
       .HasIssues(
           Issue.WRN("Proj3002", "Remove the commented-out code").WithSpan(04, 08, 04, 47),
           Issue.WRN("Proj3002", "Remove the commented-out code").WithSpan(08, 08, 10, 04));

    [Test]
    public void on_XML_commented_out_in_RESX() => new Resx.RemoveCommentedOutCode()
       .ForProject("ResxCommentedOut.cs")
       .HasIssues(
           Issue.WRN("Proj3002", "Remove the commented-out code").WithSpan(17, 06, 21, 02).WithPath("Resources.resx"),
           Issue.WRN("Proj3002", "Remove the commented-out code").WithSpan(25, 06, 25, 19).WithPath("Resources.resx"));
}

public class Guards
{
    [Test]
    public void regular_comment() => new RemoveCommentedOutCode()
      .ForInlineCsproj("""
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <!-- Only .NET 8.0 will work -->    
    <TargetFramework>net10.0</TargetFramework>
  </PropertyGroup>

</Project>
""")
      .HasNoIssues();

    [Test]
    public void TODO_comment() => new RemoveCommentedOutCode()
      .ForInlineCsproj("""
        
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <!-- TODO add .NET 9.0 too -->    
    <TargetFramework>net10.0</TargetFramework>
  </PropertyGroup>

</Project>
""")
      .HasNoIssues();

    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues_for_MS_Build(string project) => new RemoveCommentedOutCode()
        .ForProject(project)
        .HasNoIssues();

    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues_for_RESX(string project) => new Resx.RemoveCommentedOutCode()
        .ForProject(project)
        .HasNoIssues();
}
