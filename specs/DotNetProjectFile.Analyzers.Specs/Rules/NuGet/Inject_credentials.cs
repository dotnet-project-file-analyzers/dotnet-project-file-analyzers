namespace Specs.Rules.NuGet.Inject_credentials;

public class Reports
{
    [Test]
    public void exposed_passwords() => new DotNetProjectFile.Analyzers.NuGetConfig.InjectCredentials()
        .ForInlineNuGetConfig("""
        <configuration>
          <packageSourceCredentials>
            <Key1>
              <add key="Username" value="Admin" />
              <add key="ClearTextPassword" value="33f!!lloppa" />
            </Key1>
            <Key2>
              <add key="Username" value="Root" />
              <add key="ClearTextPassword" value=%SomePasswitha%somewhere" />
            </Key2>
          </packageSourceCredentials>
        </configuration>
        """)
        .HasIssue(Issue.WRN("Proj0301", "Clear previously defined package sources")
            .WithSpan(00, 00, 00, 14));
}

public class Guards
{
    [TestCase("%CI_USER_TOKEN%")]
    [TestCase("%123%")]
    [TestCase("")]
    [TestCase("1")]
    [TestCase("12")]
    public void passwords_considered_to_short_or_placeholders(string password) => new DotNetProjectFile.Analyzers.NuGetConfig.InjectCredentials()
        .ForInlineNuGetConfig($"""
        <configuration>
          <packageSourceCredentials>
            <SomeKey>
              <add key="Username" value="Admin" />
              <add key="ClearTextPassword" value="{password}" />
            </SomeKey>
          </packageSourceCredentials>
        </configuration>
        """)
        .HasNoIssues();
}
