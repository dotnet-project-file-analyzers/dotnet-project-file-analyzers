using DotNetProjectFile.Analyzers.GlobalConfig;

namespace Rules.GlobalConfig.Stick_to_known_analyzer_rules;

public class Reports
{
    [Test]
    public void unknown_() => new StickToKnownAnalyzerRules().ForInlineGlobalconfig("""
        dotnet_diagnostic.Proj001.severity   = warning
        dotnet_diagnostic.Proj6666.severity  = warning
        dotnet_diagnostic.Proj00007.severity = warning
        """)
        .HasIssues(
            Issue.WRN("Proj4026", "'Proj001' is an uknown rule").WithSpan(0000, 00, 00, 37),
            Issue.WRN("Proj4026", "'Proj6666' is an uknown rule").WithSpan(001, 00, 01, 37),
            Issue.WRN("Proj4026", "'Proj00007' is an uknown rule").WithSpan(02, 00, 02, 37));
}

public class Guards
{
    [Test]
    public void file_only_containing_key_value_pairs() => new StickToKnownAnalyzerRules().ForInlineGlobalconfig("""
        dotnet_diagnostic.Proj1001.severity = warning
        dotnet_diagnostic.Proj4000.severity = error
        """)
        .HasNoIssues();
}

