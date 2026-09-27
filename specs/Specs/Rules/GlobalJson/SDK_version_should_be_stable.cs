using DotNetProjectFile.Analyzers.GlobalJson;

namespace Rules.GlobalJson.SDK_version_should_be_stable;

public class Reports
{
    [Test]
    public void unstable_version() => new SpecifyStableSdkVersion().ForInlineGlobalJson("""
    {
      "sdk": {
        "version": "10.0.100-rc.2.25502.107",
        "allowPrerelease": true,
        "rollForward": "latestMajor"
      }
    }
    """)
    .HasIssues(
        Issue.WRN("Proj6014", "Specify a stable SDK version").WithSpan(2, 15, 2, 40),
        Issue.WRN("Proj6014", "Specify a stable SDK version").WithSpan(3, 23, 3, 27));

    [Test]
    public void not_specified_allow_pre_release() => new SpecifyStableSdkVersion().ForInlineGlobalJson("""
    {
      "sdk": {
        "version": "10.0.400",
        "rollForward": "latestMajor"
      }
    }
    """)
    .HasIssue(
        Issue.WRN("Proj6014", "Specify a stable SDK version").WithSpan(1, 9, 4, 3));
}

public class Guards
{
    [Test]
    public void explictly_disabled() => new SpecifyStableSdkVersion().ForInlineGlobalJson("""
    {
      "sdk": {
        "version": "10.0.401",
        "allowPrerelease": false
      }
    }
    """)
    .HasNoIssues();

    [TestCase("disable")]
    [TestCase("patch")]
    [TestCase("latestPatch")]
    public void disabled_via(string policy) => new SpecifyStableSdkVersion().ForInlineGlobalJson($$"""
    {
      "sdk": {
        "version": "10.0.401",
        "rollForward": "{{policy}}"
      }
    }
    """)
    .HasNoIssues();
}
