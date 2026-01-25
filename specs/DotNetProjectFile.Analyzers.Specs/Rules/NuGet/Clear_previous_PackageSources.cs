namespace Specs.Rules.NuGet.Clear_previous_PackageSources;

public class Reports
{
    [Test]
    public void missing_packageSources() => new DotNetProjectFile.Analyzers.NuGetConfig.ClearPreviousPackageSources()
        .ForInlineNuGetConfig("""
        <configuration>
        </configuration>
        """)
        .HasIssue(Issue.WRN("Proj0301", "Clear previously defined package sources")
            .WithSpan(00, 00, 00, 14));

    [Test]
    public void missing_clear() => new DotNetProjectFile.Analyzers.NuGetConfig.ClearPreviousPackageSources()
        .ForInlineNuGetConfig("""
        <configuration>
          <packageSources>
            <add key="NuGet" value="https://api.nuget.org/v3/index.json" />
          </packageSources>
        </configuration>
        """)
        .HasIssue(Issue.WRN("Proj0301", "Clear previously defined package sources")
            .WithSpan(01, 02, 03, 19));
}

public class Guards
{
    [Test]
    public void Cleared_packageSources() => new DotNetProjectFile.Analyzers.NuGetConfig.ClearPreviousPackageSources()
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
