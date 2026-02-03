namespace Specs.Rules.NuGet.Prefer_HTTPS_sources;

public class Reports
{
    [Test]
    public void missing_packageSources() => new DotNetProjectFile.Analyzers.NuGetConfig.PreferHttpsSources()
        .ForInlineNuGetConfig("""
        <configuration>
          <packageSources>
            <add key="NuGet" value="http://api.nuget.org/v3/index.json" allowInsecureConnections="true" />
          </packageSources>
        </configuration>
        """)
        .HasIssue(Issue.WRN("Proj0306", "Prefer the HTTPS protocol for package sources"));
}

public class Guards
{
    [Test]
    public void Cleared_packageSources() => new DotNetProjectFile.Analyzers.NuGetConfig.PreferHttpsSources()
        .ForInlineNuGetConfig("""
        <configuration>
          <packageSources>
            <add key="NuGet" value="https://api.nuget.org/v3/index.json" />
          </packageSources>
        </configuration>
        """)
        .HasNoIssues();
}
