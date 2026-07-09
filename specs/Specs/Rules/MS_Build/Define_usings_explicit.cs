namespace Rules.MS_Build.Define_usings_explicit;

public class Reports
{
    [Test]
    public void implicit_usings() => new DefineUsingsExplicit().ForInlineCsproj("""
        <Project Sdk="Microsoft.NET.Sdk">

          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
            <ImplicitUsings>enable</ImplicitUsings>
          </PropertyGroup>

        </Project>
        """)
        .HasIssue(
            Issue.WRN("Proj0003", "Define usings explicit").WithSpan(4, 4, 4, 43));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project) => new DefineUsingsExplicit()
        .ForProject(project)
        .HasNoIssues();
}
