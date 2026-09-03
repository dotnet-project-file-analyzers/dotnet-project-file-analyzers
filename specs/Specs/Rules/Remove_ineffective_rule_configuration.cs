namespace Rules.Remove_ineffective_rule_configuration;

public class Reports_not_configurable_rule_IDs_in
{
    [Test]
    public void MSBuild_files() => new RemoveIneffectiveRuleConfiguration().ForInlineCsproj("""
        <Project Sdk="Microsoft.NET.Sdk">

          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
            <NoWarn>CS0016</NoWarn>
            <WarningsAsErrors>S101;SA1603</WarningsAsErrors>
            <WarningsNotAsErrors>CS1680;CS1681;QW0001;AK1001</WarningsNotAsErrors>
          </PropertyGroup>

        </Project>
        """)
        .HasIssues(
            Issue.WRN("Proj1008", "Rule CS0016 is not-configurable and cannot be modified").WithSpan(04, 04, 04, 27),
            Issue.WRN("Proj1008", "Rule SA1603 is not-configurable and cannot be modified").WithSpan(05, 04, 05, 52),
            Issue.WRN("Proj1008", "Rule CS1680 is not-configurable and cannot be modified").WithSpan(06, 04, 06, 74),
            Issue.WRN("Proj1008", "Rule CS1681 is not-configurable and cannot be modified").WithSpan(06, 04, 06, 74),
            Issue.WRN("Proj1009", "Rule AK1001 does no longer exist").WithSpan(06, 04, 06, 74));

    [Test]
    public void GlobalConfig_files() => new DotNetProjectFile.Analyzers.GlobalConfig.RemoveIneffectiveRuleConfiguration().ForInlineGlobalconfig("""
        is_global = true

        dotnet_diagnostic.CS0016.severity   = none
        dotnet_diagnostic.SA1603.severity   = warning
        dotnet_diagnostic.CS1680.severity   = suggestion
        dotnet_diagnostic.CS1681.severity   = error

        dotnet_diagnostic.IDE1006.severity  = none
        dotnet_diagnostic.AK1001.severity   = warning
        """)
        .HasIssues(
            Issue.WRN("Proj1008", "Rule CS0016 is not-configurable and cannot be modified").WithSpan(02, 00, 02, 36),
            Issue.WRN("Proj1008", "Rule SA1603 is not-configurable and cannot be modified").WithSpan(03, 00, 03, 36),
            Issue.WRN("Proj1008", "Rule CS1680 is not-configurable and cannot be modified").WithSpan(04, 00, 04, 36),
            Issue.WRN("Proj1008", "Rule CS1681 is not-configurable and cannot be modified").WithSpan(05, 00, 05, 36),
            Issue.WRN("Proj1009", "Rule AK1001 is does not longer exist").WithSpan(08, 00, 08, 36));

    [Test]
    public void CSharp_files() => new DotNetProjectFile.Analyzers.CSharp.RemoveIneffectiveRuleConfiguration()
        .ForCS()
        .AddSource("Cases/Remove_ineffective_rule_configuration.cs")
        .Verify();
}
