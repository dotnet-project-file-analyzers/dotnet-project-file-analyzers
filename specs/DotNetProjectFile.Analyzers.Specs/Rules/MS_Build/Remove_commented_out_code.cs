namespace Rules.MS_Build.Remove_commented_out_code;

public class Reports
{
    [Test]
    public void on_XML_commented_out() => new RemoveCommentedOutCode()
       .ForInlineCsproj(@"
<Project Sdk=""Microsoft.NET.Sdk"">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <!-- ImplicitUsings>enable</ImplicitUsings -->
  </PropertyGroup>

  <ItemGroup>
    <!-- Reconsider adding this
    <GlobalPackageReference Include=""DotNetProjectFile.Analyzers"" Version=""1.5.8"" />
    -->
  </ItemGroup>

</Project>")
       .HasIssues(
           Issue.WRN("Proj3002", "Remove the commented-out code").WithSpan(04, 08, 04, 47),
           Issue.WRN("Proj3002", "Remove the commented-out code").WithSpan(08, 08, 10, 04));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project) => new RemoveCommentedOutCode()
        .ForProject(project)
        .HasNoIssues();
}
