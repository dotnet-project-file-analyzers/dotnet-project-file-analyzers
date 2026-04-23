namespace Rules.MS_Build.Use_analyzers_for_packages;

public class Reports
{
    [Test]
    public void missing_analyzers() => new UseAnalyzersForPackages().ForInlineCsproj("""
        <Project Sdk="Microsoft.NET.Sdk">

          <PropertyGroup>
           <TargetFramework>net10.0</TargetFramework>
            <NuGetAudit>false</NuGetAudit>
          </PropertyGroup>

          <ItemGroup>
           <PackageReference Include="AwesomeAssertions" Version="9.4.0" />
           <PackageReference Include="Ardalis.ApiEndpoints" Version="4.1.0" />
           <PackageReference Include="FakeItEasy" Version="9.0.1" />
           <PackageReference Include="FluentAssertions" Version="8.9.0" />
           <PackageReference Include="Libplanet" Version="5.5.3" />
           <PackageReference Include="Lucene.Net" Version="3.0.3 " />
           <PackageReference Include="MassTransit" Version="9.1.0" />
           <PackageReference Include="MessagePack" Version="3.1.4" />
           <PackageReference Include="MessagePipe" Version="1.8.1" />
           <PackageReference Include="Microsoft.AspNetCore.Components" Version="10.0.7" />
           <PackageReference Include="Microsoft.Azure.Functions.Extensions" Version="1.1.0" />
           <PackageReference Include="Microsoft.CodeAnalysis" Version="5.3.0" />
           <PackageReference Include="Microsoft.EntityFrameworkCore" Version="10.0.7" />
           <PackageReference Include="Microsoft.ServiceHub.Framework" Version="4.8.55" />
           <PackageReference Include="MongoDB.Bson" Version="3.8.0" />
           <PackageReference Include="Moq" Version="4.20.72" />
           <PackageReference Include="NSubstitute" Version="5.3.0" />
           <PackageReference Include="NUnit" Version="4.5.1" />
           <PackageReference Include="RuntimeContracts" Version="0.5.0" />
           <PackageReference Include="Serilog" Version="4.3.1" />
           <PackageReference Include="StreamJsonRpc" Version="2.24.84" />
           <PackageReference Include="xunit.assert" Version="2.9.3" />
           <PackageReference Include="ZeroFormatter" Version="1.6.4" />
          </ItemGroup>

        </Project>
        """)
        .HasIssues(
            // Direct.
            new("Proj1001", "Use FakeItEasy.Analyzer.CSharp to analyze FakeItEasy"),
            new("Proj1001", "Use AwesomeAssertions.Analyzers to analyze AwesomeAssertions"),
            new("Proj1001", "Use AwesomeAssertions.Analyzers to analyze AwesomeAssertions"),
            new("Proj1001", "Use Libplanet.Analyzers to analyze Libplanet"),
            new("Proj1001", "Use Lucene.Net.Analysis.Common to analyze Lucene.Net"),
            new("Proj1001", "Use MassTransit.Analyzers to analyze MassTransit"),
            new("Proj1001", "Use MessagePipe.Analyzer to analyze MessagePipe"),
            new("Proj1001", "Use Microsoft.Azure.Functions.Analyzers to analyze Microsoft.Azure.Functions.Extensions"),
            new("Proj1001", "Use MongoDB.Analyzer to analyze MongoDB.Bson"),
            new("Proj1001", "Use Moq.Analyzers to analyze Moq"),
            new("Proj1001", "Use NSubstitute.Analyzers.CSharp to analyze NSubstitute"),
            new("Proj1001", "Use NUnit.Analyzers to analyze NUnit"),
            new("Proj1001", "Use SerilogAnalyzer to analyze Serilog"),
            new("Proj1001", "Use xunit.analyzers to analyze xunit.assert"),
            new("Proj1001", "Use ZeroFormatter.Analyzer to analyze ZeroFormatter"),

            // Transitive.
#if Is_Windows // are resolvable on the build.
            new("Proj1001", "Use MessagePackAnalyzer to analyze StreamJsonRpc"),
            new("Proj1001", "Use MessagePackAnalyzer to analyze Microsoft.ServiceHub.Framework"),
#endif
            new("Proj1001", "Use SerilogAnalyzer to analyze Libplanet"));
  
}

public class Guards
{
    [Test]
    public void non_trimmed_values() => new UseAnalyzersForPackages().ForInlineCsproj("""
        <Project Sdk="Microsoft.NET.Sdk">

          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
          </PropertyGroup>

          <ItemGroup>
            <PackageReference Include="Serilog" Version="4.3.0" />
          </ItemGroup>

          <ItemGroup>
            <PackageReference Include="SerilogAnalyzer " Version="0.15.0" PrivateAssets="all" />
          </ItemGroup>

        </Project>
        """)
        .HasNoIssues();

    [Test]
    public void references_defined_globally() => new UseAnalyzersForPackages().ForInlineCsproj("""
        <Project Sdk="Microsoft.NET.Sdk">

          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
          </PropertyGroup>

          <ItemGroup>
            <GlobalPackageReference Include="NUnit.Analyzers" Version="4.12.0" />
          </ItemGroup>

          <ItemGroup>
            <PackageReference Include="NUnit" Version="4.5.1" />
          </ItemGroup>

        </Project>
        """)
        .HasNoIssues();

