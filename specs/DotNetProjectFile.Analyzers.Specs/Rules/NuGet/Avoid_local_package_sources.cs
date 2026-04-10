namespace Specs.Rules.NuGet.Avoid_local_package_sources;

public class Reports
{
    [Test]
    public void local_package_source() => new DotNetProjectFile.Analyzers.NuGetConfig.AvoidLocalPackageSources()
        .ForInlineNuGetConfig("""
        <configuration>

          <packageSources>
            <clear />
            <add key="local" value="C:/TEMP/packages" />
            <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />
          </packageSources>

        </configuration>
        """)
        .HasIssue(
            Issue.WRN("Proj0306", """Remove this local package source""").WithSpan(04, 04, 04, 48));
}

public class Does_not_report
{
    [Test]
    public void https() => new DotNetProjectFile.Analyzers.NuGetConfig.AvoidLocalPackageSources()
        .ForInlineNuGetConfig("""
        <configuration>
        
          <packageSources>
            <clear />
            <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />
          </packageSources>
        
        </configuration>
        """)
        .HasNoIssues();
}
