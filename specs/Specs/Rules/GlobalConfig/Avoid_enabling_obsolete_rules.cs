using DotNetProjectFile.Analyzers.GlobalConfig;

namespace Rules.GlobalConfig.Avoid_enabling_obsolete_rules;

public class Reports
{
    [Test]
    public void enabled_obsolete_rules() => new AvoidEnablingObsoleteRules().ForInlineGlobalconfig("""
        dotnet_diagnostic.AV0000.severity = warning
        dotnet_diagnostic.Proj1000.severity = silent
        """)
        .HasIssues(
            Issue.WRN("Proj1010", "Rule AV0000 is obsolete and should not be enabled").WithSpan(00, 00, 00, 34),
            Issue.WRN("Proj1010", "Rule Proj1000 is obsolete and should not be enabled").WithSpan(01, 00, 01, 36));
}

public class Guards
{
    [Test]
    public void disabled_obsolete_rules() => new AvoidEnablingObsoleteRules().ForInlineGlobalconfig("""
        dotnet_diagnostic.AV0000.severity = none
        """)
        .HasNoIssues();

    [Test]
    public void enabled_non_obsolete_rules() => new AvoidEnablingObsoleteRules().ForInlineGlobalconfig("""
        dotnet_diagnostic.Proj1001.severity = warning
        """)
        .HasNoIssues();
}
