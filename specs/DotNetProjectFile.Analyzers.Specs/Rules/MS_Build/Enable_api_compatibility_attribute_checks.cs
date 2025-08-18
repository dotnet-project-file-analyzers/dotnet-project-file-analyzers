namespace Rules.MS_Build.Enable_api_compatibility_attribute_checks;

public class Reports
{
    [Test]
    public void on_missing_property() => new EnableApiCompatibilityAttributeChecks()
        .ForInlineCsproj("""
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <EnablePackageValidation>true</EnablePackageValidation>
  </PropertyGroup>

</Project>
""")
        .HasIssues(Issue.WRN("Proj0251", "Define the <ApiCompatEnableRuleAttributesMustMatch> node with value 'true' or remove the <EnablePackageValidation> node with value 'true'")
        .WithSpan(00, 00, 00, 32));

    [Test]
    public void on_disabled_property() => new EnableApiCompatibilityAttributeChecks()
        .ForInlineCsproj("""
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <EnablePackageValidation>true</EnablePackageValidation>
    <ApiCompatEnableRuleAttributesMustMatch>false</ApiCompatEnableRuleAttributesMustMatch>
  </PropertyGroup>

</Project>
""")
        .HasIssues(Issue.WRN("Proj0251", "Define the <ApiCompatEnableRuleAttributesMustMatch> node with value 'true' or remove the <EnablePackageValidation> node with value 'true'")
        .WithSpan(05, 04, 05, 90));
}

public class Guards
{
    [TestCase("TestProject.cs")]
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project) => new EnableApiCompatibilityAttributeChecks()
        .ForProject(project)
        .HasNoIssues();

    [Test]
    public void on_enabled_property() => new EnableApiCompatibilityAttributeChecks()
        .ForInlineCsproj("""
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <EnablePackageValidation>true</EnablePackageValidation>
    <ApiCompatEnableRuleAttributesMustMatch>true</ApiCompatEnableRuleAttributesMustMatch>
  </PropertyGroup>

</Project>
""")
        .HasNoIssues();

    [Test]
    public void on_validation_disabled() => new EnableApiCompatibilityAttributeChecks()
        .ForInlineCsproj("""
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <ApiCompatEnableRuleAttributesMustMatch>false</ApiCompatEnableRuleAttributesMustMatch>
  </PropertyGroup>

</Project>
""")
        .HasNoIssues();
}
