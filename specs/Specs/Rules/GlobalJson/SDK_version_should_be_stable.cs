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
        "rollForward": true
      }
    }
    """)
    .HasIssues(
        Issue.WRN("Proj6014", "Specify a stable SDK version").WithSpan(2, 15, 2, 40),
        Issue.WRN("Proj6014", "Specify a stable SDK version").WithSpan(3, 23, 3, 27));
}

public class Guards
{
    [Test]
    public void stable_version() => new SpecifyStableSdkVersion().ForInlineGlobalJson("""
    {
      "sdk": {
        "version": "10.0.401",
        "allowPrerelease": false
      }
    }
    """)
    .HasNoIssues();
}