    [Test]
    public void xunit_as_analyzer_is_included() => new UseAnalyzersForPackages().ForInlineCsproj("""
        <Project Sdk="Microsoft.NET.Sdk">

          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
          </PropertyGroup>

          <ItemGroup>
            <PackageReference Include="xunit" Version="2.9.2" />
          </ItemGroup>

        </Project>
        """)
       .HasNoIssues();

    [Test]
    public void xunit_v3_as_analyzer_is_included() => new UseAnalyzersForPackages().ForInlineCsproj("""
        <Project Sdk="Microsoft.NET.Sdk">

          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
            <OutputType>exe</OutputType>
          </PropertyGroup>

          <ItemGroup>
            <PackageReference Include="xunit.v3" Version="3.2.2" />
          </ItemGroup>

        </Project>
        """)
      .HasNoIssues();

    [Test]
    public void Projects_with_included_analyzers() => new UseAnalyzersForPackages().ForInlineCsproj("""
        <Project Sdk="Microsoft.NET.Sdk">

          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
            <NuGetAudit>false</NuGetAudit>
          </PropertyGroup>

          <ItemGroup>
            <PackageReference Include="Ardalis.ApiEndpoints" Version="4.1.0" />
            <PackageReference Include="FakeItEasy" Version="9.0.1" />
            <PackageReference Include="FluentAssertions" Version="8.9.0" />
            <PackageReference Include="Libplanet" Version="5.5.3" />
            <PackageReference Include="LiteDB" Version="5.0.21" />
            <PackageReference Include="Lucene.Net" Version="4.8.0-beta00016" />
            <PackageReference Include="MassTransit" Version="9.1.0" />
            <PackageReference Include="MessagePack" Version="3.1.4" />
            <PackageReference Include="MessagePipe" Version="1.8.1" />
            <PackageReference Include="Microsoft.AspNetCore.Mvc.Testing" Version="10.0.7" />
            <PackageReference Include="Microsoft.Azure.Functions.Extensions" Version="1.1.0" />
            <PackageReference Include="Microsoft.CodeAnalysis" Version="5.3.0" />
            <PackageReference Include="Microsoft.EntityFrameworkCore" Version="10.0.7" />
            <PackageReference Include="Microsoft.ServiceHub.Framework" Version="4.8.55" />
            <PackageReference Include="MongoDB.Bson" Version="3.8.0" />
            <PackageReference Include="Moq" Version="4.20.72" />
            <PackageReference Include="NSubstitute" Version="5.3.0" />
            <PackageReference Include="NUnit" Version="4.5.1" />
            <PackageReference Include="RuntimeContracts" Version="0.5.0" />
            <PackageReference Include="Serilog" Version="4.3.1" />
            <PackageReference Include="StreamJsonRpc" Version="2.24.84" />
            <PackageReference Include="xunit" Version="2.9.3" />
            <PackageReference Include="ZeroFormatter" Version="1.6.4" />
            <PackageReference Include="Zio" Version="0.22.2" />
          </ItemGroup>

          <ItemGroup Label="Analyzers">
            <PackageReference PrivateAssets="All" Include="Ardalis.ApiEndpoints.CodeAnalyzers" Version="4.1.0" />
            <PackageReference PrivateAssets="All" Include="FakeItEasy.Analyzer.CSharp" Version="6.1.1" />
            <PackageReference PrivateAssets="All" Include="Lucene.Net.Analysis.Common" Version="1" />
            <PackageReference PrivateAssets="All" Include="Libplanet.Analyzers" Version="5.5.3" />
            <PackageReference PrivateAssets="All" Include="MassTransit.Analyzers" Version="8.5.9" />
            <PackageReference PrivateAssets="All" Include="MessagePackAnalyzer" Version="3.1.4" />
            <PackageReference PrivateAssets="All" Include="MessagePipe.Analyzer" Version="1.8.1" />
            <PackageReference PrivateAssets="All" Include="Microsoft.AspNetCore.Components.Analyzers" Version="10.0.7" />
            <PackageReference PrivateAssets="All" Include="Microsoft.Azure.Functions.Analyzers" Version="1" />
            <PackageReference PrivateAssets="All" Include="Microsoft.CodeAnalysis.Analyzers" Version="5.3.0" />
            <PackageReference PrivateAssets="All" Include="Microsoft.EntityFrameworkCore.Analyzers" Version="10.0.7" />
            <PackageReference PrivateAssets="All" Include="Microsoft.ServiceHub.Analyzers" Version="4.8.55" />
            <PackageReference PrivateAssets="All" Include="MongoDB.Analyzer" Version="2.0.0" />
            <PackageReference PrivateAssets="All" Include="Moq.Analyzers" Version="0.4.2" />
            <PackageReference PrivateAssets="All" Include="NSubstitute.Analyzers.CSharp" Version="1.0.17" />
            <PackageReference PrivateAssets="All" Include="NUnit.Analyzers" Version="4.12.0" />
            <PackageReference PrivateAssets="All" Include="RuntimeContracts.Analyzer" Version="0.4.3" />
            <PackageReference PrivateAssets="All" Include="SerilogAnalyzer" Version="0.15" />
            <PackageReference PrivateAssets="All" Include="xunit.analyzers" Version="1.27.0" />
            <PackageReference PrivateAssets="All" Include="ZeroFormatter.Analyzer" Version="1.1.1" />
          </ItemGroup>

        </Project>
        """)
        .HasNoIssues();


}
