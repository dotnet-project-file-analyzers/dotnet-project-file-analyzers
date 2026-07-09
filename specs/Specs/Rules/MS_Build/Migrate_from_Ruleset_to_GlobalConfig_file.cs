namespace Rules.MS_Build.Migrate_from_Ruleset_to_GlobalConfig_file;

public class Reports
{
    [Test]
    public void private_assets_not_being_private()
         => new MigrateFromRulesetToGlobalConfigFile()
         .ForProject("WithRuleset.cs")
         .HasIssue(Issue.WRN("Proj0025", @"Migrate ruleset 'MyPreferences.ruleset' to a .globalconfig file")
         .WithSpan(04, 04, 04, 68));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_with_analyzers(string project)
         => new MigrateFromRulesetToGlobalConfigFile()
        .ForProject(project)
        .HasNoIssues();
}

