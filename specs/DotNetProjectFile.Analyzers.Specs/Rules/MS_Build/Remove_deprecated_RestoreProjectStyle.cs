namespace Rules.MS_Build.Remove_deprecated_RestoreProjectStyle;

public class Reports
{
    [Test]
    public void when_property_is_defined() => new RemoveDeprecatedRestoreProjectStyle()
       .ForInlineCsproj(@"<Project Sdk=""Microsoft.NET.Sdk"">

  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <RestoreProjectStyle>PackageReference</RestoreProjectStyle>
  </PropertyGroup>

</Project>")
       .HasIssue(Issue.WRN("Proj0035", "Remove the <RestoreProjectStyle> property")
       .WithSpan(04, 04, 04, 63));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project) => new RemoveDeprecatedRestoreProjectStyle()
        .ForProject(project)
        .HasNoIssues();
}

