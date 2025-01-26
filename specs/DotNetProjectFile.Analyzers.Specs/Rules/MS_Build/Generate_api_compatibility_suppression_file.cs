namespace Rules.MS_Build.Generate_api_compatibility_suppression_file;

public class Reports
{
    [Test]
    public void on_missing_property() => new GenerateApiCompatibilitySuppressionFile()
        .ForInlineCsproj(@"
            <Project Sdk=""Microsoft.NET.Sdk"">

              <PropertyGroup>
                <TargetFramework>net8.0</TargetFramework>
                <EnablePackageValidation>true</EnablePackageValidation>
              </PropertyGroup>

            </Project>
        ")
        .HasIssues(Issue.WRN("Proj0250", "Define the <ApiCompatGenerateSuppressionFile> node with value 'true' or remove the <EnablePackageValidation> node with value 'true'.").WithSpan(00, 00, 07, 22));

    [Test]
    public void on_disabled_property() => new GenerateApiCompatibilitySuppressionFile()
        .ForInlineCsproj(@"
            <Project Sdk=""Microsoft.NET.Sdk"">

              <PropertyGroup>
                <TargetFramework>net8.0</TargetFramework>
                <EnablePackageValidation>true</EnablePackageValidation>
                <ApiCompatGenerateSuppressionFile>false</ApiCompatGenerateSuppressionFile>
              </PropertyGroup>

            </Project>
        ")
        .HasIssues(Issue.WRN("Proj0250", "Define the <ApiCompatGenerateSuppressionFile> node with value 'true' or remove the <EnablePackageValidation> node with value 'true'.").WithSpan(05, 16, 05, 90));
}

public class Guards
{
    [TestCase("TestProject.cs")]
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project) => new GenerateApiCompatibilitySuppressionFile()
        .ForProject(project)
        .HasNoIssues();

    [Test]
    public void on_enabled_property() => new GenerateApiCompatibilitySuppressionFile()
        .ForInlineCsproj(@"
            <Project Sdk=""Microsoft.NET.Sdk"">

              <PropertyGroup>
                <TargetFramework>net8.0</TargetFramework>
                <EnablePackageValidation>true</EnablePackageValidation>
                <ApiCompatGenerateSuppressionFile>true</ApiCompatGenerateSuppressionFile>
              </PropertyGroup>

            </Project>
        ")
        .HasNoIssues();

    [Test]
    public void on_validation_disabled() => new GenerateApiCompatibilitySuppressionFile()
        .ForInlineCsproj(@"
            <Project Sdk=""Microsoft.NET.Sdk"">

              <PropertyGroup>
                <TargetFramework>net8.0</TargetFramework>
                <ApiCompatGenerateSuppressionFile>false</ApiCompatGenerateSuppressionFile>
              </PropertyGroup>

            </Project>
        ")
        .HasNoIssues();
}
