using DotNetProjectFile.Analyzers.GlobalJson;

namespace Rules.GlobalJson.SDK_version_roll_forward_policy_should_be_specified;

public class Reports
{
    [Test]
    public void Different_casing() => new SpecifySdkRollForwardPolicy().ForInlineGlobalJson("""
        {
          "SDK": {
            "version": "10.0.400",
            "rollForward": "LATESTPATCH",
            "allowPrerelease": false
          }
        }
        """)
        .HasIssue(Issue.WRN("Proj6012", "No valid SDK version roll-forward policy has been specified"));

    [Test]
    public void numeric_value() => new SpecifySdkRollForwardPolicy().ForInlineGlobalJson("""
        {
          "sdk": {
            "version": "10.0.400",
            "rollForward": 4,
            "allowPrerelease": false
          }
        }
        """)
        .HasIssue(Issue.WRN("Proj6012", "No valid SDK version roll-forward policy has been specified").WithSpan(03, 19, 03, 20));

    [Test]
    public void invalid_value() => new SpecifySdkRollForwardPolicy().ForInlineGlobalJson("""
        {
          "sdk": {
            "version": "10.0.400",
            "rollForward": "Whatever",
            "allowPrerelease": false
          }
        }
        """)
        .HasIssue(Issue.WRN("Proj6012", "No valid SDK version roll-forward policy has been specified").WithSpan(03, 19, 03, 29));

    [Test]
    public void int_string_value() => new SpecifySdkRollForwardPolicy().ForInlineGlobalJson("""
        {
          "sdk": {
            "version": "10.0.400",
            "rollForward": "1",
            "allowPrerelease": false
          }
        }
        """)
        .HasIssue(Issue.WRN("Proj6012", "No valid SDK version roll-forward policy has been specified").WithSpan(03, 19, 03, 22));

    [TestCase("patch, major", 33)]
    [TestCase(" latestPatch ", 34)]
    public void not_a_policy_name(string value, int end) => new SpecifySdkRollForwardPolicy().ForInlineGlobalJson($$"""
        {
          "sdk": {
            "version": "10.0.400",
            "rollForward": "{{value}}",
            "allowPrerelease": false
          }
        }
        """)
        .HasIssue(Issue.WRN("Proj6012", "No valid SDK version roll-forward policy has been specified").WithSpan(03, 19, 03, end));
}

public class Guards
{
    [Test]
    public void Specified_policy() => new SpecifySdkRollForwardPolicy().ForInlineGlobalJson("""
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
