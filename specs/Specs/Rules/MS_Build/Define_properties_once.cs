namespace Rules.MS_Build.Define_properties_once;

public class Reports
{
    /// <remarks>
    /// The TargetFrameworks are older versions as newer versions make the
    /// test framework crash.
    /// </remarks>
    [Test]
    public void Properties_defined_before() => new DefinePropertiesOnce()
        .ForInlineCsproj("""
        <Project Sdk="Microsoft.NET.Sdk">

          <PropertyGroup>
            <TargetFramework>net6.0</TargetFramework>
            <ImplicitUsings>enable</ImplicitUsings>
            <ImplicitUsings>disable</ImplicitUsings>
            <Nullable>enable</Nullable>
            <ProductName>Test project</ProductName>
            <Authors>Corniel Nobel; Wesley Baartman</Authors>
          </PropertyGroup>

          <PropertyGroup>
            <TargetFrameworks>net8.0;net6.0</TargetFrameworks>
            <Nullable>annotations</Nullable>
          </PropertyGroup>

          <PropertyGroup Condition="'1' == '1'">
            <ProductName>Test project</ProductName>
          </PropertyGroup>

          <PropertyGroup>
            <Authors Condition="'2' == '1'">Corniel Nobel; Wesley Baartman</Authors>
            <ProductName Condition="'1' == '1'">Test project</ProductName>
          </PropertyGroup>

          <ItemGroup>
            <AdditionalFiles Include="*.csproj" Visible="false" />
          </ItemGroup>

        </Project>
        """)
        .HasIssues(
            Issue.WRN("Proj0011", "Property <ImplicitUsings> has been already defined").WithSpan(/*...*/ 05, 04, 05, 44),
            Issue.WRN("Proj0011", "Property <TargetFrameworks> has been already defined").WithSpan(/*.*/ 12, 04, 12, 54),
            Issue.WRN("Proj0011", "Property <Nullable> has been already defined").WithSpan(/*.........*/ 13, 04, 13, 36),
            Issue.WRN("Proj0011", "Property <ProductName> has been already defined").WithSpan(/*......*/ 22, 04, 22, 66));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_double_properties(string project) => new DefinePropertiesOnce()
        .ForProject(project)
        .HasNoIssues();
}
