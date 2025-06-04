using AwesomeAssertions;

namespace Rules.MS_Build.Use_Sonar_analyzers;

public class Reports
{
    [Test]
    public void missing_analyzer_for_CSharp() => new UseSonarAnalyzers()
        .ForProject("EmptyProject.cs")
        .HasIssue(Issue.WRN("Proj1003", "Add SonarAnalyzer.CSharp"));

    [Test]
    public void missing_analyzer_for_Visual_Basic() => new UseSonarAnalyzers()
        .ForProject("EmptyProjectVB.vb")
        .HasIssue(Issue.WRN("Proj1003", "Add SonarAnalyzer.VisualBasic"));

    [Test]
    public void only_added_as_project_version() => new UseSonarAnalyzers().ForInlineCsproj(@"
<Project Sdk=""Microsoft.NET.Sdk"">

    <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    </PropertyGroup>

  <ItemGroup>
    <PackageVersion Include=""SonarAnalyzer.CSharp"" Version=""10.6.0.109712"" />
  </ItemGroup>

</Project>")
        .HasIssue(Issue.WRN("Proj1003", "Add SonarAnalyzer.CSharp"));
}

public class Guards
{
    [Test]
    public void added_as_global_package_reference() => new UseSonarAnalyzers().ForInlineCsproj(@"
<Project Sdk=""Microsoft.NET.Sdk"">

    <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    </PropertyGroup>

  <ItemGroup>
    <GlobalPackageReference Include=""SonarAnalyzer.CSharp"" Version=""10.6.0.109712"" />
  </ItemGroup>

</Project>")
      .HasNoIssues();

    [TestCase("SonarCS.cs")]
    [TestCase("SonarVB.vb")]
    public void Projects_with_analyzers(string project) => new UseSonarAnalyzers()
        .ForProject(project)
        .HasNoIssues();
}
