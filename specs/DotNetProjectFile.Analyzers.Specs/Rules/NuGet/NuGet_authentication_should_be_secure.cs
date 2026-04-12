namespace Specs.Rules.NuGet.NuGet_authentication_should_be_secure;

public class Reports
{
    [Test]
    public void exposed_passwords() => new DotNetProjectFile.Analyzers.NuGetConfig.NuGetAuthenticationShouldBeSecure()
        .ForInlineNuGetConfig("""
        <configuration>
          <packageSourceCredentials>
            <Key1>
              <add key="Username" value="Admin" />
              <add key="ClearTextPassword" value="33f!!lloppa" />
            </Key1>
            <Key2>
              <add key="Username" value="Root" />
              <add key="ClearTextPassword" value="%SomePasswitha%somewhere" />
            </Key2>
          </packageSourceCredentials>
        </configuration>
        """)
        .HasIssues(
            Issue.WRN("Proj0302", "Use an environment variable instead of a plain text password").WithSpan(04, 42, 04, 53),
            Issue.WRN("Proj0302", "Use an environment variable instead of a plain text password").WithSpan(08, 42, 08, 66));
}

public class Guards
{
    [TestCase("%CI_USER_TOKEN%")]
    [TestCase("%123%")]
    [TestCase("1")]
    [TestCase("12")]
    public void passwords_considered_too_short_or_placeholders(string password) => new DotNetProjectFile.Analyzers.NuGetConfig.NuGetAuthenticationShouldBeSecure()
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
