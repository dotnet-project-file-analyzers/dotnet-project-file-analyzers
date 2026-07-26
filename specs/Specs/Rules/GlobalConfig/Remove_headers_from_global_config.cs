using DotNetProjectFile.Analyzers.GlobalConfig;

namespace Rules.GlobalConfig.Remove_headers_from_global_config;

public class Reports
{
    [Test]
    public void on_headers() => new RemoveSectionHeader().ForInlineGlobalconfig("""
        is_global = true
        
        dotnet_diagnostic.CA1860.severity  = none       #  Prefer comparing 'Length' to 0 rather than using 'Any()'
        dotnet_diagnostic.CS1591.severity  = suggestion # Missing XML comment for publicly visible type or member

        [IDE001]
        dotnet_diagnostic.IDE0001.severity = warning    # Simplify name
        """)
        .HasIssue(Issue.WRN("Proj4025", "Remove section header").WithSpan(05, 00, 06, 00));
}

public class Guards
{
    [Test]
    public void file_only_containing_key_value_pairs() => new RemoveSectionHeader().ForInlineGlobalconfig("""
        is_global = true

        dotnet_diagnostic.CA1860.severity  = none       #  Prefer comparing 'Length' to 0 rather than using 'Any()'
        dotnet_diagnostic.CS1591.severity  = suggestion # Missing XML comment for publicly visible type or member
        dotnet_diagnostic.IDE0001.severity = warning    # Simplify name
        """)
        .HasNoIssues();
}

