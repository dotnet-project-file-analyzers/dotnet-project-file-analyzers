using DotNetProjectFile.Analyzers.Ini;

namespace Specs.Rules.INI.Use_globalconfig_for_Roslyn_diagnostics;

public class Reports
{
    [Test]
    public void empty_sections() => new UseGlobalConfigForRoslynDiagnostics()
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
    public void empty_sections() => new UseGlobalConfigForRoslynDiagnostics()
        .ForInlineEditorconfig("""
        is_global = true

        dotnet_diagnostic.IDE0001.severity = warning
        """)
        .HasNoIssues();
}
