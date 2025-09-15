namespace Rules.MS_Build.Language_version_should_be_explicit_version_number;

public class Reports
{
    [Test]
    public void on_missing() => new LanguageVersionShouldBeExplicitVersionNumber()
       .ForInlineCsproj("""
            <Project Sdk="Microsoft.NET.Sdk">

              <PropertyGroup>
                <TargetFramework>net9.0</TargetFramework>
              </PropertyGroup>

            </Project>
       """)
        .HasIssues(Issue.WRN("Proj0048", "Define <LangVersion> with an explicit version number").WithSpan(0, 0, 0, 32));

    [TestCase("", 39)]
    [TestCase("latest", 45)]
    [TestCase("Latest", 45)]
    [TestCase("latestMajor", 50)]
    [TestCase("LatestMajor", 50)]
    [TestCase("default", 46)]
    [TestCase("Default", 46)]
    [TestCase("preview", 46)]
    [TestCase("Preview", 46)]
    public void on_illegal(string langVersion, int endPos) => new LanguageVersionShouldBeExplicitVersionNumber()
       .ForInlineCsproj($"""
            <Project Sdk="Microsoft.NET.Sdk">

                <PropertyGroup>
                    <TargetFramework>net9.0</TargetFramework>
                    <LangVersion>{langVersion}</LangVersion>
                </PropertyGroup>

            </Project>
        """)
        .HasIssues(Issue.WRN("Proj0048", "Define <LangVersion> with an explicit version number").WithSpan(4, 12, 4, endPos));
}

public class Guards
{
    [Test]
    public void with_version() => new LanguageVersionShouldBeExplicitVersionNumber()
       .ForInlineCsproj("""
            <Project Sdk="Microsoft.NET.Sdk">

                <PropertyGroup>
                    <TargetFramework>net9.0</TargetFramework>
                    <LangVersion>13</LangVersion>
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
