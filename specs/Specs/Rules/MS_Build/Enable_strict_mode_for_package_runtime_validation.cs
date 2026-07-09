namespace Rules.MS_Build.Enable_strict_mode_for_package_runtime_validation;

public class Reports
{
    [Test]
    public void on_disabled_property() => new EnableStrictModeForPackageRuntimeCompatibilityValidation()
        .ForInlineCsproj(@"
            <Project Sdk=""Microsoft.NET.Sdk"">

              <PropertyGroup>
                <TargetFramework>net10.0</TargetFramework>
                <EnablePackageValidation>true</EnablePackageValidation>
                <EnableStrictModeForCompatibleTfms>false</EnableStrictModeForCompatibleTfms>
              </PropertyGroup>

            </Project>
        ")
        .HasIssues(Issue.WRN("Proj0248", "Define the <EnableStrictModeForCompatibleTfms> node with value 'true' or remove the <EnableStrictModeForCompatibleTfms> node with value 'false' or remove the <EnablePackageValidation> node with value 'true'").WithSpan(05, 16, 05, 92));
}

public class Guards
{
    [TestCase("TestProject.cs")]
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project) => new EnableStrictModeForPackageRuntimeCompatibilityValidation()
        .ForProject(project)
        .HasNoIssues();

    [Test]
    public void on_missing_property() => new EnableStrictModeForPackageRuntimeCompatibilityValidation()
        .ForInlineCsproj(@"
            <Project Sdk=""Microsoft.NET.Sdk"">

              <PropertyGroup>
                <TargetFramework>net10.0</TargetFramework>
                <EnablePackageValidation>true</EnablePackageValidation>
              </PropertyGroup>

            </Project>
        ")
        .HasNoIssues();

    [Test]
    public void on_validation_disabled() => new EnableStrictModeForPackageBaselineValidation()
        .ForInlineCsproj(@"
            <Project Sdk=""Microsoft.NET.Sdk"">

              <PropertyGroup>
                <TargetFramework>net10.0</TargetFramework>
                <EnableStrictModeForCompatibleTfms>false</EnableStrictModeForCompatibleTfms>
              </PropertyGroup>

            </Project>
        ")
        .HasNoIssues();
}
