namespace Rules.MS_Build.Language_version_should_be_explicit_version_number;

public class Reports
{
    [Test]
    public void on_missing() => new LanguageVersionShouldBeExplicitVersionNumber()
       .ForInlineCsproj("""
            <Project Sdk="Microsoft.NET.Sdk">

              <PropertyGroup>
                <TargetFramework>net10.0</TargetFramework>
              </PropertyGroup>

            </Project>
       """)
        .HasIssues(Issue.WRN("Proj0048", "Define <LangVersion> with an explicit version number").WithSpan(0, 0, 0, 32));

    [TestCase("", 31)]
    [TestCase("latest", 37)]
    [TestCase("Latest", 37)]
    [TestCase("latestMajor", 42)]
    [TestCase("LatestMajor", 42)]
    [TestCase("default", 38)]
    [TestCase("Default", 38)]
    [TestCase("preview", 38)]
    [TestCase("Preview", 38)]
    public void on_illegal(string langVersion, int endPos) => new LanguageVersionShouldBeExplicitVersionNumber()
       .ForInlineCsproj($"""
        <Project Sdk="Microsoft.NET.Sdk">

          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
            <LangVersion>{langVersion}</LangVersion>
          </PropertyGroup>

        </Project>
        """)
        .HasIssues(Issue.WRN("Proj0048", "Define <LangVersion> with an explicit version number").WithSpan(4, 04, 4, endPos));
}

public class Guards
{
    [TestCase("13")]
    [TestCase("13.0")]
    [TestCase("14")]
    public void with_version(string langVersion) => new LanguageVersionShouldBeExplicitVersionNumber()
       .ForInlineCsproj($"""
        <Project Sdk="Microsoft.NET.Sdk">

          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
            <LangVersion>{langVersion}</LangVersion>
          </PropertyGroup>

        </Project>
        """)
        .HasNoIssues();

    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project) => new LanguageVersionShouldBeExplicitVersionNumber()
        .ForProject(project)
        .HasNoIssues();
}
