namespace Rules.Use_CSharp_specific_only_in_CSharp_context;

public class Reports
{
    [Test]
    public void Visual_Basic_context() => new UseInCSharpContextOnly().ForInlineVbproj("""
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
            Issue.WRN("Proj0029", "The property <AllowUnsafeBlocks> is only applicable when using C# and can therefore be removed" /*...*/).WithSpan(05, 4, 05, 48),
            Issue.WRN("Proj0029", "The property <CheckForOverflowUnderflow> is only applicable when using C# and can therefore be removed").WithSpan(06, 4, 06, 63),
            Issue.WRN("Proj0029", "The property <Nullable> is only applicable when using C# and can therefore be removed" /*............*/).WithSpan(07, 4, 07, 31));

    [Test]
    public void FSharp_context() => new UseInCSharpContextOnly().ForInlineFsproj("""
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
                Issue.WRN("Proj0029", "The property <AllowUnsafeBlocks> is only applicable when using C# and can therefore be removed" /*...*/).WithSpan(05, 4, 05, 48),
                Issue.WRN("Proj0029", "The property <CheckForOverflowUnderflow> is only applicable when using C# and can therefore be removed").WithSpan(06, 4, 06, 63));
}

public class Guards
{
    [Test]
    public void CSharp_context() => new UseInCSharpContextOnly().ForInlineCsproj("""
        <Project Sdk="Microsoft.NET.Sdk">
        
          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
            <OutputType>Library</OutputType>
            <AllowUnsafeBlocks>false</AllowUnsafeBlocks>
            <CheckForOverflowUnderflow>true</CheckForOverflowUnderflow>
            <Nullable>enable</Nullable>
          </PropertyGroup>
        
        </Project>
        """)
        .HasNoIssues();
}
