namespace Rules.NuGet.Omit_XML_declaration;

public class Reports
{
    [Test]
    public void config_with_XML_declaration() => new DotNetProjectFile.Analyzers.NuGetConfig.OmitXmlDeclarations()
       .ForInlineNuGetConfig("""
        <?xml version="1.0" ?>
        <configuration>
          <packageSources>
            <clear />
            <add key="NuGet" value="https://api.nuget.org/v3/index.json" />
          </packageSources>
        </configuration>
        """)
       .HasIssue(Issue.WRN("Proj1702", "Remove the XML declaration as it is redundant")
           .WithSpan(01, 00, 01, 14));
}
public class Guards
{
    [Test]
    public void config_without_XML_declaration() => new DotNetProjectFile.Analyzers.NuGetConfig.OmitXmlDeclarations()
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
