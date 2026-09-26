using DotNetProjectFile.Analyzers.GlobalJson;

namespace Rules.GlobalJson.SDK_version_roll_forward_policy_should_be_specified;

public class Reports
{
    [Test]
    public void other_then_disable_with_lock_files() => new SpecifySdkRollForwardPolicy().ForInlineGlobalJson("""
        {
          "sdk": {
            "version": "10.0.400",
            "rollForward": "LATESTPATCH"
          }
        }
        """)
        .WithBuildProperty("RestorePackagesWithLockFile", "true")
        .HasIssue(Issue.WRN("Proj6013", "Disable the SDK version roll-forward").WithSpan(3, 19, 3, 32));

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
        .WithBuildProperty("RestorePackagesWithLockFile", "false")
        .HasNoIssues();

    /// <remarks>
    /// The version has to be updated with a new version, because the build
    /// will fail otherwise (logically).
    /// </remarks>
    [Test]
    public void disabled_with_lock_files() => new SpecifySdkRollForwardPolicy().ForInlineGlobalJson("""
        {
          "sdk": {
            "version": "10.0.401",
            "rollForward": "disable",
            "allowPrerelease": false
          }
        }
        """)
        .WithBuildProperty("RestorePackagesWithLockFile", "true")
        .HasNoIssues();
}
