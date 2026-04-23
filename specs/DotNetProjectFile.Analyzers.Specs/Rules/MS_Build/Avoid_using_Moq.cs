namespace Rules.MS_Build.Avoid_using_Moq;

public class Reports
{
    [Test]
    public void on_Moq_dependency() => new AvoidUsingMoq().ForInlineCsproj("""
        <Project Sdk="Microsoft.NET.Sdk">

          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
          </PropertyGroup>

          <ItemGroup>
            <PackageReference Include="Moq" Version="4.20.2" />
          </ItemGroup>

        </Project>
        """)
       .HasIssue(Issue.WRN("Proj1100", "Do not use Moq").WithSpan(7, 04, 7, 55));
}

public class Guards
{
    [TestCase("PackagesWithAnalyzers.cs")]
    [TestCase("PackagesWithoutAnalyzers.cs")]
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project) => new AvoidUsingMoq()
        .ForProject(project)
        .HasNoIssues();
}
