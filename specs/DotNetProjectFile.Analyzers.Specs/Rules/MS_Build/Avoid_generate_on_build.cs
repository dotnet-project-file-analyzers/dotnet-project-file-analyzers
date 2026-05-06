namespace Rules.MS_Build.Avoid_generate_on_build;

public class Reports
{
    [Test]
    public void on_generate_on_build_not_packable()
       => new AvoidGeneratePackageOnBuildWhenNotPackable()
       .ForProject("GeneratePackageNotPackable.cs")
       .HasIssue(
           Issue.WRN("Proj0600", "Avoid defining <GeneratePackageOnBuild> node explicitly when <IsPackable> is 'false'").WithSpan(5, 04, 5, 57));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    [TestCase("EmptyProject.cs")]
    public void Projects_without_issues(string project)
         => new AvoidGeneratePackageOnBuildWhenNotPackable()
        .ForProject(project)
        .HasNoIssues();

    [Test]
    public void Generate_on_build_inside_false_condition_does_not_fire() => new AvoidGeneratePackageOnBuildWhenNotPackable()
        .ForInlineCsproj("""
            <Project Sdk="Microsoft.NET.Sdk">
              <PropertyGroup>
                <TargetFramework>net10.0</TargetFramework>
                <IsPackable>false</IsPackable>
              </PropertyGroup>
              <PropertyGroup Condition="'a' == 'b'">
                <GeneratePackageOnBuild>true</GeneratePackageOnBuild>
              </PropertyGroup>
            </Project>
            """)
        .HasNoIssues();
}
