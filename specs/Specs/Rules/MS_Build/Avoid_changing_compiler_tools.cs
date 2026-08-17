namespace Rules.MS_Build.Avoid_changing_compiler_tools;

public class Reports
{
    [Test]
    public void on_compiler_tools_properties() => new AvoidChangingCompilerTools().ForInlineCsproj("""
        <Project Sdk="Microsoft.NET.Sdk">

          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
            <CscToolExe>$(CscToolExe)</CscToolExe>
            <CscToolPath>$(CscToolPath)</CscToolPath>
            <VbcToolExe>$(VbcToolExe)</VbcToolExe>
            <VbcToolPath>$(VbcToolPath)</VbcToolPath>
            <DotnetFscCompilerPath>$(DotnetFscCompilerPath)</DotnetFscCompilerPath>
          </PropertyGroup>

        </Project>
        """)
       .HasIssues(
            Issue.WRN("Proj0054", "Remove <CscToolExe>"/*.......*/).WithSpan(04, 04, 04, 42),
            Issue.WRN("Proj0054", "Remove <CscToolPath>"/*......*/).WithSpan(05, 04, 05, 45),
            Issue.WRN("Proj0054", "Remove <VbcToolExe>"/*.......*/).WithSpan(06, 04, 06, 42),
            Issue.WRN("Proj0054", "Remove <VbcToolPath>"/*......*/).WithSpan(07, 04, 07, 45),
            Issue.WRN("Proj0054", "Remove <DotnetFscCompilerPath>").WithSpan(08, 04, 08, 75));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project) => new AvoidChangingCompilerTools()
        .ForProject(project)
        .HasNoIssues();
}
