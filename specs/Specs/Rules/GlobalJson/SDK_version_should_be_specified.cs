using DotNetProjectFile.Analyzers.GlobalJson;

namespace Rules.GlobalJson.SDK_version_should_be_specified;

public class Reports
{
    [Test]
    public void Different_casing() => new SpecifySdkVersion().ForInlineGlobalJson("""
        {
          "SDK": {
            "version": "10.0.400",
            "rollForward": "latestPatch",
            "allowPrerelease": false
          }
        }
        """)
        .HasIssue(Issue.WRN("Proj6011", "No valid SDK version has been specified"));

    [Test]
    public void nummeric_value() => new SpecifySdkVersion().ForInlineGlobalJson("""
        {
          "sdk": {
            "version": 10,
            "rollForward": "latestPatch",
            "allowPrerelease": false
          }
        }
        """)
        .HasIssue(Issue.WRN("Proj6011", "No valid SDK version has been specified").WithSpan(02, 15, 02, 17));


    [Test]
    public void invalid_version() => new SpecifySdkVersion().ForInlineGlobalJson("""
        {
          "sdk": {
            "version": "no-version",
            "rollForward": "latestPatch",
            "allowPrerelease": false
          }
        }
        """)
        .HasIssue(Issue.WRN("Proj6011", "No valid SDK version has been specified").WithSpan(02, 15, 02, 27));
}

public class Guards
{
    [Test]
    public void Specfied_version() => new SpecifySdkVersion().ForInlineGlobalJson("""
        {
          "sdk": {
            "version": "10.0.400",
            "rollForward": "latestPatch",
            "allowPrerelease": false
          }
        }
        """)
        .HasNoIssues();
}
