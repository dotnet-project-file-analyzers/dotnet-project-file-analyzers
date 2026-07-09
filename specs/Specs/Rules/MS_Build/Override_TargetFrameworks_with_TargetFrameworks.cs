namespace Rules.MS_Build.Override_TargetFrameworks_with_TargetFrameworks;

public class Reports
{
    [Test]
    public void TFM_overriding_TFMs() => new OverrideTargetFrameworksWithTargetFrameworks().ForInlineCsproj("""
        <Project Sdk="Microsoft.NET.Sdk">

          <PropertyGroup>
            <TargetFrameworks>net8.0;net10.0</TargetFrameworks>
          </PropertyGroup>

          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
          </PropertyGroup>

        </Project>
        """)
        .HasIssue(Issue.WRN("Proj0027", "This <TargetFramework> will be ignored due to the earlier use of <TargetFrameworks>")
        .WithSpan(07, 04, 07, 46));
}

public class Guards
{
    [Test]
    public void projects_with_TFM_only() => new OverrideTargetFrameworksWithTargetFrameworks().ForInlineCsproj("""
        <Project Sdk="Microsoft.NET.Sdk">

          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
          </PropertyGroup>

        </Project>
        """)
        .HasNoIssues();

    [Test]
    public void Single_TFM_and_conditional_TFMs() => new OverrideTargetFrameworksWithTargetFrameworks().ForInlineCsproj("""
        <Project Sdk="Microsoft.NET.Sdk">

          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
            <TargetFrameworks Condition="'$(OS)' != 'Windows_NT'">net8.0;net9.0</TargetFrameworks>
          </PropertyGroup>

        </Project>
        """)
        .HasNoIssues();

    [Test]
    public void projects_with_TFMs_only() => new OverrideTargetFrameworksWithTargetFrameworks().ForInlineCsproj("""
        <Project Sdk="Microsoft.NET.Sdk">

          <PropertyGroup>
            <TargetFrameworks>netstandard2.0;net8.0</TargetFrameworks>
            <Nullable>enable</Nullable>
          </PropertyGroup>

        </Project>
        """)
        .HasNoIssues();


    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project) => new OverrideTargetFrameworksWithTargetFrameworks()
        .ForProject(project)
        .HasNoIssues();
}
