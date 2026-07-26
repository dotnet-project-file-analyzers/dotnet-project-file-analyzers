using DotNetProjectFile.Analyzers.Ini;

namespace Rules.INI.Use_globalconfig_for_Roslyn_diagnostics;

public class Reports
{
    [Test]
    public void dotnet_diagnostics_in_editorconfig() => new UseGlobalConfigForRoslynDiagnostics()
        .ForInlineEditorconfig("""
        root = true

        [*.cs]
        indent_style = space

        [*.cs]
        dotnet_diagnostic.IDE0001.severity = warning
        """)
        .HasIssue(Issue
            .WRN("Proj4052", "Move entry dotnet_diagnostic.IDE0001.severity to the globalconfig")
            .WithSpan(06, 00, 06, 35));
}

public class Guards
{
    [Test]
    public void dotnet_diagnostics_in_globalconfig() => new UseGlobalConfigForRoslynDiagnostics()
        .ForInlineGlobalconfig("""
        is_global = true

        dotnet_diagnostic.IDE0001.severity = warning
        """)
        .HasNoIssues();
}
