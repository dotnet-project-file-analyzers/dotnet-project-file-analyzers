namespace Specs.Rules.NuGet.Clear_previous_PackageSources;

public class Reports
{
    [Test]
    public void Faulty_indented() => new DotNetProjectFile.Analyzers.NuGetConfig.ClearPreviousPackageSources()
        .ForInlineNuGetConfig("""
        <configuration>
          <packageSources>
            <add key="NuGet" value="https://api.nuget.org/v3/index.json" />
          </packageSources>
          <packageSourceMapping>
            <packageSource key="NuGet">
              <package pattern="*" />
            </packageSource>
          </packageSourceMapping>
        </configuration>
        """)
        .HasIssue(Issue.WRN("Proj0601", "Clear previous defined package sources")
            .WithSpan(01, 02, 03, 19));
}

public class Guards
{
    [Test]
    public void Cleared_packageSrouces() => new DotNetProjectFile.Analyzers.NuGetConfig.ClearPreviousPackageSources()
        .ForInlineNuGetConfig("""
        <configuration>
          <packageSources>
            <clear />
            <add key="NuGet" value="https://api.nuget.org/v3/index.json" />
          </packageSources>
          <packageSourceMapping>
            <packageSource key="NuGet">
              <package pattern="*" />
            </packageSource>
          </packageSourceMapping>
        </configuration>
        """)
        .HasNoIssues();
}
