using DotNetProjectFile.Analyzers.GlobalConfig;

namespace Rules.GlobalConfig.Configure_diagnostics_granulary;

public class Reports
{
    [Test]
    public void global_suppression() => new ConfigureDiagnosticsGranulary().ForInlineGlobalconfig("""
        dotnet_diagnostic.severity         = none
        dotnet_diagnostic.IDE0004.severity = none
        """)
        .HasIssue(Issue.WRN("Proj4030", "Use a more granular apprach to suppress specific diagnostics").WithSpan(00, 00, 01, 00));

    [Test]
    public void global_configuration() => new ConfigureDiagnosticsGranulary().ForInlineGlobalconfig("""
        dotnet_diagnostic.severity        = suggestion
        dotnet_diagnostic.CA1860.severity = default
        """)
        .HasIssue(Issue.WRN("Proj4031", "Use a more granular apprach to configure diagnostic severties").WithSpan(00, 00, 01, 00));
}

public class Guards
{
    [Test]
    public void granulary_configuration() => new ConfigureDiagnosticsGranulary().ForInlineGlobalconfig("""
        dotnet_diagnostic.IDE0001.severity           = none
        dotnet_diagnostic.IDE0002.severity           = silent
        dotnet_diagnostic.category-Style.severity    = suggestion
        dotnet_diagnostic.category-Security.severity = error
        """)
        .HasNoIssues();
}

