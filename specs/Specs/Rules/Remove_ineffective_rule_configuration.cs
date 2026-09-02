namespace Rules.Remove_ineffective_rule_configuration;

public class Reports
{
    [Test]
    public void on_not_configurable_rule_IDs() => new RemoveIneffectiveRuleConfiguration().ForInlineCsproj("""
        <Project Sdk="Microsoft.NET.Sdk">

          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
            <NoWarn>CS0016</NoWarn>
            <WarningsAsErrors>S101;SA1603</WarningsAsErrors>
            <WarningsNotAsErrors>CS1680;CS1681;QW0001</WarningsNotAsErrors>
          </PropertyGroup>

        </Project>
        """)
        .HasIssues(
            Issue.WRN("Proj1007", "Rule CS0016 is not-configurable and cannot be modified").WithSpan(04, 04, 04, 27),
            Issue.WRN("Proj1007", "Rule SA1603 is not-configurable and cannot be modified").WithSpan(05, 04, 05, 52),
            Issue.WRN("Proj1007", "Rule CS1680 is not-configurable and cannot be modified").WithSpan(06, 04, 06, 67),
            Issue.WRN("Proj1007", "Rule CS1681 is not-configurable and cannot be modified").WithSpan(06, 04, 06, 67));
}
