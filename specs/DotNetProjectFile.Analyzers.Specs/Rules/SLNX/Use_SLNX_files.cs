using DotNetProjectFile.Analyzers.Slnx;

namespace Rules.SLNX.Use_SLNX_files;

public class Reports
{
    [Test]
    public void missing_SLNX() => new UseSlnxFiles()
       .ForInlineSdkProject("""
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
  </PropertyGroup>

</Project>
""")
       .HasIssue(Issue.WRN("Proj5000", "Use a SLNX solution file instead"));
}
