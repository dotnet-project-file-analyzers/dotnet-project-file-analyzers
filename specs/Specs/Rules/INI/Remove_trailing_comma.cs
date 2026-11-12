using DotNetProjectFile.Analyzers.Ini;

namespace Rules.INI.Remove_trailing_comma;

public class Reports
{
    [Test]
    public void on_trailing_comma() => new RemoveTrailingComma()
        .ForInlineGlobalconfig("""
        is_global = true

        dotnet_code_quality.CA1822.api_surface = public,private,
        """)
        .HasIssue(
            Issue.WRN("Proj4012", "Remove trailing comma").WithSpan(02, 55, 02, 56));

    [Test]
    public void on_trailing_comma_before_comment_and_whitespace() => new RemoveTrailingComma()
        .ForInlineGlobalconfig("""
        is_global = true

        dotnet_code_quality.CA1822.api_surface = public,private,   # all API surfaces
        """)
        .HasIssue(
            Issue.WRN("Proj4012", "Remove trailing comma").WithSpan(02, 55, 02, 56));

    [Test]
    public void on_trailing_comma_in_ini_file() => new RemoveTrailingComma()
        .ForInlineIni("""
        [section]
        key = public,private,
        """)
        .HasIssue(
            Issue.WRN("Proj4012", "Remove trailing comma").WithSpan(01, 20, 01, 21));
}

public class Guards
{
    [Test]
    public void comma_separated_values_without_trailing_comma() => new RemoveTrailingComma()
        .ForInlineEditorconfig("""
        root = true

        [*.{js,jsx,ts,tsx}]
        dotnet_diagnostic.IDE0001.severity = warning
        dotnet_diagnostic.IDE0002.severity = suggestion
        """)
        .HasNoIssues();

    [Test]
    public void values_without_commas() => new RemoveTrailingComma()
        .ForInlineGlobalconfig("""
        is_global = true

        dotnet_diagnostic.IDE0001.severity = warning
        """)
        .HasNoIssues();
}
