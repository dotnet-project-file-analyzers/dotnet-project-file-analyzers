namespace Specs.Rules.NuGet.Trim_whitespace;

public class Reports
{
    [Test]
    public void mellmalformed_NuGet_config() => new DotNetProjectFile.Analyzers.NuGetConfig.TrimWhitespace()
        .ForInlineNuGetConfig("""
        <configuration>
          <packageSources>
            <clear />
            <add key = "NuGet" value="https://api.nuget.org/v3/index.json " />
          </packageSources>
        </configuration>
        """)
        .HasIssues(
            Issue.WRN("Proj3003", "Remove leading whitespace").WithSpan(03, 12, 03, 13),
            Issue.WRN("Proj3003", "Remove trailing whitespace").WithSpan(3, 14, 03, 15),
            Issue.WRN("Proj3003", "Remove trailing whitespace").WithSpan(3, 65, 03, 66));
}

public class Guards
{
    [Test]
    public void well_formated_NuGet_config() => new DotNetProjectFile.Analyzers.NuGetConfig.TrimWhitespace()
        .ForInlineNuGetConfig("""
        <configuration>
          <packageSources>
            <clear />
            <add key="NuGet" value="https://api.nuget.org/v3/index.json" />
          </packageSources>
        </configuration>
        """)
        .HasNoIssues();
}
