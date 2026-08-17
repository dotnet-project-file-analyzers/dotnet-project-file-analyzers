namespace Rules.MS_Build.Use_NET_analyzers;

public class Reports
{
    [Test]
    public void when_disabled() => new UseDotNetAnalyzers().ForInlineCsproj("""
        <Project Sdk="Microsoft.NET.Sdk">

          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
            <EnableNETAnalyzers>false</EnableNETAnalyzers>
          </PropertyGroup>

        </Project>
        """)
       .HasIssue(
            Issue.WRN("Proj1002", "Use Microsoft's .NET analyzers by setting <EnableNETAnalyzers> to true"));

    [Test]
    public void when_not_set() => new UseDotNetAnalyzers().ForInlineCsproj("""
        <Project Sdk="Microsoft.NET.Sdk">

          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
          </PropertyGroup>

        </Project>
        """)
       .HasIssue(
            Issue.WRN("Proj1002", "Use Microsoft's .NET analyzers by setting <EnableNETAnalyzers> to true"));
}

public class Guards
{
    [Test]
    public void when_enabled() => new UseDotNetAnalyzers().ForInlineCsproj("""
        <Project Sdk="Microsoft.NET.Sdk">

          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
            <EnableNETAnalyzers>true</EnableNETAnalyzers>
          </PropertyGroup>

        </Project>
        """)
       .HasNoIssues();

    [Test]
    public void non_Roslyn_projects() => new UseDotNetAnalyzers().ForInlineFsproj("""
        <Project Sdk="Microsoft.NET.Sdk">

          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
            <EnableNETAnalyzers>false</EnableNETAnalyzers>
          </PropertyGroup>

        </Project>
        """)
       .HasNoIssues();
}
