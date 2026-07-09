namespace Rules.Use_VB_specific_only_in_VB_context;

public class Reports
{
    [Test]
    public void CSharp_context() => new UseInVBContextOnly()
        .ForInlineCsproj("""
        <Project Sdk="Microsoft.NET.Sdk">

          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
            <OutputType>Library</OutputType>
            <IsPackable>false</IsPackable>
            <FrameworkPathOverride />
            <NoVBRuntimeReference>true</NoVBRuntimeReference>
            <OptionExplicit>On</OptionExplicit>
            <OptionInfer>On</OptionInfer>
            <OptionStrict>On</OptionStrict>
            <RemoveIntegerChecks>true</RemoveIntegerChecks>
            <VbcVerbosity>Verbose</VbcVerbosity>
            <VbcToolPath />
          </PropertyGroup>

        </Project>
        """)
        .HasIssues(
            Issue.WRN("Proj0030", "The property <FrameworkPathOverride> is only applicable when using VB.NET and can therefor be removed" /*.*/).WithSpan(06, 4, 06, 29),
            Issue.WRN("Proj0030", "The property <NoVBRuntimeReference> is only applicable when using VB.NET and can therefor be removed" /*..*/).WithSpan(07, 4, 07, 53),
            Issue.WRN("Proj0030", "The property <OptionExplicit> is only applicable when using VB.NET and can therefor be removed" /*........*/).WithSpan(08, 4, 08, 39),
            Issue.WRN("Proj0030", "The property <OptionInfer> is only applicable when using VB.NET and can therefor be removed" /*...........*/).WithSpan(09, 4, 09, 33),
            Issue.WRN("Proj0030", "The property <OptionStrict> is only applicable when using VB.NET and can therefor be removed" /*..........*/).WithSpan(10, 4, 10, 35),
            Issue.WRN("Proj0030", "The property <RemoveIntegerChecks> is only applicable when using VB.NET and can therefor be removed" /*...*/).WithSpan(11, 4, 11, 51),
            Issue.WRN("Proj0030", "The property <VbcVerbosity> is only applicable when using VB.NET and can therefor be removed" /*..........*/).WithSpan(12, 4, 12, 40),
            Issue.WRN("Proj0030", "The property <VbcToolPath> is only applicable when using VB.NET and can therefor be removed" /*...........*/).WithSpan(13, 4, 13, 19));
}

public class Guards
{
    [Test]
    public void Visual_Basic_context() => new UseInVBContextOnly()
        .ForInlineVbproj("""
        <Project Sdk="Microsoft.NET.Sdk">

          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
            <OutputType>Library</OutputType>
            <IsPackable>false</IsPackable>
            <FrameworkPathOverride />
            <NoVBRuntimeReference>true</NoVBRuntimeReference>
            <OptionExplicit>On</OptionExplicit>
            <OptionInfer>On</OptionInfer>
            <OptionStrict>On</OptionStrict>
            <RemoveIntegerChecks>true</RemoveIntegerChecks>
            <VbcVerbosity>Verbose</VbcVerbosity>
            <VbcToolPath />
          </PropertyGroup>

        </Project>
        """)
        .HasNoIssues();

    [TestCase("CompliantVB.vb")]
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_with_analyzers(string project) => new UseInVBContextOnly()
        .ForProject(project)
        .HasNoIssues();
}
