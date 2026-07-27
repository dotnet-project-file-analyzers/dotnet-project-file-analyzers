using DotNetProjectFile.Analyzers.GlobalConfig;

namespace Rules.GlobalConfig.Use_known_diagnostic_IDs;

public class Reports
{
    [Test]
    public void unknown_ids() => new UseKnownDiagnosticIds().ForInlineGlobalconfig("""
        dotnet_diagnostic.Proj001.severity   = warning
        dotnet_diagnostic.Proj6666.severity  = warning
        dotnet_diagnostic.Proj00007.severity = warning
        """)
        .HasIssues(
            Issue.WRN("Proj4029", "Diagnostic analyzer rule 'Proj001' is unknown").WithSpan(0000, 00, 00, 37),
            Issue.WRN("Proj4029", "Diagnostic analyzer rule 'Proj6666' is unknown").WithSpan(001, 00, 01, 37),
            Issue.WRN("Proj4029", "Diagnostic analyzer rule 'Proj00007' is unknown").WithSpan(02, 00, 02, 37));
}

public class Guards
{
    [Test]
    public void known_ids() => new UseKnownDiagnosticIds().ForInlineGlobalconfig("""
        dotnet_diagnostic.Proj1001.severity = warning
        dotnet_diagnostic.Proj4000.severity = error
        """)
        .HasNoIssues();
}

