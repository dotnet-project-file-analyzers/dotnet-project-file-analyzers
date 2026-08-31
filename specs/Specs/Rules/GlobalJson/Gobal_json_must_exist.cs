using DotNetProjectFile.Analyzers.GlobalJson;

namespace Rules.Global_json_must_exist;

public class Reports
{
    [Test]
    public void missing_global_json() => new GlobalJsonMustExist().ForInlineCsproj("""
        <Project Sdk="Microsoft.NET.Sdk">

          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
          </PropertyGroup>

        </Project>
        """)
        .HasIssue(Issue.WRN("Proj6010", "global.json does not exist"));
}
public class Guards
{
    [Test]
    public void available_global_json() => new GlobalJsonMustExist().ForInlineCsproj("""
        <Project Sdk="Microsoft.NET.Sdk">

          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
          </PropertyGroup>

        </Project>
        """)
        .WithFile("global.json", """
        {
          "sdk": {
            "version": "10.0.400",
            "rollForward": "latestPatch"
          }
        }
        """)
        .HasNoIssues();
}
