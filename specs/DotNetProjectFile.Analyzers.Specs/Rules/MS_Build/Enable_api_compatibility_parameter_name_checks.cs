namespace Rules.MS_Build.Enable_api_compatibility_parameter_name_checks;

public class Reports
{
    [Test]
    public void on_missing_property() => new EnableApiCompatibilityParameterNameChecks()
        .ForInlineCsproj(@"
            <Project Sdk=""Microsoft.NET.Sdk"">

              <PropertyGroup>
                <TargetFramework>net8.0</TargetFramework>
                <EnablePackageValidation>true</EnablePackageValidation>
              </PropertyGroup>

            </Project>
        ")
        .HasIssues(Issue.WRN("Proj0252", "Define the <ApiCompatEnableRuleCannotChangeParameterName> node with value 'true' or remove the <EnablePackageValidation> node with value 'true'.").WithSpan(00, 00, 07, 22));

    [Test]
    public void on_disabled_property() => new EnableApiCompatibilityParameterNameChecks()
        .ForInlineCsproj(@"
            <Project Sdk=""Microsoft.NET.Sdk"">

              <PropertyGroup>
                <TargetFramework>net8.0</TargetFramework>
                <EnablePackageValidation>true</EnablePackageValidation>
                <ApiCompatEnableRuleCannotChangeParameterName>false</ApiCompatEnableRuleCannotChangeParameterName>
              </PropertyGroup>

            </Project>
        ")
        .HasIssues(Issue.WRN("Proj0252", "Define the <ApiCompatEnableRuleCannotChangeParameterName> node with value 'true' or remove the <EnablePackageValidation> node with value 'true'.").WithSpan(05, 16, 05, 114));
}

public class Guards
{
    [TestCase("TestProject.cs")]
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project) => new EnableApiCompatibilityParameterNameChecks()
        .ForProject(project)
        .HasNoIssues();

    [Test]
    public void on_enabled_property() => new EnableApiCompatibilityParameterNameChecks()
        .ForInlineCsproj(@"
            <Project Sdk=""Microsoft.NET.Sdk"">

              <PropertyGroup>
                <TargetFramework>net8.0</TargetFramework>
                <EnablePackageValidation>true</EnablePackageValidation>
                <ApiCompatEnableRuleCannotChangeParameterName>true</ApiCompatEnableRuleCannotChangeParameterName>
              </PropertyGroup>

            </Project>
        ")
        .HasNoIssues();

    [Test]
    public void on_validation_disabled() => new EnableApiCompatibilityParameterNameChecks()
        .ForInlineCsproj(@"
            <Project Sdk=""Microsoft.NET.Sdk"">

              <PropertyGroup>
                <TargetFramework>net8.0</TargetFramework>
                <ApiCompatEnableRuleCannotChangeParameterName>false</ApiCompatEnableRuleCannotChangeParameterName>
              </PropertyGroup>

            </Project>
        ")
        .HasNoIssues();
}
