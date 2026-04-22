namespace Rules.MS_Build.Define_single_target_framework;

public class Reports
{
    [Test]
    public void defined_in_target_frameworks() => new DefineSingleTargetFramework()
        .ForInlineCsproj("""
        <Project Sdk="Microsoft.NET.Sdk">

          <PropertyGroup>
            <TargetFrameworks>net10.0</TargetFrameworks>
          </PropertyGroup>

        </Project>
        """)
        .HasIssues(
            Issue.WRN("Proj0009", "Use the <TargetFramework> node instead").WithSpan(3, 4, 3, 48));
}

public class Guards
{
    [Test]
    public void Projects_with_analyzers() => new DefineSingleTargetFramework().ForInlineCsproj("""
        <Project Sdk="Microsoft.NET.Sdk">

          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
          </PropertyGroup>

        </Project>
        """)
        .HasNoIssues();
}
