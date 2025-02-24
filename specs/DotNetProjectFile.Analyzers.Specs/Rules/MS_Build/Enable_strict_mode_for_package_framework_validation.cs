namespace Rules.MS_Build.Enable_strict_mode_for_package_framework_validation;

public class Reports
{
    [Test]
    public void on_missing_property() => new EnableStrictModeForPackageFrameworkCompatibilityValidation()
        .ForInlineCsproj(@"
            <Project Sdk=""Microsoft.NET.Sdk"">

              <PropertyGroup>
                <TargetFramework>net8.0</TargetFramework>
                <EnablePackageValidation>true</EnablePackageValidation>
              </PropertyGroup>

            </Project>
        ")
        .HasIssues(Issue.WRN("Proj0249", "Define the <EnableStrictModeForCompatibleFrameworksInPackage> node with value 'true' or remove the <EnablePackageValidation> node with value 'true'").WithSpan(00, 00, 07, 22));

    [Test]
    public void on_disabled_property() => new EnableStrictModeForPackageFrameworkCompatibilityValidation()
        .ForInlineCsproj(@"
            <Project Sdk=""Microsoft.NET.Sdk"">

              <PropertyGroup>
                <TargetFramework>net8.0</TargetFramework>
                <EnablePackageValidation>true</EnablePackageValidation>
                <EnableStrictModeForCompatibleFrameworksInPackage>false</EnableStrictModeForCompatibleFrameworksInPackage>
              </PropertyGroup>

            </Project>
        ")
        .HasIssues(Issue.WRN("Proj0249", "Define the <EnableStrictModeForCompatibleFrameworksInPackage> node with value 'true' or remove the <EnablePackageValidation> node with value 'true'").WithSpan(05, 16, 05, 122));
}

public class Guards
{
    [TestCase("TestProject.cs")]
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project) => new EnableStrictModeForPackageFrameworkCompatibilityValidation()
        .ForProject(project)
        .HasNoIssues();

    [Test]
    public void on_enabled_property() => new EnableStrictModeForPackageFrameworkCompatibilityValidation()
        .ForInlineCsproj(@"
            <Project Sdk=""Microsoft.NET.Sdk"">

              <PropertyGroup>
                <TargetFramework>net8.0</TargetFramework>
                <EnablePackageValidation>true</EnablePackageValidation>
                <EnableStrictModeForCompatibleFrameworksInPackage>true</EnableStrictModeForCompatibleFrameworksInPackage>
              </PropertyGroup>

            </Project>
        ")
        .HasNoIssues();

    [Test]
    public void on_validation_disabled() => new EnableStrictModeForPackageFrameworkCompatibilityValidation()
        .ForInlineCsproj(@"
            <Project Sdk=""Microsoft.NET.Sdk"">

              <PropertyGroup>
                <TargetFramework>net8.0</TargetFramework>
                <EnableStrictModeForCompatibleFrameworksInPackage>false</EnableStrictModeForCompatibleFrameworksInPackage>
              </PropertyGroup>

            </Project>
        ")
        .HasNoIssues();
}
