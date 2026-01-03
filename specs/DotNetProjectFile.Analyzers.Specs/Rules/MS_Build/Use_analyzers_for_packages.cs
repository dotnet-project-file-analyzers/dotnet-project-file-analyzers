namespace Rules.MS_Build.Use_analyzers_for_packages;

#if RELEASE
[TestFixture(Ignore = "Build has difficulties resolving (some) NuGet packages")]
#endif
public class Reports
{
    [Test]
    public void missing_analyzers() => new UseAnalyzersForPackages()
        .ForProject("PackagesWithoutAnalyzers.cs")
        .HasIssues(
            // Direct.
            new("Proj1001", "Use Ardalis.ApiEndpoints.CodeAnalyzers to analyze Ardalis.ApiEndpoints"),
            new("Proj1001", "Use FakeItEasy.Analyzer.CSharp to analyze FakeItEasy"),
            new("Proj1001", "Use AwesomeAssertions.Analyzers to analyze AwesomeAssertions"),
            new("Proj1001", "Use AwesomeAssertions.Analyzers to analyze AwesomeAssertions"),
            new("Proj1001", "Use Libplanet.Analyzers to analyze Libplanet"),
            new("Proj1001", "Use Lucene.Net.Analysis.Common to analyze Lucene.Net"),
            new("Proj1001", "Use MassTransit.Analyzers to analyze MassTransit"),
            new("Proj1001", "Use MessagePackAnalyzer to analyze MessagePack"),
            new("Proj1001", "Use MessagePipe.Analyzer to analyze MessagePipe"),
            new("Proj1001", "Use Microsoft.AspNetCore.Components.Analyzers to analyze Microsoft.AspNetCore.Components"),
            new("Proj1001", "Use Microsoft.Azure.Functions.Analyzers to analyze Microsoft.Azure.Functions.Extensions"),
            new("Proj1001", "Use Microsoft.EntityFrameworkCore.Analyzers to analyze Microsoft.EntityFrameworkCore"),
            new("Proj1001", "Use Microsoft.ServiceHub.Analyzers to analyze Microsoft.ServiceHub.Framework"),
            new("Proj1001", "Use MongoDB.Analyzer to analyze MongoDB.Bson"),
            new("Proj1001", "Use Moq.Analyzers to analyze Moq"),
            new("Proj1001", "Use NSubstitute.Analyzers.CSharp to analyze NSubstitute"),
            new("Proj1001", "Use NUnit.Analyzers to analyze NUnit"),
            new("Proj1001", "Use RuntimeContracts.Analyzer to analyze RuntimeContracts"),
            new("Proj1001", "Use SerilogAnalyzer to analyze Serilog"),
            new("Proj1001", "Use xunit.analyzers to analyze xunit"),
            new("Proj1001", "Use ZeroFormatter.Analyzer to analyze ZeroFormatter"),

            // Transitive.
            new("Proj1001", "Use SerilogAnalyzer to analyze Libplanet"),
            new("Proj1001", "Use MessagePackAnalyzer to analyze Microsoft.ServiceHub.Framework"),
            new("Proj1001", "Use MessagePackAnalyzer to analyze StreamJsonRpc"));
}


public class Guards_
{
    [Test]
    public void non_trimmed_values() => new UseAnalyzersForPackages()
        .ForInlineCsproj("""
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
}

#if RELEASE
[TestFixture(Ignore = "Build has difficulties resolving (some) NuGet packages")]
#endif
public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    [TestCase("PackagesWithAnalyzers.cs")]
    public void Projects_without_issues(string project) => new UseAnalyzersForPackages()
        .ForProject(project)
        .HasNoIssues();

    [Test]
    public void references_defined_globally() => new UseAnalyzersForPackages()
        .ForInlineCsproj(@"
<Project Sdk=""Microsoft.NET.Sdk"">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
  </PropertyGroup>

  <ItemGroup>
    <GlobalPackageReference Include=""NUnit.Analyzers"" Version=""*"" />
  </ItemGroup>

  <ItemGroup>
    <PackageReference Include=""NUnit"" Version=""4.3.2"" />
  </ItemGroup>

</Project>")
        .HasNoIssues();

}
