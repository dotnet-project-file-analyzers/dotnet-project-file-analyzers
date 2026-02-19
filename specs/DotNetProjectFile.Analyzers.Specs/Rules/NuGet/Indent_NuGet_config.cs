namespace Rules.NuGet.Indent_NuGet_config;

public class Reports
{
    [Test]
    public void Faulty_indented() => new DotNetProjectFile.Analyzers.NuGetConfig.IndentXml()
        .ForInlineNuGetConfig("""
        <configuration>
            <packageSources>
            <clear />
        	<add key="NuGet" value="https://api.nuget.org/v3/index.json" />
          </packageSources>
        </configuration>
        """)
        .HasIssues(
            Issue.WRN("Proj1700", "The element <packageSources> has not been properly indented").WithSpan(01, 04, 01, 19),
            Issue.WRN("Proj1700", "The element <add> has not been properly indented").WithSpan(03, 01, 03, 64));
}

public class Guards
{
    [Test]
    public void Faulty_indented() => new DotNetProjectFile.Analyzers.NuGetConfig.IndentXml()
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
