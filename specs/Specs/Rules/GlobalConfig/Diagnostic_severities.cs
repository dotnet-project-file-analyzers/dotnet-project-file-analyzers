using DotNetProjectFile.Analyzers.GlobalConfig;

namespace Rules.GlobalConfig.Diagnostic_severities;

public class Reports
{
    [Test]
    public void on_unknown_value() => new DiagnosticSeverities().ForInlineGlobalconfig("""
        dotnet_diagnostic.IDE0002.severity = hidden
        dotnet_diagnostic.IDE0004.severity = info
        """)
        .HasIssues(
            Issue.WRN("Proj4027", "diagnostic severity 'hidden' is unknown").WithSpan(00, 36, 00, 43),
            Issue.WRN("Proj4027", "diagnostic severity 'info' is unknown").WithSpan(01, 36, 01, 41));

    [Test]
    public void on_default() => new DiagnosticSeverities().ForInlineGlobalconfig("""
        dotnet_diagnostic.CA1860.severity = default
        """)
        .HasIssue(Issue.WRN("Proj4028", "Use explicit diagnostic severity level").WithSpan(00, 35, 00, 43));
}

public class Guards
{
    [Test]
    public void file_only_containing_key_value_pairs() => new DiagnosticSeverities().ForInlineGlobalconfig("""
        dotnet_diagnostic.IDE0001.severity = none
        dotnet_diagnostic.IDE0002.severity = silent
        dotnet_diagnostic.IDE0004.severity = suggestion
        dotnet_diagnostic.IDE0005.severity = warning
        dotnet_diagnostic.IDE0017.severity = error
        dotnet_diagnostic.IDE0018.severity = ERROR
        """)
        .HasNoIssues();
}

