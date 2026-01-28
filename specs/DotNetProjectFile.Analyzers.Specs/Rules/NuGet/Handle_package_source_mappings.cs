namespace Specs.Rules.NuGet.Handle_package_source_mappings;

public class Reports
{
    [Test]
    public void missing_mappings() => new DotNetProjectFile.Analyzers.NuGetConfig.HandlePackageSourceMappings()
        .ForInlineNuGetConfig("""
        <configuration>

          <packageSources>
            <clear />
            <add key="Internals" value="https://pkgs.dev.azure.com/company/_packaging/Components/nuget/v3/index.json" />
            <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />
          </packageSources>

        </configuration>
        """)
        .HasIssues(
            Issue.WRN("Proj0303", """The <packageSource key="Internals"> is missing a <packageSourceMapping>""").WithSpan(04, 04, 04, 112),
            Issue.WRN("Proj0303", """The <packageSource key="nuget.org"> is missing a <packageSourceMapping>""").WithSpan(05, 04, 05, 71));

    [Test]
    public void missing_mapping_and_extra() => new DotNetProjectFile.Analyzers.NuGetConfig.HandlePackageSourceMappings()
        .ForInlineNuGetConfig("""
        <configuration>
        
          <packageSources>
            <clear />
            <add key="Internal-packages" value="https://pkgs.dev.azure.com/company/_packaging/Components/nuget/v3/index.json" />
            <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />
          </packageSources>
        
          <packageSourceMapping>
            <packageSource key="other.source">
              <package pattern="*" />
            </packageSource>
            <packageSource key="Internal-packages">
              <package pattern="Qowaiv.CodeGeneration" />
              <package pattern="Company.*" />
            </packageSource>
          </packageSourceMapping>
        
        </configuration>
        """)
        .HasIssue(Issue.WRN("Proj0303", """The <packageSource key="nuget.org"> is missing a <packageSourceMapping>""").WithSpan(05, 04, 05, 71));

    [Test]
    public void duplicate_mappings() => new DotNetProjectFile.Analyzers.NuGetConfig.HandlePackageSourceMappings()
        .ForInlineNuGetConfig("""
        <configuration>
        
          <packageSources>
            <clear />
            <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />
          </packageSources>
        
          <packageSourceMapping>
            <packageSource key="nuget.org">
              <package pattern="Company.*" />
              <package pattern="Company.*" />
              <package pattern="*" />
            </packageSource>
          </packageSourceMapping>
        
        </configuration>
        """)
        .HasIssue(Issue.WRN("Proj0304", "The mapping 'Company.*' is not unique").WithSpan(10, 06, 10, 37));
}

public class Guards
{
    [Test]
    public void Single_source() => new DotNetProjectFile.Analyzers.NuGetConfig.HandlePackageSourceMappings()
        .ForInlineNuGetConfig("""
        <configuration>
        
          <packageSources>
            <clear />
            <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />
          </packageSources>

        </configuration>
        """)
        .HasNoIssues();

    [Test]
    public void mapped_packageSources() => new DotNetProjectFile.Analyzers.NuGetConfig.HandlePackageSourceMappings()
       .ForInlineNuGetConfig("""
        <configuration>

          <packageSources>
            <clear />
            <add key="Internal-packages" value="https://pkgs.dev.azure.com/company/_packaging/Components/nuget/v3/index.json" />
            <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />
          </packageSources>

          <packageSourceMapping>
            <packageSource key="nuget.org">
              <package pattern="*" />
            </packageSource>
            <packageSource key="Internal-packages">
              <package pattern="Qowaiv.CodeGeneration" />
              <package pattern="Company.*" />
            </packageSource>
          </packageSourceMapping>

        </configuration>
        """)
       .HasNoIssues();
}
