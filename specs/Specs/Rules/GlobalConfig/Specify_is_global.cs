using DotNetProjectFile.Analyzers.GlobalConfig;

namespace Specs.Rules.GlobalConfig.Specify_is_global;

public class Reports
{
    [Test]
    public void on_false() => new SpecifyIsGlobal().ForInlineGlobalconfig("""
        is_global = false
        
        dotnet_diagnostic.CA1860.severity = none
        """)
        .HasIssue(Issue.WRN("Proj4026", "Enable is_global").WithSpan(00, 11, 00, 17));


    [Test]
    public void when_not_set() => new SpecifyIsGlobal().ForInlineGlobalconfig("""
        dotnet_diagnostic.CA1860.severity = none
        """)
        .HasIssue(Issue.WRN("Proj4026", "Set is_global").WithSpan(00, 00, 00, 33));
}

public class Guards
{
    [Test]
    public void file_only_containing_key_value_pairs() => new SpecifyIsGlobal().ForInlineGlobalconfig("""
        is_global = true

        dotnet_diagnostic.CA1860.severity = none
        """)
        .HasNoIssues();
}

