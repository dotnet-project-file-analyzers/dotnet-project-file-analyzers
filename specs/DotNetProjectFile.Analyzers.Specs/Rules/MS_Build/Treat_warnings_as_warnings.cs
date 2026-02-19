namespace Rules.MS_Build.Treat_warnings_as_warnings;

public class Reports
{
    [Test]
    public void when_treat_all_warnings_is_enabled() => new TreatWarningsAsWarnings()
       .ForInlineCsproj(@"<Project Sdk=""Microsoft.NET.Sdk"">

  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <TreatWarningsAsErrors>true</TreatWarningsAsErrors>
  </PropertyGroup>

</Project>")
       .HasIssue(Issue.WRN("Proj0039", "Treat all warnings as errors is considered a bad practice")
       .WithSpan(04, 04, 04, 55));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project) => new TreatWarningsAsWarnings()
        .ForProject(project)
        .HasNoIssues();
}
