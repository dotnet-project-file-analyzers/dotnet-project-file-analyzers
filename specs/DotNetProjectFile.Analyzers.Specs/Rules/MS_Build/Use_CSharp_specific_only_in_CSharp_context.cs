namespace Rules.Use_CSharp_specific_only_in_CSharp_context;

public class Reports
{
    [Test]
    public void Visual_Basic_context() => new UseInCSharpContextOnly()
        .ForInlineVbproj("""
        <Project Sdk="Microsoft.NET.Sdk">

          <PropertyGroup>
            <TargetFramework>netstandard2.0</TargetFramework>
            <OutputType>Library</OutputType>
            <AllowUnsafeBlocks>false</AllowUnsafeBlocks>
            <CheckForOverflowUnderflow>true</CheckForOverflowUnderflow>
            <Nullable>enable</Nullable>
          </PropertyGroup>

        </Project>
        """)
        .HasIssues(
            Issue.WRN("Proj0029", "The property <AllowUnsafeBlocks> is only applicable when using C# and can therefor be removed" /*...*/).WithSpan(05, 4, 05, 48),
            Issue.WRN("Proj0029", "The property <CheckForOverflowUnderflow> is only applicable when using C# and can therefor be removed").WithSpan(06, 4, 06, 63),
            Issue.WRN("Proj0029", "The property <Nullable> is only applicable when using C# and can therefor be removed" /*............*/).WithSpan(07, 4, 07, 31));
}

public class Guards
{
    [Test]
    public void CSharp_context() => new UseInCSharpContextOnly()
        .ForInlineCsproj("""
        <Project Sdk="Microsoft.NET.Sdk">
        
          <PropertyGroup>
            <TargetFramework>net8.0</TargetFramework>
            <OutputType>Library</OutputType>
            <AllowUnsafeBlocks>false</AllowUnsafeBlocks>
            <CheckForOverflowUnderflow>true</CheckForOverflowUnderflow>
            <Nullable>enable</Nullable>
          </PropertyGroup>
        
        </Project>
        """)
        .HasNoIssues();

    [TestCase("CompliantVB.vb")]
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_with_analyzers(string project)
         => new UseInCSharpContextOnly()
        .ForProject(project)
        .HasNoIssues();
}
