using DotNetProjectFile.Licensing;

namespace Rules.MS_Build.Use_CDATA_for_large_texts;

public class Reports
{
    [Test]
    public void missing_CDATA() => new UseCDATAForLargeTexts()
        .ForProject("MissingCDATA.cs")
        .HasIssue(Issue.WRN("Proj1701", "Add <![CDATA[ and ]]> around this text")
        .WithSpan(17, 24, 23, 04));
}

public class Guards
{
    [Test]
    public void missing_CDATA() => new UseCDATAForLargeTexts()
        .ForProject("PackageDescription.cs")
        .HasNoIssues();

    
    [Test]
    public void Short_release_notes() => new UseCDATAForLargeTexts()
        .ForInlineCsproj(@$"
<Project Sdk=""Microsoft.NET.Sdk"">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <PackageReleaseNotes>Handled by GitHub Releases</PackageReleaseNotes>
  </PropertyGroup>

</Project>
        ")
        .HasNoIssues();


    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project) => new UseCDATAForLargeTexts()
        .ForProject(project)
        .HasNoIssues();
}
