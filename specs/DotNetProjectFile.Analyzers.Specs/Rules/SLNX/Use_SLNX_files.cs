using DotNetProjectFile.Analyzers.Slnx;

namespace Rules.SLNX.Use_SLNX_files;

public class Reports
{
    [Test]
    public void net90_and_up_via_TargetFramework() => new UseSlnxFiles()
       .ForInlineSdkProject("""
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
  </PropertyGroup>

</Project>
""")
       .HasIssue(
           Issue.WRN("Proj5000", "Use a SLNX solution file instead"));

    [Test]
    public void net90_and_up_via_TargetFrameworks() => new UseSlnxFiles()
     .ForInlineSdkProject("""
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFrameworks>net6.0;net9.0</TargetFrameworks>
  </PropertyGroup>

</Project>
""")
     .HasIssue(
         Issue.WRN("Proj5000", "Use a SLNX solution file instead"));
}

public class Guards
{

    [Test]
    public void older_target_frameworks() => new UseSlnxFiles()
       .ForInlineSdkProject("""
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
  </PropertyGroup>

</Project>
""")
       .HasNoIssues();
}
