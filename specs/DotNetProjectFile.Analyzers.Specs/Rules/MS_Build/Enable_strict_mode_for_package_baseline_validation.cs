namespace Rules.MS_Build.Enable_strict_mode_for_package_baseline_validation;

public class Reports
{
    [Test]
    public void on_missing_property() => new EnableStrictModeForPackageBaselineValidation()
        .ForInlineCsproj(@"
            <Project Sdk=""Microsoft.NET.Sdk"">

              <PropertyGroup>
                <TargetFramework>net8.0</TargetFramework>
                <EnablePackageValidation>true</EnablePackageValidation>
                <PackageValidationBaselineName>DotNetProjectFile.Analyzers</PackageValidationBaselineName>
                <PackageValidationBaselineVersion>1.5.0</PackageValidationBaselineVersion>
              </PropertyGroup>

            </Project>
        ")
        .HasIssues(Issue.WRN("Proj0247", "Define the <EnableStrictModeForBaselineValidation> node with value 'true' or remove the <PackageValidationBaselineVersion> node or remove the <EnablePackageValidation> node with value 'true'.").WithSpan(00, 00, 09, 22));

    [Test]
    public void on_disabled_property() => new EnableStrictModeForPackageBaselineValidation()
        .ForInlineCsproj(@"
            <Project Sdk=""Microsoft.NET.Sdk"">

              <PropertyGroup>
                <TargetFramework>net8.0</TargetFramework>
                <EnablePackageValidation>true</EnablePackageValidation>
                <PackageValidationBaselineName>DotNetProjectFile.Analyzers</PackageValidationBaselineName>
                <PackageValidationBaselineVersion>1.5.0</PackageValidationBaselineVersion>
                <EnableStrictModeForBaselineValidation>false</EnableStrictModeForBaselineValidation>
              </PropertyGroup>

            </Project>
        ")
        .HasIssues(Issue.WRN("Proj0247", "Define the <EnableStrictModeForBaselineValidation> node with value 'true' or remove the <PackageValidationBaselineVersion> node or remove the <EnablePackageValidation> node with value 'true'.").WithSpan(07, 16, 07, 100));
}

public class Guards
{
    [TestCase("TestProject.cs")]
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project) => new EnableStrictModeForPackageBaselineValidation()
        .ForProject(project)
        .HasNoIssues();

    [Test]
    public void on_enabled_property() => new EnableStrictModeForPackageBaselineValidation()
        .ForInlineCsproj(@"
            <Project Sdk=""Microsoft.NET.Sdk"">

              <PropertyGroup>
                <TargetFramework>net8.0</TargetFramework>
                <EnablePackageValidation>true</EnablePackageValidation>
                <PackageValidationBaselineName>DotNetProjectFile.Analyzers</PackageValidationBaselineName>
                <PackageValidationBaselineVersion>1.5.0</PackageValidationBaselineVersion>
                <EnableStrictModeForBaselineValidation>true</EnableStrictModeForBaselineValidation>
              </PropertyGroup>

            </Project>
        ")
        .HasNoIssues();

    [Test]
    public void on_validation_disabled() => new EnableStrictModeForPackageBaselineValidation()
        .ForInlineCsproj(@"
            <Project Sdk=""Microsoft.NET.Sdk"">

              <PropertyGroup>
                <TargetFramework>net8.0</TargetFramework>
                <PackageValidationBaselineName>DotNetProjectFile.Analyzers</PackageValidationBaselineName>
                <PackageValidationBaselineVersion>1.5.0</PackageValidationBaselineVersion>
                <EnableStrictModeForBaselineValidation>false</EnableStrictModeForBaselineValidation>
              </PropertyGroup>

            </Project>
        ")
        .HasNoIssues();
}
