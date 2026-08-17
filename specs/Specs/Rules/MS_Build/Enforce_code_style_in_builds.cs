namespace Rules.MS_Build.Enforce_code_style_in_builds;

public class Reports
{
    [Test]
    public void on_not_specified() => new EnforceCodeStyleInBuilds().ForInlineCsproj("""
        <Project Sdk="Microsoft.NET.Sdk">

          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
          </PropertyGroup>

        </Project>
        """)
        .HasIssues(Issue.WRN("Proj0050", "Set <EnforceCodeStyleInBuild> to 'true'").WithSpan(00, 00, 00, 32));
}

public class Guards
{
    [Test]
    public void non_Roslyn_projects() => new EnforceCodeStyleInBuilds().ForInlineFsproj("""
        <Project Sdk="Microsoft.NET.Sdk">

          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
          </PropertyGroup>

        </Project>
        """)
          .HasNoIssues();

    [Test]
    public void when_enabled() => new EnforceCodeStyleInBuilds().ForInlineCsproj("""
        <Project Sdk="Microsoft.NET.Sdk">

          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
            <EnforceCodeStyleInBuild>true</EnforceCodeStyleInBuild>
          </PropertyGroup>

        </Project>
        """)
        .HasNoIssues();

    [Test]
    public void when_disabled() => new EnforceCodeStyleInBuilds().ForInlineCsproj("""
        <Project Sdk="Microsoft.NET.Sdk">

          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
            <EnforceCodeStyleInBuild>false</EnforceCodeStyleInBuild>
          </PropertyGroup>

        </Project>
        """)
        .HasNoIssues();
}
