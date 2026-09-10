using DotNetProjectFile.Analyzers.GlobalConfig;

namespace Rules.GlobalConfig.Avoid_enabling_deprecated_rules;

public class Reports
{
    [Test]
    public void enabled_deprecated_rules() => new AvoidEnablingDeprecatedRules().ForInlineGlobalconfig("""
        dotnet_diagnostic.S2228.severity = warning
        dotnet_diagnostic.RCS1012.severity = silent
        """)
        .HasIssues(
            Issue.WRN("Proj1010", "Rule S2228 is deprecated and should not be enabled").WithSpan(00, 00, 00, 33),
            Issue.WRN("Proj1010", "Rule RCS1012 is deprecated and should not be enabled").WithSpan(01, 00, 01, 35));
}

public class Guards
{
    [Test]
    public void disabled_deprecated_rules() => new AvoidEnablingDeprecatedRules().ForInlineGlobalconfig("""
        dotnet_diagnostic.S2228.severity = none
        """)
        .HasNoIssues();

    [Test]
    public void enabled_non_deprecated_rules() => new AvoidEnablingDeprecatedRules().ForInlineGlobalconfig("""
        dotnet_diagnostic.Proj1001.severity = warning
        """)
        .HasNoIssues();
}
