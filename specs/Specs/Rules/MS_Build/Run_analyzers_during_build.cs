namespace Rules.MS_Build.Run_analyzers_during_build;

public class Reports
{
    [Test]
    public void when_disabled() => new RunAnalyzersDuringBuild().ForInlineCsproj("""
        <Project Sdk="Microsoft.NET.Sdk">
        
          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
            <RunAnalyzersDuringBuild>false</RunAnalyzersDuringBuild>
          </PropertyGroup>
        
        </Project>
        """)
        .HasIssue(Issue.WRN("Proj0053", "Run analyzers during build").WithSpan(04, 04, 04, 60));
}

public class Guards
{
    [Test]
    public void when_enabled_explictly() => new RunAnalyzersDuringBuild().ForInlineCsproj("""
        <Project Sdk="Microsoft.NET.Sdk">
        
          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
            <RunAnalyzersDuringBuild>true</RunAnalyzersDuringBuild>
          </PropertyGroup>
        
        </Project>
        """)
        .HasNoIssues();
}
