namespace Rules.MS_Build.SymbolPackageFormat_snupkg_requires_DebugType_portable;

public class Reports
{
    [Test]
    public void on_not_defined_debug_type() => new SymbolPackageFormatSNupkgSetup()
        .ForInlineCsproj("""
        <Project Sdk="Microsoft.NET.Sdk">

          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
            <SymbolPackageFormat>snupkg</SymbolPackageFormat>
            <IncludeSymbols>true</IncludeSymbols>
          </PropertyGroup>

        </Project>
        """)
        .HasIssues(Issue.WRN("Proj0218", "The <SymbolPackageFormat> 'snupkg' requires <DebugType> to have the value 'portable'")
        .WithSpan(04, 04, 04, 53));

    [Test]
    public void on_different_debug_type() => new SymbolPackageFormatSNupkgSetup()
        .ForInlineCsproj("""
        <Project Sdk="Microsoft.NET.Sdk">

          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
            <SymbolPackageFormat>snupkg</SymbolPackageFormat>
            <DebugType>embedded</DebugType>
            <IncludeSymbols>true</IncludeSymbols>
          </PropertyGroup>

        </Project>
        """)
        .HasIssues(Issue.WRN("Proj0218", "The <SymbolPackageFormat> 'snupkg' requires <DebugType> to have the value 'portable'")
        .WithSpan(05, 04, 05, 35));
}

public class Guards
{
    [TestCase("TestProject.cs")]
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project) => new SymbolPackageFormatSNupkgSetup()
        .ForProject(project)
        .HasNoIssues();

    [Test]
    public void on_enabled_property() => new SymbolPackageFormatSNupkgSetup()
        .ForInlineCsproj("""
        <Project Sdk="Microsoft.NET.Sdk">

          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
            <SymbolPackageFormat>snupkg</SymbolPackageFormat>
            <DebugType>portable</DebugType>
            <IncludeSymbols>true</IncludeSymbols>
          </PropertyGroup>

        </Project>
        """)
        .HasNoIssues();
}
