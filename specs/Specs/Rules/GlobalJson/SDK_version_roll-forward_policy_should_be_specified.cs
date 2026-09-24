using DotNetProjectFile.Analyzers.GlobalJson;

namespace Rules.GlobalJson.SDK_version_roll_forward_policy_should_be_specified;

public class Reports
{
    [Test]
    public void Different_casing() => new SpecifySdkRollForwardPolicy().ForInlineGlobalJson("""
        {
          "SDK": {
            "version": "10.0.400",
            "rollForward": "latestPatch",
            "allowPrerelease": false
          }
        }
        """)
        .HasIssue(Issue.WRN("Proj6012", "No valid SDK version roll-forward policy has been specified"));

    [Test]
    public void nummeric_value() => new SpecifySdkRollForwardPolicy().ForInlineGlobalJson("""
        {
          "sdk": {
            "version": "10.0.400",
            "rollForward": 4,
            "allowPrerelease": false
          }
        }
        """)
        .HasIssue(Issue.WRN("Proj6012", "No valid SDK version roll-forward policy has been specified").WithSpan(03, 19, 03, 20));


    /// <remarks>
    /// The value version is not trimmed.
    /// </remarks>
    [Test]
    public void invalid_version() => new SpecifySdkRollForwardPolicy().ForInlineGlobalJson("""
        {
          "sdk": {
            "version": "10.0.400",
            "rollForward": "Whatever",
            "allowPrerelease": false
          }
        }
        """)
        .HasIssue(Issue.WRN("Proj6012", "No valid SDK version roll-forward policy has been specified").WithSpan(03, 19, 03, 29));
}

public class Guards
{
    [Test]
    public void Specfied_version() => new SpecifySdkRollForwardPolicy().ForInlineGlobalJson("""
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
