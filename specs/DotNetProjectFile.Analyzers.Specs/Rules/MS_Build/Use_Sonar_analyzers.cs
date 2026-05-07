namespace Rules.MS_Build.Use_Sonar_analyzers;

public class Reports
{
    [Test]
    public void missing_analyzer_for_CSharp() => new UseSonarAnalyzers().ForInlineCsproj("""
        <Project Sdk="Microsoft.NET.Sdk">
        
          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
          </PropertyGroup>
        
        </Project>
        """)
        .HasIssue(Issue.WRN("Proj1003", "Add SonarAnalyzer.CSharp"));

    [Test]
    public void missing_analyzer_for_Visual_Basic() => new UseSonarAnalyzers().ForInlineVbproj("""
        <Project Sdk="Microsoft.NET.Sdk">
        
          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
          </PropertyGroup>
        
        </Project>
        """)
        .HasIssue(Issue.WRN("Proj1003", "Add SonarAnalyzer.VisualBasic"));

    [Test]
    public void only_added_as_project_version() => new UseSonarAnalyzers().ForInlineCsproj("""
        <Project Sdk="Microsoft.NET.Sdk">

          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
          </PropertyGroup>

          <ItemGroup>
            <PackageVersion Include="SonarAnalyzer.CSharp" Version="10.6.0.109712" />
          </ItemGroup>

        </Project>
        """)
        .HasIssue(Issue.WRN("Proj1003", "Add SonarAnalyzer.CSharp"));
}

public class Guards
{
    [Test]
    public void added_as_global_package_reference() => new UseSonarAnalyzers().ForInlineCsproj(@"
        <Project Sdk=""Microsoft.NET.Sdk"">

          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
          </PropertyGroup>

          <ItemGroup>
            <GlobalPackageReference Include=""SonarAnalyzer.CSharp"" Version=""10.6.0.109712"" />
          </ItemGroup>

        </Project>")
        .HasNoIssues();

    [Test]
    public void CSharp_with_analyzers() => new UseSonarAnalyzers().ForInlineCsproj("""
        <Project Sdk="Microsoft.NET.Sdk">

          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
          </PropertyGroup>

          <ItemGroup Label="Analyzers">
            <PackageReference Include="SonarAnalyzer.CSharp" Version="10.6.0.109712" PrivateAssets="all" ExcludeAssets="runtime" />
          </ItemGroup>

        </Project>
        """)
        .HasNoIssues();

    [Test]
    public void VBNet_with_analyzers() => new UseSonarAnalyzers().ForInlineVbproj("""
        <Project Sdk="Microsoft.NET.Sdk">

          <PropertyGroup>
            <TargetFramework>netstandard2.0</TargetFramework>
          </PropertyGroup>

          <ItemGroup Label="Analyzers">
            <PackageReference Include="SonarAnalyzer.VisualBasic" Version="10.6.0.109712" PrivateAssets="all" />
          </ItemGroup>

        </Project>
        """)
        .HasNoIssues();
}
