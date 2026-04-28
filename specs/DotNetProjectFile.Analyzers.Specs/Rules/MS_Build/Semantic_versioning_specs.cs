namespace Rules.MS_Build.Semantic_versioning_specs;

public class Reports
{
    [TestCase("12")]
    [TestCase("3.1415")]
    [TestCase("1.2.3.4")]
    public void non_semantic_version(string version) => new VersionShouldBeSemVerCompliant()
        .ForInlineCsproj($"""
        <Project Sdk="Microsoft.NET.Sdk">

          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
            <Version>{version}</Version>
          </PropertyGroup>

        </Project>
        """)
        .HasIssue(
            Issue.WRN("Proj0253", $"Version '{version}' does not comply with Semantic Versioning"));


    [TestCase("1.2.3.4")]
    [TestCase("1.1.0-rc.1")]
    [TestCase("1.1.0+e471d15")]
    [TestCase("1.1.0-rc.1+e471d15")]
    public void non_semantic_version_prefix(string version) => new VersionPrefixShouldBeSemVerCompliant()
       .ForInlineCsproj($"""
        <Project Sdk="Microsoft.NET.Sdk">

          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
            <VersionPrefix>{version}</VersionPrefix>
          </PropertyGroup>

        </Project>
        """)
       .HasIssue(
           Issue.WRN("Proj0254", $"VersionPrefix '{version}' does not comply with Semantic Versioning"));
}

public class Guards
{
    [TestCase("1.0.0")]
    [TestCase("3.14.1596")]
    [TestCase("1.1.0-rc.1+e471d15")]
    public void semantic_version(string version) => new VersionShouldBeSemVerCompliant()
        .ForInlineCsproj($"""
        <Project Sdk="Microsoft.NET.Sdk">

          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
            <Version>{version}</Version>
          </PropertyGroup>

        </Project>
        """)
        .HasNoIssues();


    [Test]
    public void semantic_version_prefix() => new VersionPrefixShouldBeSemVerCompliant()
       .ForInlineCsproj("""
        <Project Sdk="Microsoft.NET.Sdk">

          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
            <VersionPrefix>1.2.3</VersionPrefix>
          </PropertyGroup>

        </Project>
        """)
       .HasNoIssues();
}
